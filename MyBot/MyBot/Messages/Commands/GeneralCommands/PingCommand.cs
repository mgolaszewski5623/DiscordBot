using Discord.WebSocket;
using MyBot.Messages.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.Messages.Commands.GeneralCommands
{
    internal class PingCommand : BaseCommand
    {
        public override string Name => "ping";

        public override string Description => "Responds with 'Pong!' to check if the bot is responsive.";

        protected override bool AllowParameters => false;

        protected override Task<object> CreateMessageToSend(SocketMessage message, string[]? parameters = null)
            => Task.FromResult<object>("Pong!");
    }
}
