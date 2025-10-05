using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.Models
{
    public class Warning
    {
        public string GuildName { get; set; } = "";

        public ulong GuildId { get; set; }

        public ulong WarnedUserId { get; set; }

        public ulong ModeratorId { get; set; }

        public string Reason { get; set; } = "";

        public DateTime Date { get; set; }

        public override string ToString()
            => $"[{Date}] UserID: {WarnedUserId}, ModeratorID: {ModeratorId}, Reason: {Reason}";
    }
}
