using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using MahjongAI.Models;
using MahjongAI.Util;

namespace MahjongAI
{
    class MahjongHelper
    {
        private static List<string> programPaths = new List<string> { "./MahjongLib.exe", @"..\..\..\Release\MahjongLib.exe", "../../../MahjongLib/MahjongLib" };
        private static List<string> workingDirectories = new List<string> { ".", "../../../MahjongLib" };
        private static List<string> resourceFiles = new List<string> { "syanten.dat" };

        private static MahjongHelper instance = null;
        private Process process;
        private Syanten syanten;

        public MahjongHelper()
        {
            string programPath;
            try
            {
                programPath = Path.GetFullPath(programPaths.First(path => File.Exists(path)));
            }
            catch (Exception)
            {
                throw new FileNotFoundException("Couldn't find MahjongLib. Attempted paths:\n" + string.Join("\n", programPaths.Select(path => Path.GetFullPath(path))));
            }

            string workingDirectory;
            try
            {
                workingDirectory = Path.GetFullPath(workingDirectories.First(dir => resourceFiles.All(file => File.Exists(Path.Combine(dir, file)))));
                syanten = new Syanten(Path.Combine(workingDirectory, "syanten.dat"));
            }
            catch (Exception)
            {
                throw new FileNotFoundException("Couldn't find syanten.dat. Attempted paths:\n" + string.Join("\n", workingDirectories.Select(path => Path.GetFullPath(Path.Combine(path, resourceFiles[0])))));
            }

            process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = programPath,
                    WorkingDirectory = workingDirectory,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };

            process.Start();
            process.StandardInput.AutoFlush = true;
        }

        ~MahjongHelper()
        {
            try
            {
                process.Kill();
            }
            catch { }
        }

        public static MahjongHelper getInstance()
        {
            if (instance == null)
            {
                instance = new MahjongHelper();
            }

            return instance;
        }

        public int calcDistance(Hand hand, int fuuroCount, out int normalDistance)
        {
            int[] handTmp = new int[38];
            foreach (var tile in hand)
            {
                handTmp[tile.GeneralId]++;
            }
            syanten.set_tehai(handTmp);

            syanten.set_fuurosuu(fuuroCount);

            var res = syanten.AnySyanten(out normalDistance);
            return res;
        }

        public int calcPoint(Hand hand, Tile lastTile, Direction gameDirection, Direction playerDirection, Fuuro fuuro, Dora dora, bool isRiichi, bool tsumoAgari, bool isLastTile)
        {
            lock (process)
            {
                send("mjscore");

                int[] handTmp = new int[41];
                foreach (var tile in hand)
                {
                    handTmp[getTileId(tile)]++;
                }
                send(string.Join(" ", handTmp));

                send(lastTile.GeneralId);
                send((int)gameDirection + 30);
                send((int)playerDirection + 30);

                send(fuuro.Count);
                foreach (var group in fuuro)
                {
                    send(getFuuroType(group), group.Min(t => t.GeneralId));
                }

                int[] doraTmp = new int[10];
                for (int i = 0; i < dora.Count; i++)
                {
                    doraTmp[i] = dora[i].GeneralId;
                }
                send(string.Join(" ", doraTmp));

                send(isRiichi ? 1 : 0);
                send(tsumoAgari ? 1 : 0);
                send(isLastTile ? 1 : 0);

                var res = int.Parse(process.StandardOutput.ReadLine());
                return res;
            }
        }

        private void send(params object[] obj)
        {
            var str = string.Join(" ", obj);
            //Console.WriteLine(str);
            process.StandardInput.WriteLine(str);
        }

        private string recv()
        {
            return process.StandardOutput.ReadLine();
        }

        private int getTileId(Tile tile)
        {
            if (tile.Name == "5m*")
            {
                return 38;
            }
            else if (tile.Name == "5p*")
            {
                return 39;
            }
            else if (tile.Name == "5s*")
            {
                return 40;
            }
            else
            {
                return tile.GeneralId;
            }
        }

        private string getFuuroType(FuuroGroup group)
        {
            var hasRedDora = group.Any(t => t.isRedDora);
            if (hasRedDora && group.type == FuuroType.chii)
            {
                return "aka_chii";
            }
            else if (hasRedDora && group.type == FuuroType.pon)
            {
                return "aka_pon1";
            }
            else
            {
                return group.type.ToString();
            }
        }
    }
}
