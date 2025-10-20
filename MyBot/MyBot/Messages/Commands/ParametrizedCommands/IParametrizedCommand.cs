using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.Messages.Commands.ParametrizedCommands
{
    internal interface IParametrizedCommand : ICommand
    {
        public Task Execute(SocketMessage message, string[] parameters);
    }
}
