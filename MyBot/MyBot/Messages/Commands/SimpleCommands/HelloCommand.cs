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

        protected override Task<object> CreateMessageToSend(SocketMessage message)
            => Task.FromResult<object>($"Hello, {message.Author.Mention}! 👋");
    }
}
