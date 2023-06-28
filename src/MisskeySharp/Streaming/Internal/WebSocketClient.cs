using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MisskeySharp.Streaming.Internal
{
    internal class WebSocketClient : IDisposable
    {
        private ClientWebSocket _clientWebSocket;
        private CancellationTokenSource _connectionCancellationTokenSource;
        private event EventHandler<WebSocketReceivedEventArgs> _received;
        private event EventHandler _connectionClosed;

        private ReceivingProcess _receivingProcess;
        private Thread _receivingThread;

        public bool IsReceiveProcRunning
        {
            get => this._receivingThread == null ? false : this._receivingThread.IsAlive;
        }

        public event EventHandler<WebSocketReceivedEventArgs> Received
        {
            add => this._received += value;
            remove => this._received -= value;
        }

        public event EventHandler ConnectionClosed
        {
            add => this._connectionClosed += value;
            remove => this._connectionClosed -= value;
        }


        public WebSocketClient()
        {
            this._clientWebSocket = new ClientWebSocket();
            this._connectionCancellationTokenSource = new CancellationTokenSource();
        }



        public void Open(Uri uri)
        {
            if (this._receivingThread != null && this._receivingThread.IsAlive)
                throw new InvalidOperationException();

            this._receivingProcess = new ReceivingProcess(
                this._clientWebSocket, 
                uri, 
                this._connectionCancellationTokenSource.Token,
                data => this._received?.Invoke(this, new WebSocketReceivedEventArgs(data)),
                () => this._connectionClosed?.Invoke(this, new EventArgs()));

            this._receivingThread = new Thread(new ThreadStart(this._receivingProcess.Worker));
            this._receivingThread.Start();

            // 開始時の接続前にメッセージが送信される問題を防ぐ (強引)
            Thread.Sleep(500);
        }

        public void Send(string data)
        {
            if (this._receivingProcess != null && this._receivingProcess.IsReceivingNow)
            {
                while (this._receivingProcess.IsReceivingNow)
                {
                    Thread.Sleep(1);
                }
            }

            while (this._clientWebSocket.State != WebSocketState.Open)
            {
                Thread.Sleep(1);
            }

            Task.Run(async () => await this._clientWebSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(data)), WebSocketMessageType.Text, true, CancellationToken.None)).Wait();
        }

        public void Close()
        {
            if (this._receivingThread != null)
            {
                //this._receivingThread.Abort();
                this._receivingProcess.Cancel();
            }

            this._connectionCancellationTokenSource.Cancel();
            Thread.Sleep(100);
            Task.Run(async () => await this._clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Close", CancellationToken.None)).Wait();
        }


        public void Dispose()
        {
            this._clientWebSocket.Dispose();
        }


        private class ReceivingProcess
        {
            private Action<string> _received;
            private Action _connectionClosed;

            private bool _cancel;


            public ClientWebSocket ClientWebSocket
            {
                get;
                private set;
            }

            public Uri ConnectionUri
            {
                get;
                private set;
            }

            public CancellationToken ConnectionCancellationToken
            {
                get;
                protected set;
            }

            public bool IsReceivingNow
            {
                get;
                private set;
            }

            public ReceivingProcess(ClientWebSocket clientWebSocket, Uri connectionUri, CancellationToken cancellationToken, Action<string> received, Action connectionClosed)
            {
                this._received = received;
                this._connectionClosed = connectionClosed;

                this._cancel = false;

                this.ClientWebSocket = clientWebSocket;
                this.ConnectionUri = connectionUri;
                this.ConnectionCancellationToken = cancellationToken;
                this.IsReceivingNow = false;
            }

            public void Cancel()
            {
                this._cancel = true;
            }


            public void Worker()
            {
                this.ClientWebSocket.ConnectAsync(this.ConnectionUri, this.ConnectionCancellationToken).Wait();
                var buffer = new byte[1024 * 16];

                while (true)
                {
                    var segment = new ArraySegment<byte>(buffer);
                    var result = Task.Run(async () => await this.ClientWebSocket.ReceiveAsync(segment, CancellationToken.None)).Result;
                    if (result.MessageType == WebSocketMessageType.Close || this._cancel)
                    {
                        try
                        {
                            Task.Run(async () => await this.ClientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "OK", CancellationToken.None)).Wait();
                        }
                        catch { }

                        this._connectionClosed();
                        return;
                    }

                    this.IsReceivingNow = true;

                    var byteCount = result.Count;
                    while (result.EndOfMessage == false)
                    {
                        if (byteCount >= buffer.Length)
                        {
                            Task.Run(async () => await this.ClientWebSocket.CloseAsync(
                                WebSocketCloseStatus.InvalidPayloadData, "Payload is too long", CancellationToken.None)).Wait();
                            this._connectionClosed();
                            return;
                        }

                        if (this._cancel)
                        {
                            Task.Run(async () => await this.ClientWebSocket.CloseAsync(
                                WebSocketCloseStatus.InvalidPayloadData, "Operation cancelled", CancellationToken.None)).Wait();
                            this._connectionClosed();
                            return;
                        }

                        segment = new ArraySegment<byte>(buffer, byteCount, buffer.Length - byteCount);
                        result = Task.Run(async () => await this.ClientWebSocket.ReceiveAsync(segment, CancellationToken.None)).Result;

                        byteCount += result.Count;
                    }

                    var data = Encoding.UTF8.GetString(buffer, 0, byteCount);
                    this._received(data);

                    this.IsReceivingNow = false;
                }
            }
        }
    }
}
