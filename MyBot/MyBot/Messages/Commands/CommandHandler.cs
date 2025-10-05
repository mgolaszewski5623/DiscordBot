using Discord.WebSocket;
using MyBot.Exceptions;
using MyBot.Messages.Commands.ParametrizedCommands;
using MyBot.Messages.Commands.SimpleCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.Messages.Commands
{
    internal class CommandHandler
    {
        private readonly string prefix;

        private readonly List<ISimpleCommand> simpleCommands = new()
        {
            new LoveCommand(),
            new HelloCommand(),
            new InfoCommand(),
            new HelpCommand(),
            new PingCommand(),
            new JokeCommand(),
            new MemeCommand(),
            new QuoteCommand(),
            new ServerInfoCommand(),
            new TimeCommand(),
            // Todo: Add instances of classes implementing ISingleCommand here
        };

        private readonly List<IParametrizedCommand> parametrizedCommands = new()
        {
            // Todo: Add instances of classes implementing IParamCommand here
        };

        public CommandHandler(string prefix)
        {
            this.prefix = prefix;
        }

        public async Task HandleCommand(SocketMessage message)
        {
            try
            {
                RejestrCommand();
                var messageParts = message.Content.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (messageParts.Length == 1)
                    await HandleSingleCommand(message);
                else
                    await HandleParamCommand(message, messageParts);
            }
            catch (Exception ex)
            {
                throw new MyBotException("Error handling command", ex);
            }
        }

        private void RejestrCommand()
        {
            // Todo: Implement command registration logic here
        }

        private async Task HandleSingleCommand(SocketMessage message)
        {
            var command = simpleCommands.FirstOrDefault(c => $"{prefix}{c.Name}" == message.Content);
            if (command != null)
                await command.Execute(message);
            else
                HandleUnknownCommand(message.Content);
        }

        private async Task HandleParamCommand(SocketMessage message, string[] messageParts)
        {
            var commandName = messageParts[0];
            var args = messageParts.Skip(1).ToArray();
            var command = parametrizedCommands.FirstOrDefault(c => $"{prefix}{c.Name}" == commandName);
            if (command != null)
                await command.Execute(message, args);
            else
                HandleUnknownCommand(message.Content);
        }

        private void HandleUnknownCommand(string message)
        {
            // Todo: Implement unknown command handling logic here
        }
    }
}
