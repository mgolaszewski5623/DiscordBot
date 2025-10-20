using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.Models
{
    public class KickModel : ModerationActionModel
    {
        public override string ToString()
            => $"You have been kicked from the {GuildName} server. Reason: {Reason}";
    }
}
