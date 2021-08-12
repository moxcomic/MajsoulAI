using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MahjongAI.Models
{
    class Hand: SortedSet<Tile>
    {
        public void AddRange(IEnumerable<Tile> tiles)
        {
            foreach (var tile in tiles)
            {
                this.Add(tile);
            }
        }

        public int RemoveRange(IEnumerable<Tile> tiles)
        {
            int count = 0;
            foreach (var tile in tiles)
            {
                if (this.Remove(tile))
                {
                    count++;
                }
            }
            return count;
        }
    }
}
