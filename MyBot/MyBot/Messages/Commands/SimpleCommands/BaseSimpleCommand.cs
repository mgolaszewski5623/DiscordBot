using Discord.WebSocket;
using MyBot.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.Messages.Commands.SimpleCommands
{
    internal abstract class BaseSimpleCommand : BaseCommand, ISimpleCommand
    {
        public virtual async Task Execute(SocketMessage message)
        {
            if(!await ValidatePermissions(message))
                return;
            string messageToSend = CreateMessageToSend(message);
            await SendMessage(message, messageToSend);
        }

        protected abstract string CreateMessageToSend(SocketMessage message);
    }
}
