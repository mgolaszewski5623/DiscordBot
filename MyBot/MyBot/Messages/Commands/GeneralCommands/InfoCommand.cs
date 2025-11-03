using Discord.WebSocket;
using MyBot.Messages.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.Messages.Commands.GeneralCommands
{
    internal class InfoCommand : BaseCommand
    {
        public override string Name => "info";

        public override string Description => "Provides information about the bot.";

        protected override bool AllowParameters => false;

        protected override Task<object> CreateMessageToSend(SocketMessage message, string[]? parameters = null)
            => Task.FromResult<object>("I am a friendly bot here to assist you! Type <prefix>help to see what I can do.");
    }
}
