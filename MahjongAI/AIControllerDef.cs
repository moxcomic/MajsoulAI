using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using MahjongAI.Models;

namespace MahjongAI
{
    partial class AIController
    {
        private DefEvalResultComp defEvalResultComp = new DefEvalResultComp();

        private bool shouldDef(EvalResult evalResult, Tile discardTile = null)
        {
            if (strategy.DefenceLevel == Strategy.DefenceLevelType.NoDefence) return false;
            if (strategy.DefenceLevel == Strategy.DefenceLevelType.DefendReachedOya && gameData.players.All(p => !p.reached || p.direction != Direction.E)) return false;
            if (strategy.DefenceLevel == Strategy.DefenceLevelType.DefendReachedPlayers && gameData.players.All(p => !p.reached)) return false;

            return
                (gameData.players.Any(p => defLevel(p) >= 4) && shouldDef4(evalResult, discardTile)
                    || gameData.players.Any(p => defLevel(p) >= 2) && shouldDef2(evalResult, discardTile)
                    || gameData.players.Any(p => defLevel(p) >= 1) && shouldDef1(evalResult, discardTile))
                && !isAllLastBottom() // 不是All last四位
                && !(player.point <= gameData.getPlayerByRanking(3).point - 10000 && (gameData.direction == Direction.S || player.direction == Direction.E)); // 南场或亲家的时候比三位低了10000点以上就不防守
        }

        private bool shouldDef4(EvalResult evalResult, Tile discardTile = null)
        {
            return evalResult.Distance >= 3
                || evalResult.Distance == 2 && (
                    evalResult.E_Point < 8000
                    || evalResult.E_PromotionCount[0] <= 15 && evalResult.E_Point < 12000
                    || evalResult.E_PromotionCount[0] <= 8)
                || evalResult.Distance == 1 && (
                    (evalResult.E_Point < 2000 && isTileTooDangerous(discardTile))
                    || (evalResult.E_PromotionCount[0] <= 20 && evalResult.E_Point < 4000)
                    || (evalResult.E_PromotionCount[0] <= 9 && evalResult.E_Point < 8000)
                    || evalResult.E_PromotionCount[0] <= 4)
                || evalResult.Distance == 0 && (evalResult.E_Point < 2000 || evalResult.E_PromotionCount[0] < 1) && isTileTooDangerous(discardTile)
                || gameData.remainingTile / 4 <= evalResult.Distance * 2 && isTileTooDangerous(discardTile);
        }

        private bool shouldDef2(EvalResult evalResult, Tile discardTile = null)
        {
            return evalResult.Distance >= 3
                || evalResult.Distance == 2 && (
                    evalResult.E_Point < 4000
                    || evalResult.E_PromotionCount[0] <= 15 && evalResult.E_Point < 12000
                    || evalResult.E_PromotionCount[0] <= 8)
                || evalResult.Distance == 1 && (
                    (evalResult.E_Point < 1000 && isTileTooDangerous(discardTile))
                    || (evalResult.E_PromotionCount[0] <= 20 && evalResult.E_Point < 2000)
                    || (evalResult.E_PromotionCount[0] <= 9 && evalResult.E_Point < 8000)
                    || evalResult.E_PromotionCount[0] <= 4)
                || evalResult.Distance == 0 && (evalResult.E_Point < 1000 || evalResult.E_PromotionCount[0] < 1) && isTileTooDangerous(discardTile)
                || gameData.remainingTile / 4 <= evalResult.Distance * 2 && isTileTooDangerous(discardTile);
        }

        private bool shouldDef1(EvalResult evalResult, Tile discardTile = null)
        {
            // 最后三巡
            return evalResult.Distance >= 2;
        }

        private bool isTileTooDangerous(Tile tile)
        {
            return tile != null && evalDef(tile).Risk > 15;
        }

