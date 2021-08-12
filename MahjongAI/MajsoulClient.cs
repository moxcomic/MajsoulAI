using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Net;

using Newtonsoft.Json.Linq;
using WebSocketSharp;

using MahjongAI.Models;

using Grpc.Core;
using pb = global::Google.Protobuf;
using Newtonsoft.Json;
using Google.Protobuf;
using System.Configuration;

namespace MahjongAI
{
  class MajsoulClient : PlatformClient
  {
    private const string serverListUrl = "/recommend_list?service=ws-gateway&protocol=ws&ssl=true";
    private const string gameServerListUrlTemplate = "/recommend_list?service=ws-game-gateway&protocol=ws&ssl=true&location={0}";
    private const string replaysFileName = "replays.txt";

    private WebSocket ws;
    private WebSocket wsGame;
    private string username;
    private string password;
    //private MajsoulHelper majsoulHelper = new MajsoulHelper();
    private byte[] buffer = new byte[1048576];
    private IEnumerable<JToken> operationList;
    private bool nextReach = false;
    private bool gameEnded = false;
    private Tile lastDiscardedTile;
    private int accountId = 0;
    private int playerSeat = 0;
    private bool continued = false;
    private bool syncing = false;
    private Queue<MajsoulMessage> pendingActions = new Queue<MajsoulMessage>();
    private bool inPrivateRoom = false;
    private bool continuedBetweenGames = false;
    private bool gameStarted = false;
    private Stopwatch stopwatch = new Stopwatch();
    private Random random = new Random();
    private Dictionary<string, Timer> timers = new Dictionary<string, Timer>();


    private Metadata md = null;
    private Channel channel = null;
    private Ex.Lobby.LobbyClient lobby = null;
    private Ex.FastTest.FastTestClient fast = null;
    private Ex.Notify.NotifyClient notify = null;
    private AsyncServerStreamingCall<Ex.ServerStream> call = null;
    private JToken authData = null;

    public void InitGrpc()
    {
      md = new Metadata { { "access_token", GetDeviceUUID() } };
      channel = new Channel(config.AuthServer, ChannelCredentials.Insecure);
      lobby = new Ex.Lobby.LobbyClient(channel);
    }

