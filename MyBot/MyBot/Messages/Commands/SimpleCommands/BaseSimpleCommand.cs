using Discord.WebSocket;
using MyBot.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.Messages.Commands.SimpleCommands
{
    internal class BaseSimpleCommand : ISimpleCommand
    {
        public virtual string Name => "BaseSimpleCommand";

        public virtual string Description => "This is a base simple command. It should be overridden.";

        protected virtual bool CanBotSendMessage => true;

        protected virtual bool CanBeOutsideTextChannel => true;

        public virtual async Task Execute(SocketMessage message)
        {
            if (!CanBotSendMessage && message.Author.IsBot)
            {
                await SendMessage(message, "Bots cannot use this command.");
                return;
            }
            if(!CanBeOutsideTextChannel && message.Channel is not SocketTextChannel textChannel)
            {
                await SendMessage(message, "This command can only be used in text channels.");
                return;
            }
            string messageToSend = CreateMessageToSend(message);
            await SendMessage(message, messageToSend);
        }

        private Task SendMessage(SocketMessage receivedMessage, string messageToSend)
            => receivedMessage.Channel.SendMessageAsync(messageToSend);

        protected virtual string CreateMessageToSend(SocketMessage message)
            => $"This is a baseSimpleCommand.";
    }
}
