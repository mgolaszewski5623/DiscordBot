using Discord.WebSocket;
using MyBot.DataManager;
using MyBot.Extensions;
using MyBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.Messages.Commands.ParametrizedCommands
{
    internal class WarnCommand : BaseParametrizedCommand
    {
        public override string Name => "warn";

        public override string Description => "Issues a warning to a user. Usage: !warn <user> <reason>";

        protected override bool CanBotSendMessage => false;

        protected override bool CanBeOutsideTextChannel => false;

        protected override string CreateMessageToSend(SocketMessage message, string[] parameters)
        {
            if(!message.AuthorHasModPermission())
                return "You do not have permission to issue warnings.";
            if (!(message.MentionedUsers.FirstOrDefault() is SocketGuildUser targetUser))
                return $"Please mention a valid user to warn.";
            string reason = parameters.Length > 1 ? string.Join(" ", parameters.Skip(1)) : "No reason provided";
            var warning = new Warning
            {
                GuildId = targetUser.Guild.Id,
                GuildName = targetUser.Guild.Name,
                WarnedUserId = targetUser.Id,
                ModeratorId = message.Author.Id,
                Reason = reason,
                Date = DateTime.UtcNow
            };
            WarningManager.SaveWarning(warning).GetAwaiter().GetResult();
            ServeWarning(warning);
            return $"⚠️ {targetUser.Mention} dostał ostrzeżenie: **{reason}**";
        }

        private void ServeWarning(Warning warning)
        {
            if (WarningManager.HasReachedMaxWarnings(warning.GuildId, warning.GuildName, warning.WarnedUserId).GetAwaiter().GetResult())
            {
                // Todo: Implement additional actions like muting or banning the user
                Console.WriteLine("MAX");
            }
        }
    }
}
