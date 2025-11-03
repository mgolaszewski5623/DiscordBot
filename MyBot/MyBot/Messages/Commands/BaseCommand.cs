using Discord;
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

        protected virtual IEnumerable<Func<SocketMessage, Task<(bool isValid, string? rason)>>> GetValidators()
        {
            if (!CanBotSendMessage)
                yield return async m => (!m.Author.IsBot, "Bots cannot use this command.");

            if (!CanBeOutsideTextChannel)
                yield return async m => (m.Channel is SocketTextChannel, "This command can only be used in text channels.");

            if (!CanNoModeUserUseCommand)
                yield return async m => (m.AuthorHasModPermission(), "You do not have permission to use this command.");
        }

        protected async Task<bool> ValidatePermissions(SocketMessage message)
        {
            foreach (Func<SocketMessage, Task<(bool isValid, string? rason)>> validator in GetValidators())
            {
                var (isValid, reason) = await validator(message);
                if (!isValid)
                {
                    await SendMessage(message, reason ?? "You cannot use this command.");
                    return false;
                }
            }
            return true;
        }

        protected Task SendMessage(SocketMessage receivedMessage, string messageToSend)
            => receivedMessage.Channel.SendMessageAsync(messageToSend);

        protected Task SendMessage(SocketMessage receivedMessage, Embed embedToSend)
            => receivedMessage.Channel.SendMessageAsync(embed: embedToSend);

        protected Task SendMessage(SocketMessage receivedMessage, string messageToSend, Embed embedToSend)
            => receivedMessage.Channel.SendMessageAsync(messageToSend, embed: embedToSend);
    }
}
