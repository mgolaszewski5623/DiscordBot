using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.Messages.Commands.Base
{
    internal interface IMyBotCommand
    {
        public string Name { get; }

		public string Description { get; }

        public Task Execute(SocketMessage message, string[]? parameters = null);

    }
}
