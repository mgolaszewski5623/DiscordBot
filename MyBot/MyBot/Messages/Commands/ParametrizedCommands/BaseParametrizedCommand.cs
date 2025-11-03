using Discord;
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
            if (!await ValidatePermissions(message))
                return;
            object messageToSend = await CreateMessageToSend(message, parameters);
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

        protected abstract Task<object> CreateMessageToSend(SocketMessage message, string[] parameters);
    }
}
