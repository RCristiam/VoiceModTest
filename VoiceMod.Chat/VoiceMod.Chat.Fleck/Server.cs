using Fleck;
using System;
using System.Collections.Generic;
using System.Linq;
using VoiceMod.Chat.Abstractions;

namespace VoiceMod.Chat.Fleck
{
    public class Server : IChatCommunication
    {
        private const string Url = "ws://127.0.0.1";
        private readonly int _port;

        private IList<IWebSocketConnection> ConnectedSockets { get; set; } = new List<IWebSocketConnection>();

     
        public Server(int port)
        {
            _port = port;
        }

        public void Initialize()
        {
            var input = string.Empty;

            var server = new WebSocketServer($"{Url}:{_port}");
            server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    Console.WriteLine("Open!");
                    ConnectedSockets.Add(socket);
                };
                socket.OnClose = () =>
                {
                    Console.WriteLine("Close!");
                    ConnectedSockets.Remove(socket);
                };
                socket.OnMessage = message =>
                {
                    Console.WriteLine(message);
                    ConnectedSockets.ToList().ForEach(s => s.Send("Echo: " + message));
                };
            });
        }


        public void SendMessage(string message)
        {
            foreach (var socket in ConnectedSockets)
            {
                socket.Send(message);
            }
        }

    }
}
