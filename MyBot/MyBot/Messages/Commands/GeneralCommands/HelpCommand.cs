using Discord.WebSocket;
using MyBot.DataManager;
using MyBot.Messages.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.Messages.Commands.GeneralCommands
{
    internal class HelpCommand : BaseCommand
    {
        public override string Name => "help";

        public override string Description => "Provides a list of available commands.";

        protected override bool AllowParameters => false;

        protected override Task<object> CreateMessageToSend(SocketMessage message, string[]? parameters = null)
        {
            StringBuilder commandsList = new StringBuilder();
            List<IMyBotCommand> allCommands = CommandManager.GetAllCommands();
            commandsList.AppendLine("Here are the available commands:");
            foreach (IMyBotCommand command in allCommands)
                commandsList.AppendLine($"{message.Content[0]}{command.Name} - {command.Description}");
            return Task.FromResult<object>(commandsList.ToString());
        }
    }
}
