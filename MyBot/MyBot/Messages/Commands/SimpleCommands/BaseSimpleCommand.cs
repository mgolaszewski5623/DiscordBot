using Discord;
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
            object messageToSend = await CreateMessageToSend(message);
            switch (messageToSend)
            {
                case string text:
                    await SendMessage(message, text);
                    break;
                case Embed embed:
                    await SendMessage(message, embed);
                    break;
                case (string text, Embed embed):
                    await SendMessage(message, text, embed);
                    break;
                default:
                    await SendMessage(message, "⚠️ Unsupported message type returned by command.");
                    break;
            }
        }

        protected abstract Task<object> CreateMessageToSend(SocketMessage message);
    }
}
