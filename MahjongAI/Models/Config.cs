using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MahjongAI.Models
{
    class Config
    {
        public Platform Platform { get; set; }
        public string TenhouID { get; set; }
        public MajsoulRegion MajsoulRegion { get; set; }
        public string MajsoulUsername { get; set; }
        public string MajsoulPassword { get; set; }
        public int PrivateRoom { get; set; }
        public GameType GameType { get; set; }
        public int Repeat { get; set; }
        public Strategy strategy { get; set; }


        public string AuthServer { get; set; }
        public string AccessToken { get; set; }
        public string GameLevel { get; set; }
        public string GameMode { get; set; }
        public string MatchMode { get; set; }
        public int DefenceLevel { get; set; }
        public string DeviceUuid { get; set; }
    }
}
