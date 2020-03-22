using Autofac;
using System;
using VoiceMod.Chat.Abstractions;

namespace VoiceMod.Chat.Bootstrap
{
    public static class MessageTextExtensions
    {
        public static ContainerBuilder CreateMessageText(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<MessageText>().As<IMessageText>();

            return containerBuilder;
        }


        public class MessageText : IMessageText
        {
            public void Show(string text)
            {
                Console.WriteLine(text);
            }
        }
    }
}
