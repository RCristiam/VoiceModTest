using Microsoft.Extensions.Logging;

namespace VoiceMod.Chat.Abstractions
{
    public interface IMessageText
    {
        void Show(string text);
    }
}
