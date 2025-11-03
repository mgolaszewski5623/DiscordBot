using Discord.WebSocket;
using MyBot.Messages.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.Messages.Commands.GeneralCommands
{
    internal class HelloCommand : BaseCommand
    {
        public override string Name => "hello";

        public override string Description => "Sends a greeting message to the user.";

        protected override bool AllowParameters => false;

        protected override Task<object> CreateMessageToSend(SocketMessage message, string[]? parameters = null)
            => Task.FromResult<object>($"Hello, {message.Author.Mention}! 👋");
    }
}
