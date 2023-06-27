﻿using System;
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

        private ReceivingProcess _receivingProcess;
        private Thread _receivingThread;

        public bool IsRunning
        {
            get => this._receivingThread == null ? false : this._receivingThread.IsAlive;
        }

        public event EventHandler<WebSocketReceivedEventArgs> Received
        {
            add => this._received += value;
            remove => this._received -= value;
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
                data => this._received?.Invoke(this, new WebSocketReceivedEventArgs(data)));

            this._receivingThread = new Thread(new ThreadStart(this._receivingProcess.Worker));
            this._receivingThread.Start();
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

            this._clientWebSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(data)), WebSocketMessageType.Text, true, CancellationToken.None);
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

            public ReceivingProcess(ClientWebSocket clientWebSocket, Uri connectionUri, CancellationToken cancellationToken, Action<string> received)
            {
                this._received = received;

                this.ClientWebSocket = clientWebSocket;
                this.ConnectionUri = connectionUri;
                this.ConnectionCancellationToken = cancellationToken;
                this.IsReceivingNow = false;
            }


            public void Worker()
            {
                this.ClientWebSocket.ConnectAsync(this.ConnectionUri, this.ConnectionCancellationToken).Wait();
                var buffer = new byte[1024];

                while (true)
                {
                    var segment = new ArraySegment<byte>(buffer);
                    var result = this.ClientWebSocket.ReceiveAsync(segment, CancellationToken.None).Result;
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        this.ClientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "OK", CancellationToken.None).Wait();
                        return;
                    }

                    this.IsReceivingNow = true;

                    var byteCount = result.Count;
                    while (result.EndOfMessage == false)
                    {
                        if (byteCount >= buffer.Length)
                        {
                            this.ClientWebSocket.CloseAsync(
                                WebSocketCloseStatus.InvalidPayloadData, "Payload is too long", CancellationToken.None).Wait();
                            return;
                        }

                        segment = new ArraySegment<byte>(buffer, byteCount, buffer.Length - byteCount);
                        result = this.ClientWebSocket.ReceiveAsync(segment, CancellationToken.None).Result;

                        byteCount += result.Count;
                    }

                    var json = Encoding.UTF8.GetString(buffer, 0, byteCount);
                    this._received(json);

                    this.IsReceivingNow = false;
                }
            }
        }
    }
}