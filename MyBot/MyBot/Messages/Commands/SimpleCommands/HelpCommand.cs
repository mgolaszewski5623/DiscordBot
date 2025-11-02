using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.Messages.Commands.SimpleCommands
{
    internal class HelpCommand : BaseSimpleCommand
    {
        public override string Name => "help";

        public override string Description => "Provides a list of available commands.";

        protected override Task<object> CreateMessageToSend(SocketMessage message)
        {
            StringBuilder commandsList = new StringBuilder();
            commandsList.AppendLine("Here are the available commands:");
            commandsList.AppendLine("!hello - Sends a greeting message to the user.");
            commandsList.AppendLine("!love - Sends a love message.");
            commandsList.AppendLine("!info - Provides information about the bot.");
            return Task.FromResult<object>(commandsList.ToString());
        }
    }
}
