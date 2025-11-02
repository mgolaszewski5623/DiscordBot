using Discord.WebSocket;
using MyBot.Enums;
using MyBot.Exceptions;
using MyBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.DataManager
{
    public static class KickManager
    {
        public static async Task KickUser(KickModel kick)
        {
            try
            {
				SocketGuildUser? user = kick.Guild.GetUser(kick.TargetUserId);
				SocketGuildUser? moderator = kick.Guild.GetUser(kick.ModeratorId);
                if (!CheckHierarchy(user, moderator))
					throw new MyBotException("Cannot kick the user due to role hierarchy.");
                InformUser(user, kick);
                await user.KickAsync(string.IsNullOrEmpty(kick.Reason) ? "No reason" : kick.Reason);
			}
			catch (Exception ex)
            {
                await LogManager.LogException(ex, ExceptionType.ERROR);
                throw new MyBotException("An error occurred while trying to kick the user.", ex);
			}
        }

        private static async void InformUser(SocketGuildUser user, KickModel kick)
        {
            try
            {
                var dm = await user.CreateDMChannelAsync();
                await dm.SendMessageAsync(kick.ToString());
            }
            catch (Exception ex)
            {
                await LogManager.LogException(ex, ExceptionType.WARNING);
                // Do nothing if we can't inform the user, user can still be kicked
            }
		}

        private static bool CheckHierarchy(SocketGuildUser? user, SocketGuildUser? moderator)
        {
            if (user == null || moderator == null)
                return false;
            return moderator.Hierarchy > user.Hierarchy;
		}
    }
}
