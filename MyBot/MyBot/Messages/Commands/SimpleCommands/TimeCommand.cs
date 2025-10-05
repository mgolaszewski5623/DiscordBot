using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.Messages.Commands.SimpleCommands
{
    internal class TimeCommand : BaseSimpleCommand
    {
        public override string Name => "time";

        public override string Description => "Displays the current server time.";

        protected override string CreateMessageToSend(SocketMessage message)
            => $"Current server time is: {DateTime.Now.ToString("F")}";
    }
}
