using Autofac;
using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using VoiceMod.Chat.Abstractions;
using VoiceMod.Chat.Fleck;

namespace VoiceMod.Chat.Bootstrap
{
    public static class CommunicationFactoryExtensions
    {
        public static ContainerBuilder CreateCommunicationFactory(this ContainerBuilder containerBuilder)
        {
            containerBuilder.Register((Func<IComponentContext, Func<int, IChatCommunication>>)(c =>
            {
                var context = c.Resolve<IComponentContext>();
                return (port) =>
                {
                    if (IsPortInUse(port))
                    {
                        return new Client(port);
                    }
                    else
                    {
                        return new Server(port);
                    }
                };
            }));

            return containerBuilder;
        }

        private static bool IsPortInUse(int port)
        {
            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();

            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();

            return ipEndPoints.Any(iep => iep.Port == port);
        }

    }
}
