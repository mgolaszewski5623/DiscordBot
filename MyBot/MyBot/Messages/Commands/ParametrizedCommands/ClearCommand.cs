using Discord;
using Discord.WebSocket;
using MyBot.Exceptions;
using MyBot.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.Messages.Commands.ParametrizedCommands
{
    internal class ClearCommand : IParametrizedCommand
    {
        public string Name => "clear";

        public string Description => "Clears a specified number of messages from the channel.";

        public async Task Execute(SocketMessage message, string[] args)
        {
            try
            {
                var channel = message.Channel as SocketTextChannel;
                int count = ValidateClearCommand(message, args);
                var messages = await channel.GetMessagesAsync(count + 1).FlattenAsync();
                await channel.DeleteMessagesAsync(messages);
                var confirmationMessage = await channel.SendMessageAsync($"Deleted {count} messages.");
                await Task.Delay(1000);
                await confirmationMessage.DeleteAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing clear command: {ex.GetCompleteMessage()}");
                await message.Channel.SendMessageAsync($"{ex.Message}");
            }
        }

        private int ValidateClearCommand(SocketMessage message, string[] args)
        {
            if (!(message.Channel is SocketTextChannel))
                throw new MyBotException("This command can only be used in text channels.");
            if (!message.AuthorHasModPermission())
                throw new MyBotException("User doesn't have permission to use this command.");
            if (args.Length != 1 || !int.TryParse(args[0], out int count) || count <= 0)
                throw new MyBotException("Invalid command usage. Correct usage: !clear <number_of_messages>");
            if (count <= 0 || count > 100)
                throw new MyBotException("Number of messages to delete must be between 1 and 100.");
            return count;
        }
    }
}
