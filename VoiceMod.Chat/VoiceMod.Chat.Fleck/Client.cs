using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using VoiceMod.Chat.Abstractions;

namespace VoiceMod.Chat.Fleck
{
    public class Client : IChatCommunication
    {
        private const string Url = "ws://localhost";
        private readonly int _port;

        private byte[] _bytes = new byte[1024];
        private ClientWebSocket _client;
        private string NickName { get; set; } = string.Empty;

        public Client(int port)
        {
            _port = port;
        }

        public void Initialize()
        {
            var tokSrc = new CancellationTokenSource();

            do
            {
                Console.WriteLine("Please write your nickname: ");
                NickName = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(NickName));

            _client = new ClientWebSocket();
            var task = _client.ConnectAsync(new Uri($"{Url}:{_port}"), tokSrc.Token);
            task.Wait(); task.Dispose();

            ListenToMessages();
        }


        public void SendMessage(string message)
        {
            var tokSrc = new CancellationTokenSource();

            var task = _client.SendAsync(
                       new ArraySegment<byte>(Encoding.UTF8.GetBytes($"{NickName}: {message}")),
                       WebSocketMessageType.Text,
                       false,
                       tokSrc.Token
                   );
            task.Wait(); task.Dispose();
        }


        private void ListenToMessages()
        {
            WebSocketReceiveResult result;
            using (var cts = new CancellationTokenSource())
            {

                byte[] buffer = new byte[8192];
                var segment = new ArraySegment<byte>(buffer, 0, buffer.Length);
                using (var ms = new MemoryStream())
                {
                    do
                    {
                        result = _client.ReceiveAsync(segment, cts.Token).GetAwaiter().GetResult();
                        ms.Write(segment.Array, segment.Offset, result.Count);
                    } while (!result.EndOfMessage);

                    ms.Seek(0, SeekOrigin.Begin);

                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        using (var reader = new StreamReader(ms, Encoding.UTF8))
                        {
                            Console.WriteLine($"Server sent: {reader.ReadToEnd()}");
                        }
                    }
                }
            }
        }
    }
}