    public void HandleSyncGameMessage(string name, JToken data)
    {
            var buf = new List<byte>();
            foreach (var t in (JArray)data)
            {
                buf.Add((byte)t);
            }

      string json = null;

      switch (name)
      {
        case "NotifyRoomGameStart":
          json = JsonConvert.SerializeObject(Ex.NotifyRoomGameStart.Parser.ParseFrom(buf.ToArray()), Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
          break;
        case "NotifyMatchGameStart":
          json = JsonConvert.SerializeObject(Ex.NotifyMatchGameStart.Parser.ParseFrom(buf.ToArray()), Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
          break;
        case "NotifyGameClientConnect":
          json = JsonConvert.SerializeObject(new Ex.ReqCommon { }, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
          break;
        case "NotifyGameEndResult":
          json = JsonConvert.SerializeObject(Ex.NotifyGameEndResult.Parser.ParseFrom(buf.ToArray()), Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
          break;
        case "ActionNewRound":
          json = JsonConvert.SerializeObject(Ex.ActionNewRound.Parser.ParseFrom(buf.ToArray()), Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
          break;
        case "ActionDealTile":
          json = JsonConvert.SerializeObject(Ex.ActionDealTile.Parser.ParseFrom(buf.ToArray()), Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
          break;
        case "ActionDiscardTile":
          json = JsonConvert.SerializeObject(Ex.ActionDiscardTile.Parser.ParseFrom(buf.ToArray()), Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
          break;
        case "ActionChangeTile":
          json = JsonConvert.SerializeObject(Ex.ActionChangeTile.Parser.ParseFrom(buf.ToArray()), Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
          break;
        case "ActionNoTile":
          json = JsonConvert.SerializeObject(Ex.ActionNoTile.Parser.ParseFrom(buf.ToArray()), Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
          break;
        case "ActionHuleXueZhanEnd":
          json = JsonConvert.SerializeObject(Ex.ActionHuleXueZhanEnd.Parser.ParseFrom(buf.ToArray()), Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
          break;
        case "ActionHule":
          json = JsonConvert.SerializeObject(Ex.ActionHule.Parser.ParseFrom(buf.ToArray()), Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
          break;
        case "NotifyEndGameVote":
          json = JsonConvert.SerializeObject(Ex.NotifyEndGameVote.Parser.ParseFrom(buf.ToArray()), Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
          break;
        case "ActionLiuJu":
          json = JsonConvert.SerializeObject(Ex.ActionLiuJu.Parser.ParseFrom(buf.ToArray()), Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
          break;
        case "ActionChiPengGang":
          json = JsonConvert.SerializeObject(Ex.ActionChiPengGang.Parser.ParseFrom(buf.ToArray()), Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
          break;
        case "ActionAnGangAddGang":
          json = JsonConvert.SerializeObject(Ex.ActionAnGangAddGang.Parser.ParseFrom(buf.ToArray()), Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
          break;
        case "ActionMJStart":
          json = JsonConvert.SerializeObject(Ex.ActionMJStart.Parser.ParseFrom(buf.ToArray()), Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
          break;
        case "NotifyPlayerLoadGameReady":
          json = JsonConvert.SerializeObject(Ex.NotifyPlayerLoadGameReady.Parser.ParseFrom(buf.ToArray()), Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
          break;
      }

      if (json == null)
      {
        json = "{}";
      }

      JObject obj = JObject.Parse(json);
      var msg = new MajsoulMessage
      {
        Success = true,
        Type = MajsoulMessageType.RESPONSE,
        MethodName = name,
        Json = obj
      };
      HandleMessage(msg);
    }

    public Task CreateNotify()
    {
      new Thread(async () =>
      {
        while (await call.ResponseStream.MoveNext())
        {
          //Console.WriteLine("receive notify");

          var data = Ex.ServerStream.Parser.ParseFrom(call.ResponseStream.Current.ToByteArray());
          var w = Ex.Wrapper.Parser.ParseFrom(data.Stream);

          //Console.WriteLine("w:" + w.Name);

          string json = null;

          switch (w.Name)
          {
            case "NotifyRoomGameStart":
              json = JsonConvert.SerializeObject(Ex.NotifyRoomGameStart.Parser.ParseFrom(w.Data), Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
              break;
            case "NotifyMatchGameStart":
              json = JsonConvert.SerializeObject(Ex.NotifyMatchGameStart.Parser.ParseFrom(w.Data), Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
              break;
            case "NotifyGameClientConnect":
              json = JsonConvert.SerializeObject(new Ex.ReqCommon { }, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
              break;
            case "NotifyGameEndResult":
              json = JsonConvert.SerializeObject(Ex.NotifyGameEndResult.Parser.ParseFrom(w.Data), Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
              break;
            case "NotifyAccountUpdate":
              json = JsonConvert.SerializeObject(Ex.NotifyAccountUpdate.Parser.ParseFrom(w.Data), Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
              break;
            case "ActionNewRound":
              json = JsonConvert.SerializeObject(Ex.ActionNewRound.Parser.ParseFrom(w.Data), Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
              break;
            case "ActionDealTile":
              json = JsonConvert.SerializeObject(Ex.ActionDealTile.Parser.ParseFrom(w.Data), Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
              break;
            case "ActionDiscardTile":
              json = JsonConvert.SerializeObject(Ex.ActionDiscardTile.Parser.ParseFrom(w.Data), Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
              break;
            case "ActionChangeTile":
              json = JsonConvert.SerializeObject(Ex.ActionChangeTile.Parser.ParseFrom(w.Data), Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
              break;
            case "ActionNoTile":
              json = JsonConvert.SerializeObject(Ex.ActionNoTile.Parser.ParseFrom(w.Data), Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
              break;
            case "ActionHuleXueZhanEnd":
              json = JsonConvert.SerializeObject(Ex.ActionHuleXueZhanEnd.Parser.ParseFrom(w.Data), Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
              break;
            case "ActionHule":
              json = JsonConvert.SerializeObject(Ex.ActionHule.Parser.ParseFrom(w.Data), Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
              break;
            case "NotifyEndGameVote":
              json = JsonConvert.SerializeObject(Ex.NotifyEndGameVote.Parser.ParseFrom(w.Data), Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
              break;
            case "ActionLiuJu":
              json = JsonConvert.SerializeObject(Ex.ActionLiuJu.Parser.ParseFrom(w.Data), Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
              break;
            case "ActionChiPengGang":
              json = JsonConvert.SerializeObject(Ex.ActionChiPengGang.Parser.ParseFrom(w.Data), Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
              break;
            case "ActionAnGangAddGang":
              json = JsonConvert.SerializeObject(Ex.ActionAnGangAddGang.Parser.ParseFrom(w.Data), Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
              break;
            case "ActionMJStart":
              json = JsonConvert.SerializeObject(Ex.ActionMJStart.Parser.ParseFrom(w.Data), Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
              break;
            case "NotifyPlayerLoadGameReady":
              json = JsonConvert.SerializeObject(Ex.NotifyPlayerLoadGameReady.Parser.ParseFrom(w.Data), Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
              break;
          }

          if (json == null)
          {
            json = "{}";
          }

          JObject obj = JObject.Parse(json);
          var msg = new MajsoulMessage
          {
            Success = true,
            Type = MajsoulMessageType.RESPONSE,
            MethodName = w.Name,
            Json = obj
          };
          HandleMessage(msg);
        }
      }).Start();
      return Task.CompletedTask;
    }

    public MajsoulClient(Models.Config config) : base(config)
    {
      username = config.MajsoulUsername;
      password = config.MajsoulPassword;
      InitGrpc();
      //var host = getServerHost(serverListUrl);
      //ws = new WebSocket("wss://" + host, onMessage: OnMessage, onError: OnError);
      //ws.Connect().Wait();
    }

    public override void Close(bool unexpected = false)
    {
      //lock (ws)
      //{
      //    if (connected)
      //    {
      //        connected = false;
      //        if (unexpected)
      //        {
      //            InvokeOnConnectionException();
      //        }
      //        InvokeOnClose();
      //        try
      //        {
      //            ws.Close().Wait();
      //            wsGame.Close().Wait();
      //        }
      //        catch { }
      //    }
      //}

      try
      {
        var res = lobby.softLogout(new Ex.ReqLogout { }, md);
        Environment.Exit(0);
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
    }

    public override void Login()
    {
      //Send(ws, ".lq.Lobby.login", new
      //{
      //    currency_platforms = new[] { 2 },
      //    account = username,
      //    password = EncodePassword(password),
      //    reconnect = false,
      //    device = new { device_type = "pc", browser = "safari" },
      //    random_key = GetDeviceUUID(),
      //    client_version = "0.4.149.w",
      //    gen_access_token = false,
      //}).Wait();
      //new Task(HeartBeat).Start();
      //connected = true;

      if (lobby == null)
      {
        throw new Exception("can't get lobby client");
      }
      try
      {
        expectMessage("Login", timeout: 60000, timeoutMessage: "Login timed out.");

        object res = null;

        if (config.AccessToken == "")
        {
          res = lobby.login(new Ex.ReqLogin
          {
            CurrencyPlatforms = new pb.Collections.RepeatedField<uint> { 2, 6, 8, 10, 11 },
            Account = username,
            Password = EncodePassword(password),
            Reconnect = false,
            RandomKey = GetDeviceUUID(),
            GenAccessToken = true,
            Type = 0
          }, md);
          config.AccessToken = ((Ex.ResLogin)res).AccessToken;
          File.WriteAllText("config.json", JsonConvert.SerializeObject(config));
        }
        else
        {
          try
          {
            var vres = lobby.oauth2Check(new Ex.ReqOauth2Check
            {
              AccessToken = config.AccessToken,
              Type = 0
            }, md);
          }
          catch
          {
            config.AccessToken = "";
            File.WriteAllText("config.json", JsonConvert.SerializeObject(config));
            Console.WriteLine("Please Restart Application...");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            Environment.Exit(0);
          }

          res = lobby.oauth2Login(new Ex.ReqOauth2Login
          {
            AccessToken = config.AccessToken,
            Reconnect = false,
            RandomKey = GetDeviceUUID(),
            GenAccessToken = true,
            Type = 0
          }, md);
        }

        connected = true;

        string json = JsonConvert.SerializeObject(res, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        JObject obj = JObject.Parse(json);
        var msg = new MajsoulMessage
        {
          Success = true,
          Type = MajsoulMessageType.RESPONSE,
          MethodName = "Login",
          Json = obj
        };

        HandleMessage(msg);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public override void Join(GameType type)
    {
      if (lobby == null)
      {
        throw new Exception("can't get lobby client");
      }
      if (!inPrivateRoom && config.PrivateRoom == 0)
      {
        int typeNum = 2;

        if (type.HasFlag(GameType.Match_EastSouth))
        {
          typeNum += 1;
        }

        if (type.HasFlag(GameType.Level_Throne))
        {
          typeNum += 12;
        }
        else if (type.HasFlag(GameType.Level_Jade))
        {
          typeNum += 9;
        }
        else if (type.HasFlag(GameType.Level_Gold))
        {
          typeNum += 6;
        }
        else if (type.HasFlag(GameType.Level_Silver))
        {
          typeNum += 3;
        }

        //Send(ws, ".lq.Lobby.matchGame", new { match_mode = typeNum }).Wait();
        try
        {
          expectMessage("MatchGame", timeout: 60000, timeoutMessage: "Game matching timed out.");
          var res = lobby.matchGame(new Ex.ReqJoinMatchQueue
          {
            MatchMode = (uint)typeNum
          }, md);
          Console.WriteLine("Match Game");
          string json = JsonConvert.SerializeObject(res, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
          JObject obj = JObject.Parse(json);
          var msg = new MajsoulMessage
          {
            Success = true,
            Type = MajsoulMessageType.RESPONSE,
            MethodName = "MatchGame",
            Json = obj
          };
          HandleMessage(msg);
        }
        catch (Exception e)
        {
          Console.WriteLine(e);
        }
      }
      else
      {
        //Send(ws, ".lq.Lobby.readyPlay", new { ready = true }).Wait();
        try
        {
          Thread.Sleep(1000);
          var res = lobby.readyPlay(new Ex.ReqRoomReady { Ready = true }, md);
          Console.WriteLine("Ready Play");
          string json = JsonConvert.SerializeObject(res, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
          JObject obj = JObject.Parse(json);
          var msg = new MajsoulMessage
          {
            Success = true,
            Type = MajsoulMessageType.RESPONSE,
            MethodName = "ReadyPlay",
            Json = obj
          };
          HandleMessage(msg);
        }
        catch (Exception e)
        {
          Console.WriteLine(e);
        }
      }
    }

    public override void EnterPrivateRoom(int roomNumber)
    {
      if (roomNumber != 0)
      {
        try
        {
          //Send(ws, ".lq.Lobby.joinRoom", new { room_id = roomNumber }).Wait();
          var res = lobby.joinRoom(new Ex.ReqJoinRoom
          {
            RoomId = (uint)roomNumber
          }, md);
          Console.WriteLine("Join Room");
          inPrivateRoom = true;
          string json = JsonConvert.SerializeObject(res, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
          JObject obj = JObject.Parse(json);
          var msg = new MajsoulMessage
          {
            Success = true,
            Type = MajsoulMessageType.RESPONSE,
            MethodName = "JoinRoom",
            Json = obj
          };
          HandleMessage(msg);
        }
        catch (Exception e)
        {
          Console.WriteLine(e);
          inPrivateRoom = false;
        }
      }
      else
      {
        inPrivateRoom = false;
      }
    }

    public override void NextReady()
    {
      //Send(wsGame, "confirmNewRound", new { }).Wait();
      try
      {
        var res = fast.confirmNewRound(new Ex.ReqCommon { }, md);
        Console.WriteLine("ConfirmNewRound");
        string json = JsonConvert.SerializeObject(res, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        JObject obj = JObject.Parse(json);
        var msg = new MajsoulMessage
        {
          Success = true,
          Type = MajsoulMessageType.RESPONSE,
          MethodName = "ConfirmNewRound",
          Json = obj
        };
        HandleMessage(msg);
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
    }

    public override void Bye()
    {
      //wsGame.Close();
      //wsGame = null;
      //Console.WriteLine("Bye()");
    }

    public override void Pass()
    {
      doRandomDelay();
      //Send(wsGame, "InputChiPengGang", new { cancel_operation = true, timeuse = stopwatch.Elapsed.Seconds }).Wait();
      try
      {
        var res = fast.inputChiPengGang(new Ex.ReqChiPengGang
        {
          CancelOperation = true,
          Timeuse = (uint)stopwatch.Elapsed.Seconds
        }, md);
        string json = JsonConvert.SerializeObject(res, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        JObject obj = JObject.Parse(json);
        var msg = new MajsoulMessage
        {
          Success = true,
          Type = MajsoulMessageType.RESPONSE,
          MethodName = "InputChiPengGang",
          Json = obj
        };
        HandleMessage(msg);
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
    }

    public override void Discard(Tile tile)
    {
      doRandomDelay();
      //Send(wsGame, "inputOperation", new { type = nextReach ? 7 : 1, tile = tile.OfficialName, moqie = gameData.lastTile == tile, timeuse = stopwatch.Elapsed.Seconds }).Wait();
      try
      {
        var res = fast.inputOperation(new Ex.ReqSelfOperation
        {
          Type = nextReach ? (uint)7 : (uint)1,
          Tile = tile.OfficialName,
          Moqie = gameData.lastTile == tile,
          Timeuse = (uint)stopwatch.Elapsed.Seconds
        }, md);
        string json = JsonConvert.SerializeObject(res, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        JObject obj = JObject.Parse(json);
        var msg = new MajsoulMessage
        {
          Success = true,
          Type = MajsoulMessageType.RESPONSE,
          MethodName = "InputOperation",
          Json = obj
        };

        nextReach = false;
        lastDiscardedTile = tile;
        HandleMessage(msg);
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
    }

    public override void Pon(Tile tile0, Tile tile1)
    {
      doRandomDelay();
      var combination = operationList.First(item => (int)item["Type"] == 3)["Combination"].Select(t => (string)t);
      int index = combination.ToList().FindIndex(comb => comb.Contains(tile0.GeneralName));
      //Send(wsGame, "InputChiPengGang", new { type = 3, index, timeuse = stopwatch.Elapsed.Seconds }).Wait();
      try
      {
        var res = fast.inputChiPengGang(new Ex.ReqChiPengGang
        {
          Type = 3,
          Index = (uint)index,
          Timeuse = (uint)stopwatch.Elapsed.Seconds
        }, md);
        string json = JsonConvert.SerializeObject(res, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        JObject obj = JObject.Parse(json);
        var msg = new MajsoulMessage
        {
          Success = true,
          Type = MajsoulMessageType.RESPONSE,
          MethodName = "InputChiPengGang",
          Json = obj
        };
        HandleMessage(msg);
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
    }

    public override void Minkan()
    {
      doRandomDelay();
      //Send(wsGame, "InputChiPengGang", new { type = 5, index = 0, timeuse = stopwatch.Elapsed.Seconds }).Wait();
      try
      {
        var res = fast.inputChiPengGang(new Ex.ReqChiPengGang
        {
          Type = 5,
          Index = (uint)0,
          Timeuse = (uint)stopwatch.Elapsed.Seconds
        }, md);
        string json = JsonConvert.SerializeObject(res, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        JObject obj = JObject.Parse(json);
        var msg = new MajsoulMessage
        {
          Success = true,
          Type = MajsoulMessageType.RESPONSE,
          MethodName = "InputChiPengGang",
          Json = obj
        };
        HandleMessage(msg);
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
    }

    public override void Chii(Tile tile0, Tile tile1)
    {
      doRandomDelay();
      var combination = operationList.First(item => (int)item["Type"] == 2)["Combination"].Select(t => (string)t);
      int index = combination.ToList().FindIndex(comb => comb.Split('|').OrderBy(t => t).SequenceEqual(new[] { tile0.OfficialName, tile1.OfficialName }.OrderBy(t => t)));
      //Send(wsGame, "InputChiPengGang", new { type = 2, index, timeuse = stopwatch.Elapsed.Seconds }).Wait();
      try
      {
        var res = fast.inputChiPengGang(new Ex.ReqChiPengGang
        {
          Type = 2,
          Index = (uint)index,
          Timeuse = (uint)stopwatch.Elapsed.Seconds
        }, md);
        string json = JsonConvert.SerializeObject(res, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        JObject obj = JObject.Parse(json);
        var msg = new MajsoulMessage
        {
          Success = true,
          Type = MajsoulMessageType.RESPONSE,
          MethodName = "InputChiPengGang",
          Json = obj
        };
        HandleMessage(msg);
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
    }

    public override void Ankan(Tile tile)
    {
      doRandomDelay();
      var combination = operationList.First(item => (int)item["Type"] == 4)["Combination"].Select(t => (string)t);
      int index = combination.ToList().FindIndex(comb => comb.Contains(tile.GeneralName));
      //Send(wsGame, "InputChiPengGang", new { type = 4, index, timeuse = stopwatch.Elapsed.Seconds }).Wait();
      try
      {
        var res = fast.inputChiPengGang(new Ex.ReqChiPengGang
        {
          Type = 4,
          Index = (uint)index,
          Timeuse = (uint)stopwatch.Elapsed.Seconds
        }, md);
        string json = JsonConvert.SerializeObject(res, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        JObject obj = JObject.Parse(json);
        var msg = new MajsoulMessage
        {
          Success = true,
          Type = MajsoulMessageType.RESPONSE,
          MethodName = "InputChiPengGang",
          Json = obj
        };
        HandleMessage(msg);
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
    }

    public override void Kakan(Tile tile)
    {
      doRandomDelay();
      var combination = operationList.First(item => (int)item["Type"] == 6)["Combination"].Select(t => (string)t);
      int index = combination.ToList().FindIndex(comb => comb.Contains(tile.GeneralName) || comb.Contains(tile.OfficialName));
      //Send(wsGame, "InputChiPengGang", new { type = 6, index, timeuse = stopwatch.Elapsed.Seconds }).Wait();
      try
      {
        var res = fast.inputChiPengGang(new Ex.ReqChiPengGang
        {
          Type = 6,
          Index = (uint)index,
          Timeuse = (uint)stopwatch.Elapsed.Seconds
        }, md);
        string json = JsonConvert.SerializeObject(res, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        JObject obj = JObject.Parse(json);
        var msg = new MajsoulMessage
        {
          Success = true,
          Type = MajsoulMessageType.RESPONSE,
          MethodName = "InputChiPengGang",
          Json = obj
        };
        HandleMessage(msg);
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
    }

    public override void Ron()
    {
      //Send(wsGame, "InputChiPengGang", new { type = 9, index = 0 }).Wait();
      try
      {
        var res = fast.inputChiPengGang(new Ex.ReqChiPengGang
        {
          Type = 9,
          Index = 0
        }, md);
        string json = JsonConvert.SerializeObject(res, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        JObject obj = JObject.Parse(json);
        var msg = new MajsoulMessage
        {
          Success = true,
          Type = MajsoulMessageType.RESPONSE,
          MethodName = "InputChiPengGang",
          Json = obj
        };
        HandleMessage(msg);
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
    }

    public override void Tsumo()
    {
      //Send(wsGame, "InputChiPengGang", new { type = 8, index = 0 }).Wait();
      try
      {
        var res = fast.inputChiPengGang(new Ex.ReqChiPengGang
        {
          Type = 8,
          Index = 0
        }, md);
        string json = JsonConvert.SerializeObject(res, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        JObject obj = JObject.Parse(json);
        var msg = new MajsoulMessage
        {
          Success = true,
          Type = MajsoulMessageType.RESPONSE,
          MethodName = "InputChiPengGang",
          Json = obj
        };
        HandleMessage(msg);
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
    }

    public override void Ryuukyoku()
    {
      doRandomDelay();
      //Send(wsGame, "InputChiPengGang", new { type = 10, index = 0, timeuse = stopwatch.Elapsed.Seconds }).Wait();
      try
      {
        var res = fast.inputChiPengGang(new Ex.ReqChiPengGang
        {
          Type = 10,
          Index = 0,
          Timeuse = (uint)stopwatch.Elapsed.Seconds
        }, md);
        string json = JsonConvert.SerializeObject(res, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        JObject obj = JObject.Parse(json);
        var msg = new MajsoulMessage
        {
          Success = true,
          Type = MajsoulMessageType.RESPONSE,
          MethodName = "InputChiPengGang",
          Json = obj
        };
        HandleMessage(msg);
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
    }

    public override void Nuku()
    {
      throw new NotSupportedException();
    }

    public override void Reach(Tile tile)
    {
      nextReach = true;
      player.reached = true;
    }

    private void DelayedNextReady()
    {
      new Thread(() =>
      {
        Thread.Sleep(5000);
        if (!gameEnded)
        {
          NextReady();
        }
      }).Start();
    }

    private void StartGame(JToken data, bool continued)
    {
      //gameStarted = false;

      //Task.Factory.StartNew(() =>
      //{
      //    InvokeOnUnknownEvent("Game found. Connecting...");
      //    while (!gameStarted)
      //    {
      //        wsGame = new WebSocket("wss://" + getServerHost(string.Format(gameServerListUrlTemplate, data["location"])), onMessage: OnMessage, onError: OnError);
      //        wsGame.Connect().Wait();
      //        Send(wsGame, "authGame", new
      //        {
      //            account_id = accountId,
      //            token = data["connect_token"],
      //            game_uuid = data["game_uuid"]
      //        }).Wait();
      //        Thread.Sleep(3000);
      //        if (!gameStarted)
      //        {
      //            InvokeOnUnknownEvent("Failed to connect. Retrying...");
      //        }
      //    }
      //});

      try
      {
        gameStarted = false;
        InvokeOnUnknownEvent("Game found. Connecting...");
        var res = fast.authGame(new Ex.ReqAuthGame
        {
          AccountId = (uint)accountId,
          Token = (string)data["ConnectToken"],
          GameUuid = (string)data["GameUuid"]
        }, md);
        string json = JsonConvert.SerializeObject(res, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        JObject obj = JObject.Parse(json);
        var msg = new MajsoulMessage
        {
          Success = true,
          Type = MajsoulMessageType.RESPONSE,
          MethodName = "AuthGame",
          Json = obj
        };
        HandleMessage(msg);

        SaveReplay((string)data["GameUuid"]);
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
    }

    private int NormalizedPlayerId(int seat)
    {
      return (seat - playerSeat + 4) % 4;
    }

    private void HandleMessage(MajsoulMessage message, bool forSync = false)
    {
      if (syncing && !forSync && message.MethodName != "FetchGamePlayerState")
      {
        pendingActions.Enqueue(message);
        return;
      }

      if (message.MethodName != null && timers.ContainsKey(message.MethodName))
      {
        timers[message.MethodName].Dispose();
      }

      if (!message.Success && message.MethodName != "AuthGame")
      {
        return;
      }
      if (message.MethodName == "Login" || message.MethodName == "Oauth2Login")
      {
        accountId = (int)message.Json["AccountId"];

        if (message.Json["Error"] != null && message.Json["Error"]["Code"] != null)
        {

          InvokeOnLogin(resume: false, succeeded: false);
        }
        else if (message.Json["GameInfo"] != null)
        {
          continued = true;
          fast = new Ex.FastTest.FastTestClient(channel);
          notify = new Ex.Notify.NotifyClient(channel);
          call = notify.Notify(new Ex.ClientStream { }, md);
          _ = CreateNotify();
          InvokeOnLogin(resume: true, succeeded: true);
          authData = message.Json["GameInfo"];
        }
        else
        {
          fast = new Ex.FastTest.FastTestClient(channel);
          notify = new Ex.Notify.NotifyClient(channel);
          call = notify.Notify(new Ex.ClientStream { }, md);
          _ = CreateNotify();
          InvokeOnLogin(resume: false, succeeded: true);
        }
      }
      if (message.MethodName == "NotifyRoomGameStart" || message.MethodName == "NotifyMatchGameStart")
      {
        //StartGame(message.Json, false);
        authData = message.Json;
      }
      else if (message.MethodName == "NotifyGameClientConnect")
      {
        StartGame(authData, false);
      }
      else if (message.MethodName == "NotifyGameSync")
      {
        StartGame(authData, true);
      }
      else if (message.MethodName == "AuthGame")
      {
        gameStarted = true;
        InvokeOnGameStart(continued);

        if (!continued)
        {
          //Send(wsGame, "enterGame", new { }).Wait();
          try
          {
            var res = fast.enterGame(new Ex.ReqCommon { }, md);
            string json = JsonConvert.SerializeObject(res, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            JObject obj = JObject.Parse(json);
            var msg = new MajsoulMessage
            {
              Success = true,
              Type = MajsoulMessageType.RESPONSE,
              MethodName = "EnterGame",
              Json = obj
            };
            HandleMessage(msg);
          }
          catch (Exception e)
          {
            Console.WriteLine(e);
          }
        }
        else
        {
          //Send(wsGame, "syncGame", new { round_id = "-1", step = 1000000 }).Wait();
          try
          {
            var res = fast.syncGame(new Ex.ReqSyncGame
            {
              RoundId = "-1",
              Step = 1000000
            }, md);
            string json = JsonConvert.SerializeObject(res, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            JObject obj = JObject.Parse(json);
            var msg = new MajsoulMessage
            {
              Success = true,
              Type = MajsoulMessageType.RESPONSE,
              MethodName = "SyncGame",
              Json = obj
            };
            HandleMessage(msg);
            continued = false;
          }
          catch (Exception e)
          {
            Console.WriteLine(e);
          }
        }
      }
      else if (message.MethodName == "NotifyPlayerLoadGameReady")
      {
        playerSeat = message.Json["ReadyIdList"].Select(t => (int)t).ToList().IndexOf(accountId);
      }
      else if (message.MethodName == "ActionMJStart")
      {
        gameEnded = false;
      }
      else if (message.MethodName == "NotifyGameEndResult")
      {
        Bye();
        gameEnded = true;
        InvokeOnGameEnd();
      }
      else if (message.MethodName == "NotifyEndGameVote")
      {
        new Thread(() =>
        {
          try
          {
            fast.voteGameEnd(new Ex.ReqVoteGameEnd { Yes = true }, md);
          }
          catch { }
        }).Start();
        Bye();
        gameEnded = true;
        InvokeOnGameEnd();
      }
      else if (message.MethodName == "ActionHule")
      {
        int[] points = message.Json["Scores"].Select(t => (int)t).ToArray();
        int[] rawPointDeltas = message.Json["DeltaScores"].Select(t => (int)t).ToArray();
        int[] pointDeltas = new int[4];

        for (var i = 0; i < 4; i++)
        {
          gameData.players[NormalizedPlayerId(i)].point = points[i];
          pointDeltas[NormalizedPlayerId(i)] = rawPointDeltas[i];
        }

        foreach (var agari in message.Json["Hules"])
        {
          Player who = gameData.players[NormalizedPlayerId((int)agari["Seat"])];
          Player fromWho = pointDeltas.Count(s => s < 0) == 1 ? gameData.players[Array.FindIndex(pointDeltas, s => s < 0)] : who;
          int point = !(bool)agari["Zimo"] ? (int)agari["PointRong"] : (bool)agari["Qinjia"] ? (int)agari["PointZimoXian"] * 3 : (int)agari["PointZimoXian"] * 2 + (int)agari["PointZimoQin"];
          if (gameData.lastTile != null)
          {
            gameData.lastTile.IsTakenAway = true;
          }
          if ((bool)agari["Yiman"])
          {
            SaveReplayTag("Yakuman");
          }
          InvokeOnAgari(who, fromWho, point, pointDeltas, gameData.players);
        }

        DelayedNextReady();
      }
      else if (message.MethodName == "ActionLiuJu")
      {
        InvokeOnAgari(null, null, 0, new[] { 0, 0, 0, 0 }, gameData.players);
        DelayedNextReady();
      }
      else if (message.MethodName == "ActionNoTile")
      {
        var scoreObj = message.Json["Scores"][0];
        int[] rawPointDeltas = scoreObj["DeltaScores"] != null ? scoreObj["DeltaScores"].Select(t => (int)t).ToArray() : new[] { 0, 0, 0, 0 };
        if (rawPointDeltas.Length != 4)
        {
          rawPointDeltas = new[] { 0, 0, 0, 0 };
        }
        int[] pointDeltas = new int[4];
        for (var i = 0; i < 4; i++)
        {
          try
          {
            gameData.players[NormalizedPlayerId(i)].point = (int)scoreObj["OldScores"][i] + rawPointDeltas[i];
            pointDeltas[NormalizedPlayerId(i)] = rawPointDeltas[i];
          }
          catch (Exception e)
          {
            Console.WriteLine(e);
            Console.WriteLine("Index Out Of Range:" + gameData.players.Length + ":" + i + ":" + NormalizedPlayerId(i) + ":" + scoreObj["OldScores"] + ":" + rawPointDeltas.Length);
          }
        }
        InvokeOnAgari(null, null, 0, pointDeltas, gameData.players);
        DelayedNextReady();
      }
      else if (message.MethodName == "ActionNewRound")
      {
        Tile.Reset();
        gameData = new GameData();
        HandleInit(message.Json);

        if (!syncing)
        {
          InvokeOnInit(/* continued */ false, gameData.direction, gameData.seq, gameData.seq2, gameData.players);
        }

        if (player.hand.Count > 13)
        {
          operationList = message.Json["Operation"]["OperationList"];
          if (!syncing)
          {
            Thread.Sleep(2000); // 等待发牌动画结束
            stopwatch.Restart();
            InvokeOnDraw(player.hand.Last());
          }
        }
      }
      else if (message.MethodName == "SyncGame")
      {
        syncing = true;
        continuedBetweenGames = (int)message.Json["Step"] == 0;
        //Send(wsGame, "fetchGamePlayerState", new { }).Wait();
        try
        {
          if (message.Json["GameRestore"]["Actions"] != null)
          {
            foreach (var action in message.Json["GameRestore"]["Actions"])
            {
              HandleSyncGameMessage((string)action["Name"], action["Data"]);
            }
          }
          var res = fast.fetchGamePlayerState(new Ex.ReqCommon { }, md);
          string json = JsonConvert.SerializeObject(res, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
          JObject obj = JObject.Parse(json);
          var msg = new MajsoulMessage
          {
            Success = true,
            Type = MajsoulMessageType.RESPONSE,
            MethodName = "FetchGamePlayerState",
            Json = obj
          };
          HandleMessage(msg);
        }
        catch (Exception e)
        {
          Console.WriteLine(e);
        }
      }
      else if (message.MethodName == "FetchGamePlayerState")
      {
        bool inited = false;
        playerSeat = message.Json["StateList"].ToList().IndexOf("SYNCING") - 2;

        while (pendingActions.Count > 1)
        {
          var actionMessage = pendingActions.Dequeue();
          if (actionMessage.MethodName == "ActionNewRound")
          {
            inited = true;
          }
          HandleMessage(actionMessage, forSync: true);
        }

        //Send(wsGame, "finishSyncGame", new { }).Wait();
        try
        {
          var res = fast.finishSyncGame(new Ex.ReqCommon { }, md);
          string json = JsonConvert.SerializeObject(res, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
          JObject obj = JObject.Parse(json);
          var msg = new MajsoulMessage
          {
            Success = true,
            Type = MajsoulMessageType.RESPONSE,
            MethodName = "FinishSyncGame",
            Json = obj
          };
          HandleMessage(msg);
        }
        catch (Exception e)
        {
          Console.WriteLine(e);
        }
        syncing = false;

        if (inited)
        {
          InvokeOnInit(/* continued */ true, gameData.direction, gameData.seq, gameData.seq2, gameData.players);
        }

        // Queue里的最后一个action需要响应
        if (pendingActions.Count > 0)
        {
          HandleMessage(pendingActions.Dequeue());
        }

        if (continuedBetweenGames)
        {
          NextReady();
        }
      }
      else if (message.MethodName == "ActionDealTile")
      {
        gameData.remainingTile = (int)message.Json["LeftTileCount"];
        if (message.Json["Doras"] != null)
        {
          var doras = message.Json["Doras"].Select(t => (string)t);
          foreach (var dora in doras.Skip(gameData.dora.Count))
          {
            gameData.dora.Add(new Tile(dora));
          }
        }
        if (NormalizedPlayerId((int)message.Json["Seat"]) == 0)
        {
          Tile tile = new Tile((string)message.Json["Tile"]);
          player.hand.Add(tile);
          gameData.lastTile = tile;
          operationList = message.Json["Operation"]["OperationList"];
          if (!syncing)
          {
            stopwatch.Restart();
            InvokeOnDraw(tile);
          }
        }
      }
      else if (message.MethodName == "ActionDiscardTile")
      {
        Player currentPlayer = gameData.players[NormalizedPlayerId((int)message.Json["Seat"])];
        if (!(bool)message.Json["Moqie"])
        {
          currentPlayer.safeTiles.Clear();
        }
        var tileName = (string)message.Json["Tile"];
        if (currentPlayer == player)
        {
          if (lastDiscardedTile == null || lastDiscardedTile.OfficialName != tileName)
          {
            lastDiscardedTile = player.hand.First(t => t.OfficialName == tileName);
          }
          player.hand.Remove(lastDiscardedTile);
        }
        Tile tile = currentPlayer == player ? lastDiscardedTile : new Tile(tileName);
        lastDiscardedTile = null;
        currentPlayer.graveyard.Add(tile);
        gameData.lastTile = tile;
        foreach (var p in gameData.players)
        {
          p.safeTiles.Add(tile);
        }
        if ((bool)message.Json["IsLiqi"] || (bool)message.Json["IsWliqi"])
        {
          currentPlayer.reached = true;
          currentPlayer.safeTiles.Clear();
          if (!syncing) InvokeOnReach(currentPlayer);
        }
        if (!syncing) InvokeOnDiscard(currentPlayer, tile);
        JToken keyValuePairs = message.Json;
        if (keyValuePairs["Doras"] != null)
        {
          var doras = message.Json["Doras"].Select(t => (string)t);
          foreach (var dora in doras.Skip(gameData.dora.Count))
          {
            gameData.dora.Add(new Tile(dora));
          }
        }
        if (keyValuePairs["Operation"] != null)
        {
          operationList = message.Json["Operation"]["OperationList"];
          if (!syncing)
          {
            stopwatch.Restart();
            InvokeOnWait(tile, currentPlayer);
          }
        }
      }
      else if (message.MethodName == "ActionChiPengGang")
      {
        Player currentPlayer = gameData.players[NormalizedPlayerId((int)message.Json["Seat"])];
        var fuuro = HandleFuuro(currentPlayer, (int)message.Json["Type"], message.Json["Tiles"].Select(t => (string)t), message.Json["Froms"].Select(t => (int)t));

        if (!syncing) InvokeOnNaki(currentPlayer, fuuro);
      }
      else if (message.MethodName == "ActionAnGangAddGang")
      {
        Player currentPlayer = gameData.players[NormalizedPlayerId((int)message.Json["Seat"])];
        FuuroGroup fuuro = null;
        if ((int)message.Json["Type"] == 2)
        {
          fuuro = HandleKakan(currentPlayer, (string)message.Json["Tiles"]);
        }
        else if ((int)message.Json["Type"] == 3)
        {
          fuuro = HandleAnkan(currentPlayer, (string)message.Json["Tiles"]);
        }

        if (!syncing) InvokeOnNaki(currentPlayer, fuuro);
      }
    }

    private void HandleInit(JToken data)
    {
      switch ((int)data["Chang"])
      {
        case 0:
          gameData.direction = Direction.E;
          break;
        case 1:
          gameData.direction = Direction.S;
          break;
        case 2:
          gameData.direction = Direction.W;
          break;
      }

      gameData.seq = (int)data["Ju"] + 1;
      gameData.seq2 = (int)data["Ben"];
      gameData.reachStickCount = (int)data["Liqibang"];

      gameData.remainingTile = GameData.initialRemainingTile;

      gameData.dora.Clear();
      gameData.dora.Add(new Tile((string)data["Doras"][0]));

      for (int i = 0; i < 4; i++)
      {
        gameData.players[NormalizedPlayerId(i)].point = (int)data["Scores"][i];
        gameData.players[NormalizedPlayerId(i)].reached = false;
        gameData.players[NormalizedPlayerId(i)].graveyard = new Graveyard();
        gameData.players[NormalizedPlayerId(i)].fuuro = new Fuuro();
        gameData.players[NormalizedPlayerId(i)].hand = new Hand();
      }

      int oyaNum = (4 - playerSeat + (int)data["Ju"]) % 4;
      gameData.players[oyaNum].direction = Direction.E;
      gameData.players[(oyaNum + 1) % 4].direction = Direction.S;
      gameData.players[(oyaNum + 2) % 4].direction = Direction.W;
      gameData.players[(oyaNum + 3) % 4].direction = Direction.N;

      foreach (var tileName in data["Tiles"].Select(t => (string)t))
      {
        player.hand.Add(new Tile(tileName));
      }
    }

    private FuuroGroup HandleFuuro(Player currentPlayer, int type, IEnumerable<string> tiles, IEnumerable<int> froms)
    {
      FuuroGroup fuuroGroup = new FuuroGroup();
      switch (type)
      {
        case 0:
          fuuroGroup.type = FuuroType.chii;
          break;
        case 1:
          fuuroGroup.type = FuuroType.pon;
          break;
        case 2:
          fuuroGroup.type = FuuroType.minkan;
          break;
      }

      foreach (var (tileName, from) in tiles.Zip(froms, Tuple.Create))
      {
        if (NormalizedPlayerId(from) != currentPlayer.id) // 从别人那里拿来的牌
        {
          fuuroGroup.Add(gameData.lastTile);
          gameData.lastTile.IsTakenAway = true;
        }
        else if (currentPlayer == player) // 自己的手牌
        {
          Tile tile = player.hand.First(t => t.OfficialName == tileName);
          player.hand.Remove(tile);
          fuuroGroup.Add(tile);
        }
        else
        {
          fuuroGroup.Add(new Tile(tileName));
        }
      }

      currentPlayer.fuuro.Add(fuuroGroup);
      return fuuroGroup;
    }

    private FuuroGroup HandleAnkan(Player currentPlayer, string tileName)
    {
      tileName = tileName.Replace('0', '5');

      FuuroGroup fuuroGroup = new FuuroGroup();
      fuuroGroup.type = FuuroType.ankan;

      if (currentPlayer == player)
      {
        IEnumerable<Tile> tiles = player.hand.Where(t => t.GeneralName == tileName).ToList();
        fuuroGroup.AddRange(tiles);
        player.hand.RemoveRange(tiles);
      }
      else
      {
        if (tileName[0] == '5' && tileName[1] != 'z') // 暗杠中有红牌
        {
          fuuroGroup.Add(new Tile(tileName));
          fuuroGroup.Add(new Tile(tileName));
          fuuroGroup.Add(new Tile(tileName));
          fuuroGroup.Add(new Tile("0" + tileName[1]));
        }
        else
        {
          for (var i = 0; i < 4; i++)
          {
            fuuroGroup.Add(new Tile(tileName));
          }
        }
      }

      currentPlayer.fuuro.Add(fuuroGroup);
      return fuuroGroup;
    }

    private FuuroGroup HandleKakan(Player currentPlayer, string tileName)
    {
      FuuroGroup fuuroGroup = currentPlayer.fuuro.First(g => g.type == FuuroType.pon && g.All(t => t.GeneralName == tileName.Replace('0', '5')));
      fuuroGroup.type = FuuroType.kakan;

      if (currentPlayer == player)
      {
        Tile tile = player.hand.First(t => t.GeneralName == tileName.Replace('0', '5'));
        player.hand.Remove(tile);
        fuuroGroup.Add(tile);
      }
      else
      {
        fuuroGroup.Add(new Tile(tileName));
      }

      return fuuroGroup;
    }

    private void HeartBeat()
    {
      //while (true)
      //{
      //    Thread.Sleep(60000);
      //    try
      //    {
      //        Send(ws, ".lq.Lobby.heatbeat", new { no_operation_counter = 0 }).Wait();
      //    }
      //    catch (Exception)
      //    {
      //        Close(true);
      //        return;
      //    }
      //}
    }

    private void SaveReplay(string gameID)
    {
      StreamWriter writer = new StreamWriter(replaysFileName, true);
      writer.WriteLine("https://game.maj-soul.com/1/?paipu={0}", gameID);
      writer.Close();
    }

    private void SaveReplayTag(string tag)
    {
      StreamWriter writer = new StreamWriter(replaysFileName, true);
      writer.WriteLine("tag: {0}", tag);
      writer.Close();
    }

    private async Task OnMessage(MessageEventArgs args)
    {
      try
      {
        int length = await args.Data.ReadAsync(buffer, 0, buffer.Length);
        //MajsoulMessage message = majsoulHelper.decode(buffer, 0, length);
        //HandleMessage(message);
      }
      catch (Exception ex)
      {
        Trace.TraceError(ex.ToString());
        Close(true);
      }
    }

    private Task OnError(WebSocketSharp.ErrorEventArgs args)
    {
      return Task.Factory.StartNew(() => Close(true));
    }

    private static string EncodePassword(string password)
    {
      using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes("lailai")))
      {
        return BitConverter.ToString(hmac.ComputeHash(Encoding.UTF8.GetBytes(password))).Replace("-", "").ToLower();
      }
    }

    public async Task Send(WebSocket ws, string methodName, object data)
    {
      /*
       byte[] buffer = majsoulHelper.encode(new MajsoulMessage
      {
          Type = MajsoulMessageType.REQUEST,
          MethodName = methodName,
          Data = data,
      });
      try
      {
          await ws.Send(buffer);
      }
      catch (Exception ex)
      {
          Trace.TraceError(ex.ToString());
          Close(true);
      }
       */
    }

    private string GetDeviceUUID()
    {
      string uuid = config.DeviceUuid; //(string)Properties.Settings.Default["DeviceUUID"];
      if (string.IsNullOrEmpty(uuid))
      {
        uuid = Guid.NewGuid().ToString();
        //Properties.Settings.Default["DeviceUUID"] = uuid;
        //Properties.Settings.Default.Save();
        config.DeviceUuid = uuid;
        File.WriteAllText("config.json", JsonConvert.SerializeObject(config));
      }
      return uuid;
    }

    private string getServerHost(string serverListUrl)
    {
      var webClient = new WebClient();
      var serverListJson = webClient.DownloadString(Constants.MAJSOUL_API_URL_PRIFIX[config.MajsoulRegion] + serverListUrl);
      var serverList = JObject.Parse(serverListJson)["servers"];
      return (string)serverList[0];
    }

    private void doRandomDelay()
    {
      if (stopwatch.Elapsed < TimeSpan.FromSeconds(2))
      {
        Thread.Sleep(random.Next(1, 4) * 1000);
      }
    }

    private void expectMessage(string methodName, int timeout, string timeoutMessage)
    {
      timers[methodName] = new Timer((state) =>
      {
        InvokeOnUnknownEvent(timeoutMessage);
        Close(true);
      }, state: null, dueTime: timeout, period: Timeout.Infinite);
    }
  }
}
