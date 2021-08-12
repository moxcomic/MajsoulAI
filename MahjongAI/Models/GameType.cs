using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MahjongAI.Models
{
    [Flags]
    enum GameType
    {
        Match_East = 1,
        Match_EastSouth = 9,
        Mode_Fast = 64,
        Level_High = 128,
        Level_VeryHigh = 32,
        Level_Phoenix = 160,
        Level_Silver = 128,
        Level_Gold = 32,
        Level_Jade = 160,
        Level_Throne = 256,
    }
}
