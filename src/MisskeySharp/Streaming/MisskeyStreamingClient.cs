using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MisskeySharp.Streaming.Entities;
using MisskeySharp.Streaming.Internal;

namespace MisskeySharp.Streaming
{
    public class MisskeyStreamingClient : IDisposable
    {
        private MisskeyService _parent;
        private WebSocketClient _webSocketClient;
        private bool _isDisposed;


        public event EventHandler<MisskeyNoteReceivedEventArgs> NoteReceived;

        public event EventHandler ConnectionClosed;


        internal MisskeyStreamingClient(MisskeyService parent)
        {
            this._parent = parent;
            this._webSocketClient = new WebSocketClient(); ;
            this._isDisposed = false;

            this._webSocketClient.Received += (sender, e) =>
            {
                var json = e.Data;
                if (String.IsNullOrEmpty(json))
                {
                    // 良くないけど無視する
                    return;
                }

                NoteMessage noteMessage = null;
                try
                {
                    noteMessage = this._deserialize<NoteMessage>(json);
                }
                catch (Exception ex)
                {
                    throw new MisskeyException("Failed to parse the JSON data received in the streaming.", ex);
                }

                this.NoteReceived?.Invoke(this, new MisskeyNoteReceivedEventArgs()
                {
                    NoteMessage = noteMessage,
                });
            };
        }


        private void _checkDisposed()
        {
            if (this._isDisposed)
                throw new ObjectDisposedException(GetType().FullName);
        }

        private Uri _getConnectionUri()
        {
            var hostUri = new Uri(this._parent.Host);
            var actualHostName = hostUri.Host;
            return new Uri($"wss://{actualHostName}/streaming?i={this._parent.AccessToken}");
        }

        private string _serialize<T>(T value)
        {
            return JsonSerializer.Serialize<T>(value, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });
        }

        private T _deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            });
        }


        public void Connect(MisskeyStreamingChannels channels)
        {
            this._checkDisposed();
            var wsUri = this._getConnectionUri();

            var channelName = channels.ToString().ToLower()[0] + channels.ToString().Substring(1);
            var channelId = Guid.NewGuid().ToString();

            this._webSocketClient.Open(wsUri);
            this._webSocketClient.Send(this._serialize(new ConnectRequestParameter()
            {
                Type = "connect",
                Body = new ConnectRequestParameter.BodyObject()
                {
                    Channel = channelName,
                    channelId,
                }
            }));

            //while (true)
            //{
            //    Thread.Sleep(500);
            //    Console.WriteLine("Running: " + this._webSocketClient.IsReceiveProcRunning);
            //}
        }





        public void Dispose()
        {
            this._webSocketClient.Dispose();
            this._isDisposed = true;
        }
    }
}
