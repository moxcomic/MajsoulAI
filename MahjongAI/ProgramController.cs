using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

using MahjongAI.Models;

namespace MahjongAI
{
    class ProgramController: Controller
    {
        Process process;

        public ProgramController(TenhouClient client, string program): base(client)
        {
            this.process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = program,
                    WorkingDirectory = Path.GetDirectoryName(program),
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };
        }

        public override void Start()
        {
            base.Start();
            process.Start();
            process.StandardInput.AutoFlush = true;
            process.OutputDataReceived += process_OutputDataReceived;
            process.BeginOutputReadLine();
        }

        public override void Stop()
        {
            base.Stop();

            if (isRunning)
            {
                try
                {
                    process.Kill();
                    process.OutputDataReceived -= process_OutputDataReceived;
                }
                catch { }
            }
        }

        void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                Trace.TraceInformation("Program output: {0}", e.Data);
                HandleCommand(e.Data.Split());
            }
        }

        void Send(string message)
        {
            process.StandardInput.WriteLine(message);
            Trace.TraceInformation("Program input: {0}", message);
        }

        public override void OnDraw(Tile tile)
        {
            Send(string.Format("draw {0}", tile.Name));
        }

        public override void OnWait(Tile tile, Player fromPlayer)
        {
            Send(string.Format("wait {0} {1}", tile.Name, fromPlayer.id));
        }

        private void HandleCommand(string[] cmd)
        {
            var handTiles = client.player.hand;
            switch (cmd[0])
            {
                case "hand":
                    Send(handTiles.ToString(" ", (tile) => tile.Name));
                    break;
                case "reached":
                    Send(client.gameData.players[int.Parse(cmd[1])].reached.ToString());
                    break;
                case "direction":
                    if (cmd[1] == "0")
                    {
                        Send(client.gameData.direction.ToString());
                    }
                    else
                    {
                        Send(client.gameData.players[int.Parse(cmd[2])].direction.ToString());
                    }
                    break;
                case "graveyard":
                    IEnumerable<Tile> tiles = client.gameData.players[int.Parse(cmd[1])].graveyard;
                    if (cmd.Length > 2 && cmd[2] == "1")
                    {
                        tiles = tiles.Where((tile) => !tile.IsTakenAway);
                    }
                    Send(tiles.ToString(" ", (tile) => tile.Name));
                    break;
                case "dora":
                    Send(client.gameData.dora.ToString(" ", (tile) => tile.Name));
                    break;
                case "fuuro":
                    Send(client.gameData.players[int.Parse(cmd[1])].fuuro.Select((group) => group.ToString(" ", (tile) => tile.Name)).ToString(" "));
                    break;
                case "fuurosuu":
                    Send(client.gameData.players[int.Parse(cmd[1])].fuuro.Count.ToString());
                    break;
                case "discard":
                    client.Discard(handTiles.First((tile) => tile.Name == cmd[1]));
                    break;
                case "tsumokiri":
                    client.Discard(client.gameData.lastTile);
                    break;
                case "reach":
                    client.Reach(handTiles.First((tile) => tile.Name == cmd[1]));
                    break;
                case "pass":
                    client.Pass();
                    break;
                case "pon":
                    Tile tile0 = handTiles.First((tile) => tile.Name == cmd[1]);
                    handTiles.Remove(tile0);
                    Tile tile1 = handTiles.First((tile) => tile.Name == cmd[2]);
                    client.Pon(tile0, tile1);
                    break;
                case "minkan":
                    client.Minkan();
                    break;
                case "chii":
                    client.Chii(handTiles.First((tile) => tile.Name == cmd[1]), handTiles.First((tile) => tile.Name == cmd[2]));
                    break;
                case "ankan":
                    client.Ankan(handTiles.First((tile) => tile.Name == cmd[1]));
                    break;
                case "kakan":
                    client.Kakan(handTiles.First((tile) => tile.Name == cmd[1]));
                    break;
                case "ron":
                    client.Ron();
                    break;
                case "tsumo":
                    client.Tsumo();
                    break;
                case "ryuukyoku":
                    client.Ryuukyoku();
                    break;
                case "nuku":
                    client.Nuku();
                    break;
            }
        }
    }
}
