using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.Messages.Commands.SimpleCommands
{
    internal class InfoCommand : BaseSimpleCommand
    {
        public override string Name => "info";

        public override string Description => "Provides information about the bot.";

        protected override string CreateMessageToSend(SocketMessage message)
            => "I am a friendly bot here to assist you! Type <prefix>help to see what I can do.";
    }
}
