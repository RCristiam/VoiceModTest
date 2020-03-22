using System.Threading.Tasks;

namespace VoiceMod.Chat.Abstractions
{
    public interface IChatCommunication
    {
        Task Initialize();

        Task SendMessage(string message);
    }
}
