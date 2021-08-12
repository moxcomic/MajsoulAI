using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MahjongAI.Models;

namespace MahjongAI
{
    static class Constants
    {
        public static readonly IReadOnlyDictionary<MajsoulRegion, string> MAJSOUL_API_URL_PRIFIX = new Dictionary<MajsoulRegion, string>()
        {
            { MajsoulRegion.CN_DOMESTIC, "https://lb-mainland.majsoul.com:2901/api/v0" },
            { MajsoulRegion.CN_INTERNATIONAL, "https://lb-hk.majsoul.com:7891/api/v0" },
        };

        public const string TENHOU_SERVER_HOST = "133.242.10.78";

        public const int TENHOU_SERVER_PORT = 10080;
    }
}
