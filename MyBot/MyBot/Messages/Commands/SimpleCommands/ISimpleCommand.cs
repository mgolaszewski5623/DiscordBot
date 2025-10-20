using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.Messages.Commands.SimpleCommands
{
    internal interface ISimpleCommand : ICommand
	{
        public Task Execute(SocketMessage message);
    }
}
