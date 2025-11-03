using Discord;
using Discord.WebSocket;
using MyBot.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.Messages.Commands.Base
{
    internal abstract class BaseCommand : IMyBotCommand
    {
        public abstract string Name { get; }

        public abstract string Description { get; }

        protected virtual bool CanBotSendMessage => true;

        protected virtual bool CanBeOutsideTextChannel => true;

        protected virtual bool CanNoModeUserUseCommand => true;

        protected virtual bool AllowParameters => true;

        protected virtual bool RequireParameters => false;

        protected virtual IEnumerable<Func<SocketMessage, string[]?, Task<(bool isValid, string? rason)>>> GetValidators()
        {
            if (!CanBotSendMessage)
                yield return async (m, _) => (!m.Author.IsBot, "Bots cannot use this command.");

            if (!CanBeOutsideTextChannel)
                yield return async (m, _) => (m.Channel is SocketTextChannel, "This command can only be used in text channels.");

            if (!CanNoModeUserUseCommand)
                yield return async (m, _) => (m.AuthorHasModPermission(), "You do not have permission to use this command.");

            if (!AllowParameters)
                yield return async (_, p) => (p == null || p.Length == 0, "This command cannot take parameters.");

            if (RequireParameters)
                yield return async (_, p) => (p != null && p.Length > 0, "This command requires parameters.");
        }

        protected async Task<bool> ValidatePermissions(SocketMessage message, string[]? parameters)
        {
            foreach (Func<SocketMessage, string[]?, Task<(bool isValid, string? rason)>> validator in GetValidators())
            {
                var (isValid, reason) = await validator(message, parameters);
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

        public async Task Execute(SocketMessage message, string[]? parameters = null)
        {
            if (!await ValidatePermissions(message, parameters))
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

        protected abstract Task<object> CreateMessageToSend(SocketMessage message, string[]? parameters = null);
    }
}
