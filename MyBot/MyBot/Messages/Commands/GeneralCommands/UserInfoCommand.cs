using Discord.WebSocket;
using MyBot.DataManager;
using MyBot.Enums;
using MyBot.Messages.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.Messages.Commands.GeneralCommands
{
    internal class UserInfoCommand : BaseCommand
    {
        public override string Name => "userinfo";

        public override string Description => "Displays information about a specified user.";

        protected override bool RequireParameters => true;

        protected override async Task<object> CreateMessageToSend(SocketMessage message, string[]? parameters)
        {
            if (parameters.Length != 1)
                return "Invalid number of arguments.";
            try
            {
                SocketUser user = message.MentionedUsers.First();
                string messageToSend = $"""
                    User Info:
                    Username: {user.Username}
                    Discriminator: {user.Discriminator}
                    ID: {user.Id}
                    Is Bot: {user.IsBot}
                    Created At: {user.CreatedAt.UtcDateTime} (UTC)
                    Status: {user.Status}
                    """;
                return messageToSend;
            }
            catch (Exception ex)
            {
                await LogManager.LogException(ex, ExceptionType.ERROR);
                return "Specified user not found or error retrieving user info.";
            }
        }
    }
}
