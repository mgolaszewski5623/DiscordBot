using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.Messages.Commands.SimpleCommands
{
    internal interface ISimpleCommand
    {
        public string Name { get; }

        public string Description { get; }

        public Task Execute(SocketMessage message);
    }
}
