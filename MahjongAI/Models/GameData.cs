using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MahjongAI.Models
{
    class GameData
    {
        public const int initialRemainingTile = 69;
        public Dora dora = new Dora();
        public Player[] players = new Player[4];
        public Direction direction;
        public int seq;
        public int seq2;
        public Tile lastTile;
        public int remainingTile;
        public int reachStickCount;

        public Player player
        {
            get
            {
                return players[0];
            }
        }

        public bool isAllLast(GameType gameType)
        {
            return !gameType.HasFlag(GameType.Match_EastSouth) && (seq == 4 || direction == Direction.S)
                || (direction == Direction.S && seq == 4 || direction == Direction.W);
        }

        public Player getPlayerByRanking(int ranking)
        {
            return players.OrderByDescending(p => p, new PlayerComp(seq)).ElementAt(ranking - 1);
        }

        public int getRankingByPlayer(Player player)
        {
            return Array.IndexOf(players.OrderByDescending(p => p, new PlayerComp(seq)).ToArray(), player) + 1;
        }

        private class PlayerComp : IComparer<Player>
        {
            private int seq;

            public PlayerComp(int seq)
            {
                this.seq = seq;
            }

            public int Compare(Player x, Player y)
            {
                int res = 0;
                if ((res = x.point.CompareTo(y.point)) != 0)
                {
                    return res;
                }
                else
                {
                    return -(((int)x.direction + seq - 2) % 4).CompareTo(((int)y.direction + seq - 2) % 4);
                }
            }
        }

        public GameData()
        {
            for (int i = 0; i < 4; i++)
            {
                players[i] = new Player();
                players[i].id = i;
            }
        }
    }
}
