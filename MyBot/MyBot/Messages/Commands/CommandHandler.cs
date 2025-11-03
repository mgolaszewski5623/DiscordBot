using Discord.WebSocket;
using MyBot.DataManager;
using MyBot.Enums;
using MyBot.Exceptions;
using MyBot.Extensions;
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
        };

        private readonly List<IParametrizedCommand> parametrizedCommands = new()
        {
            new ClearCommand(),
            new UserInfoCommand(),
            new WarnCommand(),
            new KickCommand(),
            new UserWarningsCommand(),
        };

        public CommandHandler(string prefix)
        {
            this.prefix = prefix;
        }

        public async Task HandleCommand(SocketMessage message)
        {
            try
            {
                await RejestrCommand(message);
                string[] messageParts = message.Content.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (messageParts.Length == 1)
                    await HandleSingleCommand(message);
                else
                    await HandleParamCommand(message, messageParts);
            }
            catch (Exception ex)
            {
                await LogManager.LogException(ex, ExceptionType.ERROR);
            }
        }

        private async Task RejestrCommand(SocketMessage message)
        {
            await LogManager.LogCommand(message);
        }

        private async Task HandleSingleCommand(SocketMessage message)
        {
            ISimpleCommand? command = simpleCommands.FirstOrDefault(c => $"{prefix}{c.Name}" == message.Content);
            if (command != null)
                await command.Execute(message);
            else
                HandleUnknownCommand(message.Content);
        }

        private async Task HandleParamCommand(SocketMessage message, string[] messageParts)
        {
            string commandName = messageParts[0];
            string[] args = messageParts.Skip(1).ToArray();
            IParametrizedCommand? command = parametrizedCommands.FirstOrDefault(c => $"{prefix}{c.Name}" == commandName);
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
