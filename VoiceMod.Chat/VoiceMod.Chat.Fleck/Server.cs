﻿using Fleck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VoiceMod.Chat.Abstractions;

namespace VoiceMod.Chat.Fleck
{
    public class Server : IChatCommunication
    {
        private const string Url = "ws://127.0.0.1";

        private readonly IMessageText _msgText;
        private readonly int _port;

        private IList<IWebSocketConnection> ConnectedSockets { get; set; } = new List<IWebSocketConnection>();


        public Server(IMessageText msgText, int port)
        {
            _msgText = msgText ?? throw new ArgumentNullException(nameof(msgText));
            _port = port;
        }

        public async Task Initialize()
        {
            var input = string.Empty;

            var server = new WebSocketServer($"{Url}:{_port}");
            await Task.Run(() =>
            server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    _msgText.Show($"New client connected: {socket.ConnectionInfo.ClientIpAddress}");
                    ConnectedSockets.Add(socket);
                };
                socket.OnClose = () =>
                {
                    _msgText.Show($"Client disconnected: {socket.ConnectionInfo.ClientIpAddress}");
                    ConnectedSockets.Remove(socket);
                };
                socket.OnMessage = message =>
                {
                    _msgText.Show(message);
                    ConnectedSockets.ToList().ForEach(s => s.Send(message));
                };
            }));
        }

        public async Task SendMessage(string message)
        {
            foreach (var socket in ConnectedSockets)
            {
                await Task.Run(() => socket.Send(message));
            }
        }
    }
}