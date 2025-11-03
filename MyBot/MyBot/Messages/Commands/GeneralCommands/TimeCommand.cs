using Discord.WebSocket;
using MyBot.Messages.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.Messages.Commands.GeneralCommands
{
    internal class TimeCommand : BaseCommand
    {
        public override string Name => "time";

        public override string Description => "Displays the current server time.";

        protected override bool AllowParameters => false;

        protected override Task<object> CreateMessageToSend(SocketMessage message, string[]? parameters = null)
            => Task.FromResult<object>($"Current server time is: {DateTime.Now.ToString("F")}");
    }
}
