using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MahjongAI.Models;

namespace MahjongAI
{
    class Monitor: Controller
    {
        public Monitor(PlatformClient client): base(client)
        {

        }

        protected override void OnLogin(bool resume, bool succeeded)
        {
            if (succeeded)
            {
                Trace.WriteLine("Login succeeded.");
            }
            else
            {
                Trace.WriteLine("Login failed.");
            }
        }

        protected override void OnClose()
        {
            base.OnClose();
            Trace.WriteLine("Client closed.");
        }

        protected override void OnConnectionException()
        {
            Trace.WriteLine("Unexpectedly disconnected.");
        }

        public override void OnDraw(Tile tile)
        {
            Trace.WriteLine(string.Format("draw {0}", tile.Name));
            showCurrentState();
        }

        public override void OnWait(Tile tile, Player fromPlayer)
        {
            Trace.WriteLine(string.Format("wait {0} from player({1})", tile.Name, fromPlayer.id));
            showCurrentState();
        }

        protected override void OnDiscard(Player player, Tile tile)
        {
            Trace.WriteLine(string.Format("player({0}) discard {1}", player.id, tile.Name));
            if (player.id == 0)
            {
                showCurrentState();
            }
        }

        protected override void OnReach(Player player)
        {
            Trace.WriteLine(string.Format("player({0}) reach", player.id));
        }

        protected override void OnNaki(Player player, FuuroGroup fuuro)
        {
            if (fuuro == null)
            {
                Trace.WriteLine(string.Format("player({0}) unknown naki", player.id));
                return;
            }
            Trace.WriteLine(string.Format("player({0}) {1}: {2}", player.id, fuuro.type.ToString(), fuuro.ToString(" ", t => t.Name)));
        }

        protected override void OnGameStart(bool continued)
        {
            if (!continued)
            {
                Trace.WriteLine("Game started.");
            }
            else
            {
                Trace.WriteLine("Game continued.");
            }
        }

        protected override void OnGameEnd()
        {
            Trace.WriteLine("Game ended.");
        }

        protected override void OnInit(bool continued, Direction direction, int seq, int seq2, Player[] players)
        {
            Trace.WriteLine(string.Format("{0} {1}{2}{3}, points: {4}", 
                !continued ? "Init" : "Reinit", 
                direction, 
                seq, 
                seq2 != 0 ? "(" + seq2 + ")" : "", 
                string.Join(", ", players.Select(player => player.point))));
        }

        protected override void OnAgari(Player who, Player fromWho, int point, int[] pointDeltas, Player[] players)
        {
            Trace.WriteLine(string.Format("{0}{1} {2}, points: {3}",
                who == null ? "Ryuukyoku" : who == fromWho ? "Tsumo" : "Ron",
                who != null ? " " + who.id + "<--" + fromWho.id : " ",
                point,
                string.Join(", ", players.Select(player => player.point).Zip(pointDeltas, (point1, delta) => string.Format("{0} ({1}{2})", point1, delta > 0 ? "+" : "", delta)))));
        }

        protected override void OnUnknownEvent(string str)
        {
            Trace.WriteLine(str);
        }

        private void showCurrentState()
        {
            Trace.WriteLine(client.player.hand.ToString(" ", (handTile) => handTile.Name) + " | " + client.player.fuuro.Tiles.ToString(" ", t => t.Name));
        }
    }
}
