using System;
using System.Threading.Tasks;

namespace VoiceMod.Chat.Abstractions
{
    public interface IChatCommunication : IDisposable
    {
        Task Initialize();

        Task SendMessage(string message);
    }
}