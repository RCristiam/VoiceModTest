using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VoiceMod.Chat.Abstractions;

namespace VoiceMod.Chat.Fleck
{
    public class Client : IChatCommunication
    {
        private const string Url = "ws://localhost";

        private readonly IMessageText _msgText;
        private readonly int _port;

        private ClientWebSocket _client;

        public Client(IMessageText messageText, int port)
        {
            _msgText = messageText ?? throw new ArgumentNullException(nameof(messageText));
            _port = port;
        }

        private string NickName { get; set; } = string.Empty;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task Initialize()
        {
            var tokSrc = new CancellationTokenSource();

            do
            {
                _msgText.Show("Please write your nickname: ");
                NickName = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(NickName));

            _client = new ClientWebSocket();
            await _client.ConnectAsync(new Uri($"{Url}:{_port}"), tokSrc.Token);

            if (_client.State == WebSocketState.Open)
            {
                _msgText.Show("Connected successfully.");
            }

            await Task.WhenAll(ListenToMessages());
        }

        public async Task SendMessage(string message)
        {
            var tokSrc = new CancellationTokenSource();
            await _client.SendAsync(
                   new ArraySegment<byte>(Encoding.UTF8.GetBytes($"{NickName}: {message}")),
                   WebSocketMessageType.Text,
                   true,
                   tokSrc.Token
               );
        }

        private async Task ListenToMessages()
        {
            WebSocketReceiveResult result;
            while (_client.State == WebSocketState.Open)
            {
                using (var cts = new CancellationTokenSource())
                {
                    byte[] buffer = new byte[8192];
                    var segment = new ArraySegment<byte>(buffer, 0, buffer.Length);
                    using (var ms = new MemoryStream())
                    {
                        do
                        {
                            result = await _client.ReceiveAsync(segment, cts.Token);
                            ms.Write(segment.Array, segment.Offset, result.Count);
                        } while (!result.EndOfMessage);

                        ms.Seek(0, SeekOrigin.Begin);

                        switch (result.MessageType)
                        {
                            case WebSocketMessageType.Close:
                                await _client.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                                break;
                            case WebSocketMessageType.Text:
                                using (var reader = new StreamReader(ms, Encoding.UTF8))
                                {
                                    _msgText.Show($"{reader.ReadToEnd()}");
                                }
                                break;
                        }
                    }
                }
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                var task = _client.CloseAsync(WebSocketCloseStatus.NormalClosure, $"{_client} Disconnecting", CancellationToken.None);
                task.Wait(); task.Dispose();

                _msgText.Show("Disconnected from the server.");
            }
        }
    }
}