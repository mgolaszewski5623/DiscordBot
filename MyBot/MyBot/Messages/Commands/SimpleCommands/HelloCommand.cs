using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.Messages.Commands.SimpleCommands
{
    internal class HelloCommand : BaseSimpleCommand
    {
        public override string Name => "hello";

        public override string Description => "Sends a greeting message to the user.";

        protected override string CreateMessageToSend(SocketMessage message)
            => $"Hello, {message.Author.Mention}! 👋";
    }
}
