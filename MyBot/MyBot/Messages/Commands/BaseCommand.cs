using Discord.WebSocket;
using MyBot.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.Messages.Commands
{
    internal abstract class BaseCommand : ICommand
    {
        public abstract string Name { get; }

        public abstract string Description { get; }

        protected virtual bool CanBotSendMessage => true;

        protected virtual bool CanBeOutsideTextChannel => true;

        protected virtual bool CanNoModeUserUseCommand => true;

        protected async Task<bool> ValidatePermissions(SocketMessage message)
        {
            if (!CanBotSendMessage && message.Author.IsBot)
            {
                await SendMessage(message, "Bots cannot use this command.");
                return false;
            }
            if (!CanBeOutsideTextChannel && message.Channel is not SocketTextChannel textChannel)
            {
                await SendMessage(message, "This command can only be used in text channels.");
                return false;
            }
            if (!CanNoModeUserUseCommand && !message.AuthorHasModPermission())
            {
                await SendMessage(message, "You do not have permission to use this command.");
                return false;
            }
            return true;
        }

        protected Task SendMessage(SocketMessage receivedMessage, string messageToSend)
            => receivedMessage.Channel.SendMessageAsync(messageToSend);
    }
}
