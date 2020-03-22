using Autofac;
using System;
using System.Linq;
using VoiceMod.Chat.Abstractions;
using VoiceMod.Chat.Bootstrap;

namespace VoiceMod.Chat.Executable
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = -1;
            var input = string.Empty;
            if (args.Any() == false || int.TryParse(args.First(), out port) == false)
            {
                do
                {
                    Console.WriteLine($"Type the port and press enter to continue:  ");
                    input = Console.ReadLine();
                } while (int.TryParse(input, out port) == false);

                IContainer builder = CreateAutofacBuilder();

                var factory = builder.Resolve<Func<int, IChatCommunication>>();

                var comm = factory.Invoke(port);

                comm.Initialize().GetAwaiter().GetResult();
                Console.Write($"--- Write any text (type exit to quit) --- {Environment.NewLine}");
                input = Console.ReadLine();
                while (input != "exit")
                {
                    comm.SendMessage(input);

                    input = Console.ReadLine();
                }

                comm.Dispose();
            }
        }

        private static IContainer CreateAutofacBuilder() =>
            new ContainerBuilder()
            .CreateCommunicationFactory()
            .CreateMessageText(new MessageText())
            .Build();

    }

    internal class MessageText : IMessageText
    {
        public void Show(string text)
        {
            Console.WriteLine(text);
        }
    }
}
