using Discord.WebSocket;
using MyBot.DataManager;
using MyBot.Exceptions;
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

        protected override async Task<object> CreateMessageToSend(SocketMessage message, string[] parameters)
        {
            try
            {
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
                string? result = await ServeWarning(warning);
                if (result != null)
                    return result;

                return $"⚠️ {targetUser.Mention} dostał ostrzeżenie: **{reason}**";
            }
            catch(Exception ex)
            {
                return $"Failed to issue a warning: {ex.Message}";
            }
        }

        private async Task<string?> ServeWarning(WarningModel warning)
        {
            try
            {
                if (WarningManager.HasReachedMaxWarnings(warning.GuildId, warning.GuildName, warning.TargetUserId).GetAwaiter().GetResult())
                {
                    KickModel kick = warning.ToKick();
                    await KickManager.KickUser(kick);
                }
                return null;
            }
            catch(MyBotInformationException ex)
            {
                return $"Failed in serving a warning: {ex.Message}";
            }
            catch(Exception ex)
            {
                await LogManager.LogException(ex, Enums.ExceptionType.ERROR);
                return "Failed in serving a warning";
            }
        }
    }
}
