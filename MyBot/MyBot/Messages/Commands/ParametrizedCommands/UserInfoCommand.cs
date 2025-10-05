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
    internal class UserInfoCommand : BaseParametrizedCommand
    {
        public override string Name => "userinfo";

        public override string Description => "Displays information about a specified user.";

        protected override string CreateMessageToSend(SocketMessage message, string[] args)
        {
            if (args.Length != 1)
                return "Invalid number of arguments.";
            try
            {
                var user = message.MentionedUsers.First();
                var messageToSend = $"""
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
                Console.WriteLine($"Error retrieving user info: {ex.GetCompleteMessage()}");
                return "Specified user not found or error retrieving user info.";
            }
        }
    }
}
