﻿using Discord.WebSocket;
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
    internal class KickCommand : BaseParametrizedCommand
    {
        public override string Name => "kick";

        public override string Description => "Kicks a user from the server. Usage: !kick <user> <reason>";

        protected override bool CanBotSendMessage => false;

        protected override bool CanBeOutsideTextChannel => false;

        protected override bool CanNoModeUserUseCommand => false;

        protected override string CreateMessageToSend(SocketMessage message, string[] parameters)
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
            return ServeKick(kick);
        }

        private string ServeKick(KickModel kick)
        {
            try
            {
                KickManager.KickUser(kick).GetAwaiter().GetResult();
                return $"👢 {kick.TargetUserId} has been kicked. Reason: **{kick.Reason}**";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error kicking user: {ex.GetCompleteMessage()}");
                return $"Failed to kick user:";
            }
        }
    }
}
