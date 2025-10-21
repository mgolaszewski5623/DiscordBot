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

        protected override bool CanNoModeUserUseCommand => false;

        protected override string CreateMessageToSend(SocketMessage message, string[] parameters)
        {
            //var targetUser = message.TryGetGuildUser(parameters[0]);
            //if (targetUser == null)
            //    return $"Please specify a valid user to warn.";
            //else
            //    return $"targetUser.Mention = {targetUser.Mention}";
            if (!(message.MentionedUsers.FirstOrDefault() is SocketGuildUser targetUser))
                return $"Please mention a valid user to warn.";
            string reason = parameters.Length > 1 ? string.Join(" ", parameters.Skip(1)) : "No reason provided";
            WarningModel warning = new WarningModel
            {
                Guild = targetUser.Guild,
				GuildId = targetUser.Guild.Id,
                GuildName = targetUser.Guild.Name,
                TargetUserId = targetUser.Id,
                ModeratorId = message.Author.Id,
                Reason = reason,
                Date = DateTime.UtcNow
            };
            WarningManager.SaveWarning(warning).GetAwaiter().GetResult();
            //ServeWarning(warning);
            return $"⚠️ {targetUser.Mention} dostał ostrzeżenie: **{reason}**";
        }

        private void ServeWarning(WarningModel warning)
        {
            if (WarningManager.HasReachedMaxWarnings(warning.GuildId, warning.GuildName, warning.TargetUserId).GetAwaiter().GetResult())
            {
                KickModel kick = warning.ToKick();
                KickManager.KickUser(kick).GetAwaiter().GetResult();
			}
        }
    }
}
