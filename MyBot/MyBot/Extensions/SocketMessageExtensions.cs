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

        public static SocketGuildUser? TryGetGuildUser(this SocketMessage message, string userArg)
        {
            if (message == null || string.IsNullOrWhiteSpace(userArg))
                return null;

            // 🔹 1️⃣ Mention (działa, jeśli użytkownik kliknął @user)
            if (message.MentionedUsers.FirstOrDefault() is SocketGuildUser mentioned)
                return mentioned;

            // 🔹 2️⃣ ID (np. !warn 123456789012345678)
            if (ulong.TryParse(userArg.Replace("<@", "").Replace(">", "").Replace("!", ""), out ulong userId))
            {
                var guild = (message.Author as SocketGuildUser)?.Guild;
                var byId = guild?.GetUser(userId);
                if (byId != null)
                    return byId;
            }

            // 🔹 3️⃣ Nazwa (np. !warn Robert)
            if (message.Author is SocketGuildUser author)
            {
                var guild = author.Guild;
                var byName = guild.Users.FirstOrDefault(u =>
                    u.Username.Equals(userArg, StringComparison.OrdinalIgnoreCase) ||
                    (u.Nickname != null && u.Nickname.Equals(userArg, StringComparison.OrdinalIgnoreCase))
                );

                if (byName != null)
                    return byName;
            }

            return null;
        }
    }
}