        private int defLevel(Player forPlayer)
        {
            if (forPlayer == player)
            {
                return 0;
            }
            else if (forPlayer.reached)
            {
                return 4;
            }
            else if (forPlayer.fuuro.Tiles.Sum(t => doraValue(t)) >= 3 && forPlayer.fuuro.VisibleCount * 3 + forPlayer.fuuro.MinkanCount + forPlayer.fuuro.AnkanCount * 4 + forPlayer.graveyard.Count >= 15) //牌河枚树+副露宝牌数+副露数判断防御
            {
                return 4;
            }
            else if (forPlayer.fuuro.VisibleCount > 0 && forPlayer.fuuro.VisibleCount * 3 + forPlayer.fuuro.MinkanCount + forPlayer.fuuro.AnkanCount * 4 + forPlayer.graveyard.Count >= 15)
            {
                return 2;
            }
            else if (forPlayer.graveyard.Count >= 15)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public Tile chooseDiscardForDef(List<Tuple<Tile, EvalResult>> options)
        {
            return chooseDiscardForDef(out DefEvalResult defEvalResult, options);
        }

        private Tile chooseDiscardForDef(out DefEvalResult defEvalResult, List<Tuple<Tile, EvalResult>> options)
        {
            var evalResults = new Dictionary<string, DefEvalResult>();
            Tuple<Tile, DefEvalResult> bestResult = null;
            Trace.WriteLine("Defense: " + gameData.players.Where(p => defLevel(p) > 0).ToString(", ", p => string.Format("player({0}):{1}", p.id, defLevel(p))));

            foreach (var tile in player.hand)
            {
                if (!evalResults.ContainsKey(tile.Name))
                {
                    var atkEvalResult = options.Where(tuple => tuple.Item1.Name == tile.Name).DefaultIfEmpty(Tuple.Create<Tile, EvalResult>(null, null)).First().Item2;
                    var result = evalResults[tile.Name] = evalDef(tile, atkEvalResult);
                    Trace.WriteLine(string.Format("Option: discard {0}, {1}", tile.Name, result));
                    if (bestResult == null
                        || defEvalResultComp.Compare(result, bestResult.Item2) > 0)
                    {
                        bestResult = new Tuple<Tile, DefEvalResult>(tile, result);
                    }
                }
            }

            foreach (var tile in player.hand)
            {
                var result = evalResults[tile.Name];
                if (result.Risk - result.Bonus <= (bestResult.Item2.Risk - bestResult.Item2.Bonus) && evalResultComp.Compare(result.AtkEvalResult, bestResult.Item2.AtkEvalResult) > 0)
                {
                    bestResult = new Tuple<Tile, DefEvalResult>(tile, result);
                }
            }

            Trace.WriteLine(string.Format("BestResult: discard {0}, {1}", bestResult.Item1.Name, bestResult.Item2));
            defEvalResult = bestResult.Item2;
            return bestResult.Item1;
        }

        public DefEvalResult evalDef(Tile tile, EvalResult atkEvalResult = null)
        {
            var res = new DefEvalResult();
            var defTargets = gameData.players.Where(p => defLevel(p) > 0);
            res.Risk = defTargets.Sum(p => evalDef(tile, p).Risk * defLevel(p)) / defTargets.Sum(p => defLevel(p));
            var others = gameData.players.Where(p => p != player && defLevel(p) == 0);
            res.RiskForOthers = others.Count() > 0 ? others.Average(p => evalDef(tile, p).Risk) : 0;
            res.Bonus = atkEvalResult == null ? 0 : 
                isAllLastTop() ? 5 : 
                atkEvalResult.E_Point > 0 ? atkEvalResult.E_Point / 800 : 
                gameData.remainingTile < 16 && atkEvalResult.Distance == 0 ? 1 : // 晚巡型听
                0;
            res.AtkEvalResult = atkEvalResult;
            return res;
        }

        private DefEvalResult evalDef(Tile tile, Player forPlayer)
        {
            var res = new DefEvalResult();

            if (forPlayer.safeTiles.Get().TileExists(tile))
            {
                res.Risk = 0;
            }
            else if (tile.Type == "z")
            {
                res.Risk = evalDefZ(tile, forPlayer);
            }
            else if (tile.Number == 1 || tile.Number == 9)
            {
                res.Risk = evalDef19(tile, forPlayer, tile.Number);
            }
            else if (tile.Number == 2 || tile.Number == 8)
            {
                res.Risk = evalDef28(tile, forPlayer, tile.Number);
            }
            else if (tile.Number == 3 || tile.Number == 7)
            {
                res.Risk = evalDef37(tile, forPlayer, tile.Number);
            }
            else
            {
                res.Risk = evalDefMid(tile, forPlayer);
            }

            var tmp = forPlayer.graveyard.Where(t => t.Type != "z" && t.Number != 1 && t.Number != 9).Take(3).ToList();
            if (tmp.Count >= 1 && isOutsideOf(tile, tmp[0]))
            {
                res.Risk /= 4;
            }
            else if (tmp.Count >= 2 && isOutsideOf(tile, tmp[1]))
            {
                res.Risk /= 2;
            }
            else if (tmp.Count >= 3 && isOutsideOf(tile, tmp[2]))
            {
                res.Risk -= 1;
            }

            if (doraValue(tile) > 0)
            {
                res.Risk += 5;
            }
            else if (res.Risk > 0 && gameData.dora.Exists(t => t.Type == tile.Type && t.Type != "z" && t.getNextGeneralId() + 1 == tile.GeneralId && t.getNextGeneralId() - 1 == tile.GeneralId))
            {
                res.Risk += 3;
            }
            else if (res.Risk > 0 && gameData.dora.Exists(t => t.Type == tile.Type && t.Type != "z" && t.getNextGeneralId() + 2 == tile.GeneralId && t.getNextGeneralId() - 2 == tile.GeneralId))
            {
                res.Risk += 2;
            }
            else if (res.Risk > 0 && gameData.dora.Exists(t => t.Type == tile.Type && t.Type != "z" && t.getNextGeneralId() + 3 == tile.GeneralId && t.getNextGeneralId() - 3 == tile.GeneralId))
            {
                res.Risk += 1;
            }

            res.Risk = Math.Max(res.Risk, 0);

            return res;
        }

        private double evalDefZ(Tile tile, Player forPlayer)
        {
            var visibleCount = getVisibleTiles().TilesCount(tile);
            if (visibleCount == 4)
            {
                return 0;
            }
            else if (visibleCount == 3)
            {
                return 2;
            }
            else if (visibleCount == 2)
            {
                if (getWeight(tile, forPlayer) == 0)
                {
                    return 5;
                }
                else
                {
                    return 6;
                }
            }
            else
            {
                if (getWeight(tile, forPlayer) == 0)
                {
                    return 15;
                }
                else
                {
                    return 16;
                }
            }
        }

        private double evalDef19(Tile tile, Player forPlayer, int side)
        {
            if (side != 1 && side != 9)
            {
                throw new ArgumentException();
            }

            var delta = side == 1 ? 1 : -1;
            var visibleCount = getVisibleTiles().TilesCount(tile);
            var res = 0;

            switch (visibleCount)
            {
                case 4:
                    res = 0;
                    break;
                case 3:
                    res = 1;
                    break;
                case 2:
                    res = 3;
                    break;
                default:
                    res = 8;
                    break;
            }

            if (getVisibleTiles().TilesCount(tile, delta) == 4 || getVisibleTiles().TilesCount(tile, delta * 2) == 4 || forPlayer.safeTiles.Get().TileExists(tile, delta * 3))
            {
                res += 0;
            }
            else if (getVisibleTiles().TilesCount(tile, delta) == 3 && getVisibleTiles().TilesCount(tile, delta * 2) == 3)
            {
                res += 6;
            }
            else if (getVisibleTiles().TilesCount(tile, delta) == 3 || getVisibleTiles().TilesCount(tile, delta * 2) == 3)
            {
                res += 15;
            }
            else
            {
                res += 30;
            }

            return res;
        }

        private double evalDef28(Tile tile, Player forPlayer, int side)
        {
            if (side != 2 && side != 8)
            {
                throw new ArgumentException();
            }

            var delta = side == 2 ? 1 : -1;
            var visibleCount = getVisibleTiles().TilesCount(tile);

            if (getVisibleTiles().TilesCount(tile, -delta) == 4)
            {
                return evalDef19(tile, forPlayer, side == 2 ? 1 : 9);
            }
            else if (getVisibleTiles().TilesCount(tile, delta) == 4)
            {
                switch (visibleCount)
                {
                    case 4:
                        return 0;
                    case 3:
                        return 1;
                    case 2:
                        return 3;
                    default:
                        return 8;
                }
            }

            var res = 0;

            switch (visibleCount)
            {
                case 4:
                    res = 1;
                    break;
                case 3:
                    res = 3;
                    break;
                case 2:
                    res = 8;
                    break;
                default:
                    res = 15;
                    break;
            }

            if (getVisibleTiles().TilesCount(tile, delta * 2) == 4 || forPlayer.safeTiles.Get().TileExists(tile, delta * 3))
            {
                res += 0;
            }
            else if (getVisibleTiles().TilesCount(tile, delta) == 3 && getVisibleTiles().TilesCount(tile, delta * 2) == 3)
            {
                res += 8;
            }
            else if (getVisibleTiles().TilesCount(tile, delta) == 3 || getVisibleTiles().TilesCount(tile, delta * 2) == 3)
            {
                res += 20;
            }
            else
            {
                res += 40;
            }

            return res;
        }

        private double evalDef37(Tile tile, Player forPlayer, int side)
        {
            if (side != 3 && side != 7)
            {
                throw new ArgumentException();
            }

            var delta = side == 3 ? 1 : -1;
            var visibleCount = getVisibleTiles().TilesCount(tile);

            if (getVisibleTiles().TilesCount(tile, -delta) == 4)
            {
                return evalDef19(tile, forPlayer, side == 3 ? 1 : 9);
            }
            else if (getVisibleTiles().TilesCount(tile, -delta * 2) == 4)
            {
                return evalDef28(tile, forPlayer, side == 3 ? 2 : 8);
            }
            else if (getVisibleTiles().TilesCount(tile, delta) == 4)
            {
                switch (visibleCount)
                {
                    case 4:
                        return 1;
                    case 3:
                        return 3;
                    case 2:
                        return 8;
                    default:
                        return 15;
                }
            }

            var res = 0;

            switch (visibleCount)
            {
                case 4:
                    res = 3;
                    break;
                case 3:
                    res = 8;
                    break;
                case 2:
                    res = 15;
                    break;
                default:
                    res = 30;
                    break;
            }

            if (getVisibleTiles().TilesCount(tile, delta * 2) == 4 || forPlayer.safeTiles.Get().TileExists(tile, delta * 3))
            {
                res += 0;
            }
            else if (getVisibleTiles().TilesCount(tile, delta) == 3 && getVisibleTiles().TilesCount(tile, delta * 2) == 3)
            {
                res += 10;
            }
            else if (getVisibleTiles().TilesCount(tile, delta) == 3 || getVisibleTiles().TilesCount(tile, delta * 2) == 3)
            {
                res += 25;
            }
            else
            {
                res += 50;
            }

            return res;
        }

        private double evalDefMid(Tile tile, Player forPlayer)
        {
            var visibleCount = getVisibleTiles().TilesCount(tile);

            if (getVisibleTiles().TilesCount(tile, -1) == 4)
            {
                return evalDef19(tile, forPlayer, 1);
            }
            else if (getVisibleTiles().TilesCount(tile, 1) == 4)
            {
                return evalDef19(tile, forPlayer, 9);
            }
            else if (getVisibleTiles().TilesCount(tile, -2) == 4 || forPlayer.safeTiles.Get().TileExists(tile, -3))
            {
                return evalDef28(tile, forPlayer, 2);
            }
            else if (getVisibleTiles().TilesCount(tile, 2) == 4 || forPlayer.safeTiles.Get().TileExists(tile, 3))
            {
                return evalDef28(tile, forPlayer, 8);
            }
            else if (getVisibleTiles().TilesCount(tile, -3) == 4)
            {
                return evalDef37(tile, forPlayer, 3);
            }
            else if (getVisibleTiles().TilesCount(tile, 3) == 4)
            {
                return evalDef37(tile, forPlayer, 7);
            }
            else
            {
                return 100;
            }
        }

        private bool isOutsideOf(Tile x, Tile y)
        {
            if (x.Type != y.Type || x.Type == "z")
            {
                return false;
            }

            if (y.Number < 5)
            {
                return x.Number < y.Number && x.Number > y.Number - 3;
            }
            else if (y.Number > 5)
            {
                return x.Number > y.Number && x.Number < y.Number + 3;
            }
            else
            {
                return x.Number == 3 || x.Number == 7;
            }
        }

        private class DefEvalResultComp : IComparer<DefEvalResult>
        {
            public int Compare(DefEvalResult x, DefEvalResult y)
            {
                int res = 0;
                if ((y == null) && (x == null))
                {
                    return 0;
                }
                if ((res = (y == null).CompareTo(x == null)) != 0)
                {
                    return res;
                }
                else if ((res = (y.Risk).CompareTo(x.Risk)) != 0)
                {
                    return res;
                }
                else if (x.Risk == 0 && y.Risk == 0 && (res = x.RiskForOthers.CompareTo(y.RiskForOthers)) != 0)
                {
                    return res;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
