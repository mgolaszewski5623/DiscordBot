using Discord.WebSocket;
using MyBot.DataManager;
using MyBot.Enums;
using MyBot.Messages.Commands.Base;
using MyBot.Messages.Commands.FunCommands;
using MyBot.Messages.Commands.GeneralCommands;
using MyBot.Messages.Commands.ModerationCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyBot.Messages.Commands
{
    internal class CommandHandler
    {
        private readonly string prefix;

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
                string commandName = messageParts[0];
                string[] messageArgs = messageParts.Skip(1).ToArray();

                IMyBotCommand? command = CommandManager.GetCommand(commandName, prefix);
                if (command != null)
                    await command.Execute(message, messageArgs);
                else
                    await HandleUnknownCommand(message);
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

        private async Task HandleUnknownCommand(SocketMessage message)
        {
            await message.Channel.SendMessageAsync("❓ Unknown command. Type !help to see the list of available commands.");
        }
    }
}
