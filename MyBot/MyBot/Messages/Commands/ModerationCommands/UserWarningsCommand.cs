using Discord.WebSocket;
using MyBot.DataManager;
using MyBot.Enums;
using MyBot.Messages.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.Messages.Commands.ModerationCommands
{
    internal class UserWarningsCommand : BaseCommand
    {
        public override string Name => "userwarnings";

        public override string Description => "Get the user warnings.";

        protected override bool CanBeOutsideTextChannel => false;

        protected override bool CanNoModeUserUseCommand => false;

        protected override bool RequireParameters => true;

        protected override async Task<object> CreateMessageToSend(SocketMessage message, string[]? parameters)
        {
            try
            {
                if (!(message.MentionedUsers.FirstOrDefault() is SocketGuildUser targetUser))
                    return $"Please mention a valid user to warn.";
                List<Models.WarningModel> warnings = await WarningManager.GetWarnings(targetUser.Guild.Id, targetUser.Guild.Name, targetUser.Id);
                if (warnings.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine($"User {targetUser.Username} has {warnings.Count} warning(s):");
                    foreach(var warning in warnings)
                        sb.AppendLine(warning.ToString());
                    return sb.ToString();
                }
                return $"User {targetUser.Nickname} has not warnings.";
            }
            catch (Exception ex)
            {
                await LogManager.LogException(ex, ExceptionType.ERROR);
                return "An error occurred while retrieving user warnings.";
            }
        }
    }
}
