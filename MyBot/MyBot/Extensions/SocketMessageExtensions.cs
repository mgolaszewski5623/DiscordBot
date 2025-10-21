using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.Extensions
{
    public static class SocketMessageExtensions
    {
        public static bool AuthorHasModPermission(this SocketMessage message)
        {
            if(message.Author is not SocketGuildUser user)
                return false;
            GuildPermissions permissions = user.GuildPermissions;
            return permissions.Administrator || permissions.ManageMessages || permissions.KickMembers || permissions.BanMembers;
        }
    }
}
