using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Configuration;

using MahjongAI.Models;
using Newtonsoft.Json;

namespace MahjongAI
{
    class Program
    {
        static PlatformClient client;
        static Monitor monitor;
        static AIController controller;
        static AutoResetEvent gameEnd = new AutoResetEvent(false);

        static void Start(Models.Config config)
        {
            if (config.Platform == Platform.Tenhou)
            {
                client = new TenhouClient(config);
            }
            else if (config.Platform == Platform.Majsoul)
            {
                client = new MajsoulClient(config);
            }

            gameEnd.Reset();

            client.OnLogin += (resume, succeeded) =>
            {
                if (!resume && succeeded)
                {
                    //Thread.Sleep(3000);
                    //client.EnterPrivateRoom(config.PrivateRoom);
                    //client.Join(config.GameType);
                }
            };
            client.OnGameEnd += () =>
            {
                config.Repeat--;
                //gameEnd.Set();
                new Thread(() =>
                {
                    if (config.Repeat > 0)
                    {
                        Thread.Sleep(new Random().Next(20, 71) * 1000);
                        client.Join(config.GameType);
                    }
                    else
                    {
                        Thread.Sleep(5000);
                        Console.WriteLine("Repeat End, Logout...");
                        client.Close();
                    }
                }).Start();
            };
            client.OnConnectionException += () =>
            {
                if (config.Platform == Platform.Tenhou && config.TenhouID.Length <= 8) // 如果没有天凤账号，无法断线重连
                {
                    config.Repeat--;
                }
                //gameEnd.Set();
            };

            monitor = new Monitor(client);
            monitor.Start();

            controller = new AIController(client, config.strategy);
            controller.Start();

            client.Login();
        }

        static void HandleInput(Models.Config config)
        {
            while (true)
            {
                string input = Console.ReadLine();
                switch (input.ToLower())
                {
                    case "q":
                        Console.WriteLine("Quiting...");
                        config.Repeat = 1;
                        break;
                    case "g":
                        Console.WriteLine("Exiting...");
                        client.Close();
                        break;
                    case "j":
                        Console.WriteLine("Join Room...");
                        client.EnterPrivateRoom(config.PrivateRoom);
                        break;
                    case "s":
                        Console.WriteLine("Start...");
                        client.Join(config.GameType);
                        break;
                }
            }
        }

        static Models.Config GetConfig()
        {
            var config = new Models.Config()
            {
                Platform = Platform.Majsoul,
                TenhouID = "",
                MajsoulRegion = MajsoulRegion.CN_INTERNATIONAL,
                MajsoulUsername = "",
                MajsoulPassword = "",
                PrivateRoom = 0,
                Repeat = 1,

                strategy = new Strategy() { DefenceLevel = Strategy.DefenceLevelType.Default },

                AuthServer = "",
                AccessToken = "",
                GameLevel = "Normal",
                GameMode = "Normal",
                MatchMode = "East",
                DefenceLevel = 3,
                DeviceUuid = ""
            };

            var ok = false;
            try
            {
                config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));
                ok = true;
            } catch
            {

            }

            //config.Platform = (Platform)Enum.Parse(typeof(Platform), ConfigurationManager.AppSettings["Platform"]);
            //config.Platform = Platform.Majsoul;

            //config.PrivateRoom = int.Parse(ConfigurationManager.AppSettings["PrivateRoom"]);

            //var repeat = ConfigurationManager.AppSettings["Repeat"];
            //config.Repeat = repeat == "INF" ? int.MaxValue : int.Parse(repeat);

            config.GameType = new GameType();

            var gameType_Match = config.MatchMode; //ConfigurationManager.AppSettings["GameType_Match"];
            var matchName = Enum.GetNames(typeof(GameType)).FirstOrDefault(n => n == "Match_" + gameType_Match);
            config.GameType |= matchName != null ? (GameType)Enum.Parse(typeof(GameType), matchName) : 0;

            var gameType_Level = config.GameLevel; //ConfigurationManager.AppSettings["GameType_Level"];
            var levelName = Enum.GetNames(typeof(GameType)).FirstOrDefault(n => n == "Level_" + gameType_Level);
            config.GameType |= levelName != null ? (GameType)Enum.Parse(typeof(GameType), levelName) : 0;

            config.strategy = new Strategy();

            config.strategy.DefenceLevel = (Strategy.DefenceLevelType)config.DefenceLevel;

            if (config.Platform == Platform.Tenhou)
            {
                config.TenhouID = ConfigurationManager.AppSettings["TenhouID"];

                var gameType_Mode = ConfigurationManager.AppSettings["GameType_Mode"];
                var modeName = Enum.GetNames(typeof(GameType)).FirstOrDefault(n => n == "Mode_" + gameType_Mode);
                config.GameType |= modeName != null ? (GameType)Enum.Parse(typeof(GameType), modeName) : 0;
            }
            else if (config.Platform == Platform.Majsoul)
            {
                //config.MajsoulRegion = (MajsoulRegion)Enum.Parse(typeof(MajsoulRegion), ConfigurationManager.AppSettings["MajsoulRegion"], ignoreCase: true);
                //config.MajsoulUsername = ConfigurationManager.AppSettings["MajsoulUsername"];
                //config.MajsoulPassword = ConfigurationManager.AppSettings["MajsoulPassword"];
            }

            if (!ok)
            {
                File.WriteAllText("config.json", JsonConvert.SerializeObject(config));
            }

            return config;
        }

        static void SelfCheck(Models.Config config)
        {
            try
            {
                MahjongHelper.getInstance();
                if (config.Platform == Platform.Majsoul)
                {
                    new MajsoulHelper().selfCheck();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                Console.ReadKey();
                //Environment.Exit(2);
            }
        }

        static void Main(string[] args)
        {
            var listener = new ConsoleTraceListener();
            listener.Filter = new EventTypeFilter(SourceLevels.Warning);
            Trace.Listeners.Add(listener);
            StreamWriter writer = File.CreateText("log.txt");
            writer.AutoFlush = true;
            Trace.Listeners.Add(new TextWriterTraceListener(writer));
            Models.Config config = GetConfig();

           // SelfCheck(config);

            var handleInputThread = new Thread(() => HandleInput(config));
            handleInputThread.Start();

            while (config.Repeat > 0)
            {
                Start(config);
                gameEnd.WaitOne();
                client.Close();
            }

            handleInputThread.Abort();
        }
    }
}
