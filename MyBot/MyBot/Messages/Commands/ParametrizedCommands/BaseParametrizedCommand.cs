using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.Messages.Commands.ParametrizedCommands
{
    internal class BaseParametrizedCommand : IParametrizedCommand
    {
        public virtual string Name => "BaseParametrizedCommand";

        public virtual string Description => "This is a base parametrized command. It should be overridden.";

        protected virtual bool CanBotSendMessage => true;

        protected virtual bool CanBeOutsideTextChannel => true;

        public virtual async Task Execute(SocketMessage message, string[] parameters)
        {
            if (!CanBotSendMessage && message.Author.IsBot)
            {
                await SendMessage(message, "Bots cannot use this command.");
                return;
            }
            if (!CanBeOutsideTextChannel && message.Channel is not SocketTextChannel textChannel)
            {
                await SendMessage(message, "This command can only be used in text channels.");
                return;
            }
            string messageToSend = CreateMessageToSend(message, parameters);
            await SendMessage(message, messageToSend);
        }

        private Task SendMessage(SocketMessage receivedMessage, string messageToSend)
            => receivedMessage.Channel.SendMessageAsync(messageToSend);

        protected virtual string CreateMessageToSend(SocketMessage message, string[] parameters)
        {
            string paramsList = string.Join(", ", parameters);
            return $"This is a baseParametrizedCommand with parameters: {paramsList}.";
        }
    }
}
