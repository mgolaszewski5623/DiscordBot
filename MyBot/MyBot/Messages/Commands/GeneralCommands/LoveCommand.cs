using Discord.WebSocket;
using MyBot.Messages.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.Messages.Commands.GeneralCommands
{
    internal class LoveCommand : BaseCommand
    {
        public override string Name => "love";

        public override string Description => "Expresses love between Bubu and Dudu.";

        protected override bool AllowParameters => false;

        protected override Task<object> CreateMessageToSend(SocketMessage message, string[]? parameters = null)
            => Task.FromResult<object>($"Bubu loves Dudu! ❤️ Dudu loves Bubu! ❤️");
    }
}
