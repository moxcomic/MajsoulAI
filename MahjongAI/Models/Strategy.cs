using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MahjongAI.Models
{
    class Strategy
    {
        public enum DefenceLevelType
        {
            Default = 3,
            DefendReachedPlayers = 2,
            DefendReachedOya = 1,
            NoDefence = 0,
        }

        public DefenceLevelType DefenceLevel { get; set; } = DefenceLevelType.Default;
    }
}
