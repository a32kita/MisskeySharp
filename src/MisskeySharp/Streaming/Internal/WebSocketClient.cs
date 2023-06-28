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
        }

        public void Send(string data)
        {
            if (this._receivingProcess != null && this._receivingProcess.IsReceivingNow || this._clientWebSocket.State != WebSocketState.Open)
            {
                while (this._receivingProcess.IsReceivingNow || this._clientWebSocket.State != WebSocketState.Open)
                {
                    Thread.Sleep(1);
                }
            }

            Task.Run(async () => await this._clientWebSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(data)), WebSocketMessageType.Text, true, CancellationToken.None)).Wait();
        }

        public void Close()
        {
            if (this._receivingThread != null)
                this._receivingThread.Abort();
        }


        public void Dispose()
        {
            this._clientWebSocket.Dispose();
        }


        private class ReceivingProcess
        {
            private Action<string> _received;
            private Action _connectionClosed;


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

                this.ClientWebSocket = clientWebSocket;
                this.ConnectionUri = connectionUri;
                this.ConnectionCancellationToken = cancellationToken;
                this.IsReceivingNow = false;
            }


            public void Worker()
            {
                this.ClientWebSocket.ConnectAsync(this.ConnectionUri, this.ConnectionCancellationToken).Wait();
                var buffer = new byte[1024 * 16];

                while (true)
                {
                    //Console.WriteLine("Connection: Waiting ...");
                    var segment = new ArraySegment<byte>(buffer);
                    var result = Task.Run(async () => await this.ClientWebSocket.ReceiveAsync(segment, CancellationToken.None)).Result;
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        Task.Run(async () => await this.ClientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "OK", CancellationToken.None)).Wait();
                        //Console.WriteLine("Connection: Closed");
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
                            //Console.WriteLine("Connection: Closed (Payload is too long)");
                            this._connectionClosed();
                            return;
                        }

                        segment = new ArraySegment<byte>(buffer, byteCount, buffer.Length - byteCount);
                        result = Task.Run(async () => await this.ClientWebSocket.ReceiveAsync(segment, CancellationToken.None)).Result;

                        byteCount += result.Count;
                        //Console.WriteLine("Connection: Receiving {0} bytes", byteCount);
                    }

                    var data = Encoding.UTF8.GetString(buffer, 0, byteCount);
                    this._received(data);

                    this.IsReceivingNow = false;
                }
            }
        }
    }
}
