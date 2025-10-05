using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.Messages.Commands.SimpleCommands
{
    internal class LoveCommand : BaseSimpleCommand
    {
        public override string Name => "love";

        public override string Description => "Expresses love between Bubu and Dudu.";

        protected override string CreateMessageToSend(SocketMessage message)
            => $"Bubu loves Dudu! ❤️ Dudu loves Bubu! ❤️";
    }
}
