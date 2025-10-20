using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyBot.Models
{
    public class ModerationActionModel
    {
		[JsonIgnore]
		public SocketGuild Guild { get; set; }

		public string GuildName { get; set; } = "";

		public ulong GuildId { get; set; }

		public ulong TargetUserId { get; set; }

		public ulong ModeratorId { get; set; }

		public string Reason { get; set; } = "";

		public DateTime Date { get; set; }
	}
}
