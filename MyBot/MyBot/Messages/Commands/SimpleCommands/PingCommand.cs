using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.Messages.Commands.SimpleCommands
{
    internal class PingCommand : BaseSimpleCommand
    {
        public override string Name => "ping";

        public override string Description => "Responds with 'Pong!' to check if the bot is responsive.";

        protected override Task<object> CreateMessageToSend(SocketMessage message)
            => Task.FromResult<object>("Pong!");
    }
}
