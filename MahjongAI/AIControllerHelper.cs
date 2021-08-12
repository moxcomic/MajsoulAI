using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MahjongAI.Models;

namespace MahjongAI
{
    partial class AIController : Controller
    {
        private static int[] origWeightMap = new int[38] { -1, 1, 11, 12, 12, 13, 12, 12, 11, 1, -1, 1, 11, 12, 12, 13, 12, 12, 11, 1, -1, 1, 11, 12, 12, 13, 12, 12, 11, 1, -1, 0, 0, 0, 0, 3, 3, 3 };

        private GameData gameData
        {
            get
            {
                return client.gameData;
            }
        }
        private Player player
        {
            get
            {
                return gameData.player;
            }
        }

        private int doraValue(Tile tile)
        {
            int res = gameData.dora.Count(d => d.getNextGeneralId() == tile.GeneralId);
            if (tile.isRedDora)
            {
                res++;
            }
            return res;
        }

        private int getWeight(Tile tile)
        {
            return getWeight(tile, player);
        }

        private int getWeight(Tile tile, Player forPlayer)
        {
            var res = origWeightMap[tile.GeneralId];
            if (tile.Type == "z" && tile.Number == (int)gameData.direction)
            {
                res += 2;
            }
            if (tile.Type == "z" && tile.Number == (int)forPlayer.direction)
            {
                res += 4;
            }

            return res;
        }

        private IEnumerable<Tile> getVisibleTiles()
        {
            foreach (var tile in player.hand)
            {
                yield return tile;
            }
            foreach (var p in gameData.players)
            {
                foreach (var tile in p.graveyard.Get(false))
                {
                    yield return tile;
                }
                foreach (var tile in p.fuuro.Tiles)
                {
                    yield return tile;
                }
            }
            foreach (var tile in gameData.dora)
            {
                yield return tile;
            }
        }

        private IEnumerable<Tile> getEnemyDiscardedTiles()
        {
            foreach (var p in gameData.players.Where(p => p != player))
            {
                foreach (var tile in p.graveyard.Get(true))
                {
                    yield return tile;
                }
            }
        }

        private int calcDistance()
        {
            return calcDistance(out int tmp);
        }

        private int calcDistance(out int normalDistance)
        {
            return MahjongHelper.getInstance().calcDistance(player.hand, player.fuuro.Count, out normalDistance);
        }

        private FuuroGroup tryGetFuuroGroup(FuuroType type, IEnumerable<Tuple<int, int>> req, params Tile[] tiles)
        {
            var res = new FuuroGroup
            {
                type = type
            };

            foreach (var reqItem in req)
            {
                var tmp = new List<Tile>();
                tmp.AddRange(player.hand.Where(t => t.GeneralId == reqItem.Item1 && !tiles.Any(t2 => t == t2) && t.isRedDora));
                tmp.AddRange(player.hand.Where(t => t.GeneralId == reqItem.Item1 && !tiles.Any(t2 => t == t2) && !t.isRedDora));
                if (tmp.Count >= reqItem.Item2)
                {
                    res.AddRange(tmp.Take(reqItem.Item2));
                }
                else
                {
                    return null;
                }
            }

            res.AddRange(tiles);

            return res;
        }

        private bool hasYakuhai()
        {
            foreach (var group in player.fuuro)
            {
                if (group[0].Type == "z" && getWeight(group[0]) > 0)
                {
                    return true;
                }
            }

            foreach (var tile in player.hand)
            {
                if (tile.Type == "z" && getWeight(tile) > 0 && player.hand.Count(t => t.GeneralId == tile.GeneralId) >= 3)
                {
                    return true;
                }
            }

            return false;
        }

        private bool isAllLast()
        {
            return gameData.isAllLast(client.config.GameType);
        }

        private bool isAllLastTop()
        {
            return isAllLast() && gameData.getPlayerByRanking(1) == player;
        }

        private bool isAllLastBottom()
        {
            return isAllLast() && gameData.getPlayerByRanking(4) == player;
        }

        // 自摸后针对的点数变化估计
        private double pointGain(double e_point, Player target)
        {
            if (player.direction == Direction.E)
            {
                e_point *= 1.5 * (4 / 3);
            }
            else if (target.direction == Direction.E)
            {
                e_point *= 1.5;
            }
            else
            {
                e_point *= 1.25;
            }

            return e_point;
        }

        private int calcPoint(Tile lastTile, bool riichi = true, bool tsumoAgari = false, bool isLastTile = false)
        {
            return MahjongHelper.getInstance().calcPoint(player.hand, lastTile, gameData.direction, player.direction, player.fuuro, gameData.dora, riichi && player.fuuro.VisibleCount == 0, tsumoAgari && player.fuuro.VisibleCount == 0, isLastTile);
        }
    }
}
