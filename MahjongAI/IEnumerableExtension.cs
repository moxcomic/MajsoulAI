using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MahjongAI.Models;

namespace MahjongAI
{
    static class IEnumerableExtension
    {
        public static string ToString<T>(this IEnumerable<T> list, string separator, Func<T, string> convert = null)
        {
            if (convert == null)
            {
                convert = (item) => item.ToString();
            }
            IEnumerable<string> strList = list.Select((item) => convert(item));
            return string.Join(separator, strList);
        }

        public static double Product<T>(this IEnumerable<T> list, Func<T, double> selector)
        {
            double res = 1;
            foreach (var item in list)
            {
                res *= selector(item);
            }
            return res;
        }

        public static int TilesCount(this IEnumerable<Tile> tileList, Tile baseTile, int deltaNum = 0)
        {
            return tileList.Count(t => t.Type == baseTile.Type && (t.Type != "z" || deltaNum == 0) && t.GeneralId == baseTile.GeneralId + deltaNum);
        }

        public static bool TileExists(this IEnumerable<Tile> tileList, Tile baseTile, int deltaNum = 0)
        {
            return TilesCount(tileList, baseTile, deltaNum) > 0;
        }

        public static void Deconstruct<T1, T2>(this Tuple<T1, T2> tuple, out T1 item1, out T2 item2)
        {
            item1 = tuple.Item1;
            item2 = tuple.Item2;
        }
    }
}
