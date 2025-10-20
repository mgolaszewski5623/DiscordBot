using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.Models
{
    public class WarningModel : ModerationActionModel
    {
        public override string ToString()
            => $"[{Date}] UserID: {TargetUserId}, ModeratorID: {ModeratorId}, Reason: {Reason}";

        public KickModel ToKick()
            => new KickModel
            {
                Guild = this.Guild,
                GuildId = this.GuildId,
                GuildName = this.GuildName,
                TargetUserId = this.TargetUserId,
                ModeratorId = this.ModeratorId,
                Reason = "To many warnings",
                Date = DateTime.UtcNow
            };
    }
}
