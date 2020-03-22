using Autofac;
using VoiceMod.Chat.Abstractions;

namespace VoiceMod.Chat.Bootstrap
{
    public static class MessageTextExtensions
    {
        public static ContainerBuilder CreateMessageText(this ContainerBuilder containerBuilder, IMessageText messageText)
        {
            containerBuilder.RegisterInstance(messageText).As<IMessageText>();

            return containerBuilder;
        }
    }
}