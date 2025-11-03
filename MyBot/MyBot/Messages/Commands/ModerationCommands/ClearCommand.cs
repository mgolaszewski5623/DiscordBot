using Discord;
using Discord.Rest;
using Discord.WebSocket;
using MyBot.DataManager;
using MyBot.Exceptions;
using MyBot.Messages.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.Messages.Commands.ModerationCommands
{
    internal class ClearCommand : BaseCommand
    {
        private const int SLEEP_TIME = 2000;
        private const int MAX_MESSAGES = 100;

        public override string Name => "clear";

        public override string Description => "Clears a specified number of messages from the channel.";

        protected override bool CanBotSendMessage => false;

        protected override bool CanBeOutsideTextChannel => false;

        protected override bool CanNoModeUserUseCommand => false;

        protected override bool RequireParameters => true;

        protected override async Task<object> CreateMessageToSend(SocketMessage message, string[]? parameters)
        {
            try
            {
                SocketTextChannel? channel = message.Channel as SocketTextChannel;
                int count = ValidateCount(parameters);
                IEnumerable<IMessage> messages = await channel.GetMessagesAsync(count + 1).FlattenAsync();
                await channel.DeleteMessagesAsync(messages);

                RestUserMessage confirmationMessage = await channel.SendMessageAsync($"Deleted {count} messages.");
                await Task.Delay(SLEEP_TIME);
                await confirmationMessage.DeleteAsync();
                return string.Empty;
            }
            catch(MyBotInformationException informationEx)
            {
                return informationEx.Message;
            }
            catch (Exception ex)
            {
                await LogManager.LogException(ex, Enums.ExceptionType.INFORMATION);
                return $"Occured error during clearing a text channel.";
            }
        }

        private static int ValidateCount(string[] args)
        {
            if (args.Length != 1 || !int.TryParse(args[0], out int count))
                throw new MyBotInformationException("Usage: !clear <number_of_messages>");
            if (count < 1 || count > MAX_MESSAGES)
                throw new MyBotInformationException($"Number of messages to delete must be between 1 and {MAX_MESSAGES}.");
            return count;
        }
    }
}
