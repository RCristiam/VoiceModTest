namespace VoiceMod.Chat.Abstractions
{
    public interface IChatCommunication
    {
        void Initialize();

        void SendMessage(string message);
    }
}
