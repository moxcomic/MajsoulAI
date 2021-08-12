using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MahjongAI.Models
{
    class Graveyard : List<Tile>
    {
        public IEnumerable<Tile> Get(bool includingTakenAway = true)
        {
            if (includingTakenAway)
            {
                return this;
            }
            else
            {
                return this.Where(tile => !tile.IsTakenAway);
            }
        }
    }
}
