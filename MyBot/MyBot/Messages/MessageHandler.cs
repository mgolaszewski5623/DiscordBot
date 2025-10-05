using Discord.WebSocket;
using MyBot.Messages.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.Messages
{
    internal class MessageHandler
    {
        private readonly string prefix;

        private readonly CommandHandler _commandHandler;

        public MessageHandler(string prefix)
        {
            this.prefix = prefix;
            _commandHandler = new CommandHandler(prefix);
        }

        public async Task HandleMessage(SocketMessage message)
        {
            if (IsCommand(message.Content))
                await _commandHandler.HandleCommand(message);
            else
                await HandleNonCommand(message);
        }

        private bool IsCommand(string messageContent)
            => messageContent.StartsWith(prefix, StringComparison.OrdinalIgnoreCase);

        private async Task HandleNonCommand(SocketMessage message)
        {
            // Todo: Implement non-command message handling logic here
        }
    }
}
