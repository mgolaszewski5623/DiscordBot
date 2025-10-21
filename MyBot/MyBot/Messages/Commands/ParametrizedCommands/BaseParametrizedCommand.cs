using Discord.WebSocket;
using MyBot.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.Messages.Commands.ParametrizedCommands
{
    internal abstract class BaseParametrizedCommand : BaseCommand, IParametrizedCommand
    {
        public virtual async Task Execute(SocketMessage message, string[] parameters)
        {
            if (! await ValidatePermissions(message))
                return;
            string messageToSend = CreateMessageToSend(message, parameters);
            await SendMessage(message, messageToSend);
        }

        protected abstract string CreateMessageToSend(SocketMessage message, string[] parameters);
    }
}
