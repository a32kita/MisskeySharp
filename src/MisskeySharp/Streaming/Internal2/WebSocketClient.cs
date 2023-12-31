﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MisskeySharp.Streaming.Internal2
{
    internal class WebSocketClient : IDisposable
    {
        private bool _isDisposed;
        private ClientWebSocket _clientWebSocket;
        private Task _receptionTask;
        private bool _receptionTaskCanceled;
        private CancellationTokenSource _receptionTaskReciWaitingTaskCancellationTokenSource;
        private Uri _currentConnectionUri;


        public event EventHandler<WebSocketReceivedEventArgs> Received;

        public event EventHandler ConnectionClosed;


        public WebSocketClientState State
        {
            get;
            private set;
        }

        public Encoding MessageEncoding
        {
            get;
            private set;
        }


        public WebSocketClient(Encoding messageEncoding)
        {
            this._isDisposed = false;
            this._clientWebSocket = new ClientWebSocket();
            this._receptionTask = null;

            this.State = WebSocketClientState.Disconnected;
            this.MessageEncoding = messageEncoding;
        }


        private void _checkDisposed()
        {
            if (this._isDisposed == false)
                return;

            throw new ObjectDisposedException(nameof(WebSocketClient));
        }

        private void _receptionProcess()
        {
            this.State = WebSocketClientState.Connecting;
            this._clientWebSocket.ConnectAsync(this._currentConnectionUri, CancellationToken.None).Wait();

            //if (this._clientWebSocket.State != WebSocketState.Open)
            //{
            //    Thread.Sleep(100);
            //}

            this.State = WebSocketClientState.Connected;

            var receive = new Func<ArraySegment<byte>, WebSocketReceiveResult>(segment =>
            {
                try
                {
                    return Task.Run(async () => {
                        try
                        {
                            return await this._clientWebSocket.ReceiveAsync(segment, this._receptionTaskReciWaitingTaskCancellationTokenSource.Token);
                        }
                        catch (TaskCanceledException)
                        {
                            return null;
                        }
                    }).Result;
                }
                catch (TaskCanceledException)
                {
                    return null;
                }
            });

            var buffer = new byte[1024 * 32];
            while (true)
            {
                var segment = new ArraySegment<byte>(buffer);
                var result = receive(segment);

                if (result == null || result.MessageType == WebSocketMessageType.Close || this._receptionTaskCanceled)
                {
                    try
                    {
                        Task.Run(async () =>await this._clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "OK", CancellationToken.None)).Wait();
                    }
                    catch { }

                    this.State = WebSocketClientState.Disconnected;
                    this.ConnectionClosed?.Invoke(this, EventArgs.Empty);
                    return;
                }

                var byteCount = result.Count;
                while (result.EndOfMessage == false)
                {
                    if (byteCount >= buffer.Length)
                    {
                        Task.Run(async () => await this._clientWebSocket.CloseAsync(
                                WebSocketCloseStatus.InvalidPayloadData, "Payload is too long", CancellationToken.None)).Wait();
                        this.State = WebSocketClientState.Disconnected;
                        this.ConnectionClosed?.Invoke(this, EventArgs.Empty);
                        return;
                    }

                    if (this._receptionTaskCanceled)
                    {
                        try
                        {
                            Task.Run(async () => await this._clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "OK", CancellationToken.None)).Wait();
                        }
                        catch { }

                        this.State = WebSocketClientState.Disconnected;
                        this.ConnectionClosed?.Invoke(this, EventArgs.Empty);
                        return;
                    }

                    segment = new ArraySegment<byte>(buffer, byteCount, buffer.Length - byteCount);
                    result = Task.Run(async () => await this._clientWebSocket.ReceiveAsync(segment, CancellationToken.None)).Result;

                    byteCount += result.Count;
                }

                var data = this.MessageEncoding.GetString(buffer, 0, byteCount);
                this.Received?.Invoke(this, new WebSocketReceivedEventArgs(data));
            }
        }


        public void Open(Uri uri)
        {
            switch (uri.Scheme)
            {
                case "ws":
                case "wss":
                    break;
                default:
                    throw new NotSupportedException();
            }

            if (this._clientWebSocket.State == WebSocketState.Open || (this._receptionTask != null && this._receptionTask.Status != TaskStatus.Running))
            {
                throw new InvalidOperationException();
            }

            this._currentConnectionUri = uri;
            this._receptionTaskCanceled = false;
            this._receptionTaskReciWaitingTaskCancellationTokenSource = new CancellationTokenSource();
            this._receptionTask = Task.Run(
                () => this._receptionProcess());
        }

        public async Task<bool> WaitForConnectedAsync(int timeout)
        {
            await Task.WhenAny(
                Task.Run(async () =>
                {
                    while (this.State != WebSocketClientState.Connected)
                    {
                        await Task.Delay(1);
                    }
                }),
                Task.Delay(timeout));

            return this.State == WebSocketClientState.Connected;
        }

        public void Send(string data)
        {
            if (this.State == WebSocketClientState.Connected == false)
                throw new InvalidOperationException($"{nameof(WebSocketClient)} is not connected.");

            Task.Run(async () => await this._clientWebSocket.SendAsync(new ArraySegment<byte>(this.MessageEncoding.GetBytes(data)), WebSocketMessageType.Text, true, CancellationToken.None)).Wait();
        }

        public void Close()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            this._receptionTaskCanceled = true;
            this._receptionTaskReciWaitingTaskCancellationTokenSource.Cancel();
        }
    }
}
