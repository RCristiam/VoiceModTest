# VoiceModTest

This repository belongs to a technical interview for Voicemod. The basic structure is as following below:
* A project for the abstractions (interfaces)
* A project to resolve the abstractions (Bootstrap)
* A Console Application project (Executable)
* A project to create a chat server and client using Fleck


## VoiceMod.Chat.Abstractions

Contains all interfaces for loose coupling (SOL**I**D).

## VoiceMod.Chat.Bootstrap

The Bootstrap responsability is to resolve all interfaces. The IoC Container chosen is Autofac due to its lightness and the one that I have been working lately.

### Autofac
Autofac is an addictive IoC container for .NET. It manages the dependencies between classes so that applications stay easy to change as they grow in size and complexity. This is achieved by treating regular .NET classes as components.

There are two extensions methods: 

* One to resolve the IChatCommunication using factory method pattern;
* Other to resolve the IMessageText. 

## VoiceMod.Chat.Executable

The executable of the application using .NET Core 2.2.

## VoiceMod.Chat.Fleck

Server and client implementation.

Server is implemented using [Fleck](https://github.com/statianzo/Fleck).

Client implementation uses [ClientWebSocket](https://docs.microsoft.com/en-us/dotnet/api/system.net.websockets.clientwebsocket?view=netcore-2.2)

## Technical Debts
* xUnit test cases
* move Client class to another project, since it is not implementing Fleck

## Next steps

In a hypothetical real world scenario, the next steps in this development would be:
* Create a WPF application for the chat
* Include logs using [log4net](https://logging.apache.org/log4net/) or similar 
 
