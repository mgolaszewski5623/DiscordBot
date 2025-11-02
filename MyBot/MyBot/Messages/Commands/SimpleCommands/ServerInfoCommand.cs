using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.Messages.Commands.SimpleCommands
{
    internal class ServerInfoCommand : BaseSimpleCommand
    {
        public override string Name => "serverinfo";

        public override string Description => "Provides information about the server.";

        protected override bool CanBotSendMessage => false;

        protected override bool CanBeOutsideTextChannel => false;

        protected override Task<object> CreateMessageToSend(SocketMessage message)
        {
            if (message.Channel is not SocketGuildChannel guildChannel)
                return Task.FromResult<object>("This command can only be used in a server text channel.");
            SocketGuild guild = guildChannel.Guild;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Server Name: {guild.Name}");
            sb.AppendLine($"Total Members: {guild.MemberCount}");
            sb.AppendLine($"Created On: {guild.CreatedAt.UtcDateTime.ToString("f")} UTC");
            sb.AppendLine($"Owner: {guild.Owner?.Username}#{guild.Owner?.Discriminator}");
            sb.AppendLine($"Region: {guild.VoiceRegionId}");
            sb.AppendLine($"Roles: {guild.Roles.Count}");
            sb.AppendLine($"Channels: {guild.Channels.Count}");
            sb.AppendLine($"Emojis: {guild.Emotes.Count}");
            return Task.FromResult<object>(sb.ToString());
        }
    }
}
