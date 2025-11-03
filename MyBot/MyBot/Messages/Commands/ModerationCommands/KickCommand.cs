using Discord.WebSocket;
using MyBot.DataManager;
using MyBot.Exceptions;
using MyBot.Messages.Commands.Base;
using MyBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.Messages.Commands.ModerationCommands
{
    internal class KickCommand : BaseCommand
    {
        public override string Name => "kick";

        public override string Description => "Kicks a user from the server. Usage: !kick <user> <reason>";

        protected override bool CanBotSendMessage => false;

        protected override bool CanBeOutsideTextChannel => false;

        protected override bool CanNoModeUserUseCommand => false;

        protected override bool RequireParameters => true;

        protected override async Task<object> CreateMessageToSend(SocketMessage message, string[]? parameters)
        {
            if (!(message.MentionedUsers.FirstOrDefault() is SocketGuildUser targetUser))
                return $"Please mention a valid user to kick.";
            string reason = parameters.Length > 1 ? string.Join(" ", parameters.Skip(1)) : "No reason provided";
            KickModel kick = new KickModel
            {
                Guild = targetUser.Guild,
                GuildId = targetUser.Guild.Id,
                GuildName = targetUser.Guild.Name,
                TargetUserId = targetUser.Id,
                ModeratorId = message.Author.Id,
                Reason = reason,
                Date = DateTime.UtcNow
            };
            return await ServeKick(kick);
        }

        private async Task<string> ServeKick(KickModel kick)
        {
            try
            {
                await KickManager.KickUser(kick);
                return $"👢 {kick.TargetUserId} has been kicked. Reason: **{kick.Reason}**";
            }
            catch(MyBotInformationException informationEx)
            {
                return $"Failed to kick user: {informationEx.Message}";
            }
            catch (Exception)
            {
                return $"Failed to kick user";
            }
        }
    }
}
