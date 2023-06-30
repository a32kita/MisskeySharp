using System.Text;
using System.Text.Json;

using WebSocket4Net;

namespace MisskeySharp.StreamingCT
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("取得済みのアクセストークンを入力してください。");
            Console.Write(">");
            var accessToken = Console.ReadLine();

            var uri = $"wss://misskey.io/streaming";
            if (String.IsNullOrEmpty(accessToken) == false)
            {
                uri += $"?i={accessToken}";
            }

            var sock = new WebSocket(uri);

            sock.DataReceived += (sender, e) =>
            {
                Console.WriteLine("Received !!");
                Console.WriteLine(Encoding.UTF8.GetString(e.Data));
                Console.WriteLine();
            };

            sock.MessageReceived += (sender, e) =>
            {
                Console.WriteLine("Message received !!");
                Console.WriteLine(e.Message);
                Console.WriteLine();
            };

            sock.Error += (sender, e) =>
            {
                Console.WriteLine("Error occured");
                Console.WriteLine(e.Exception.Message);
                Console.WriteLine();
            };

            sock.Open();
            while (sock.State != WebSocketState.Open)
            {
                Task.Run(async () =>
                {
                    await Task.Delay(500);
                    Console.WriteLine("Wait for connection");
                }).Wait();
            }

            var conReq = s_serialize(new ConnectionRequest()
            {
                Type = "connect",
                Body = new ConnectionRequest.BodyObject()
                {
                    Channel = "localTimeline",
                    Id = Guid.NewGuid().ToString(),
                }
            });

            //conReq = "test";

            sock.Send(conReq);
            Console.WriteLine(conReq);

            while (sock.State != WebSocketState.Closed)
            {
                Task.Run(async () =>
                {
                    await Task.Delay(500);
                    Console.WriteLine("State: {0}", sock.State);
                }).Wait();


            }
        }

        private static string s_serialize<T>(T obj)
        {
            return JsonSerializer.Serialize(obj, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
    }
}