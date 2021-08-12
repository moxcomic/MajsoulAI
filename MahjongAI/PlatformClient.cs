using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MahjongAI.Models;

namespace MahjongAI
{
    abstract class PlatformClient
    {
        public event Action<Tile> OnDraw;
        public event Action<Tile, Player> OnWait;
        public event Action<Player, Tile> OnDiscard;
        public event Action<bool, bool> OnLogin;
        public event Action OnClose;
        public event Action OnConnectionException;
        public event Action<bool> OnGameStart;
        public event Action OnGameEnd;
        public event Action<bool, Direction, int, int, Player[]> OnInit;
        public event Action<Player, Player, int, int[], Player[]> OnAgari;
        public event Action<Player, FuuroGroup> OnNaki;
        public event Action<Player> OnReach;
        public event Action<string> OnUnknownEvent;

        public GameData gameData;
        public bool connected;
        public Models.Config config;

        public Player player
        {
            get
            {
                return gameData.player;
            }
        }

        public PlatformClient(Models.Config config)
        {
            this.config = config;
        }

        ~PlatformClient()
        {
            Close();
        }

        public abstract void Close(bool unexpected = false);

        public abstract void Login();

        public abstract void Join(GameType type);

        public abstract void EnterPrivateRoom(int roomNumber);

        public abstract void NextReady();

        public abstract void Bye();

        public abstract void Pass();

        public abstract void Discard(Tile tile);

        public abstract void Pon(Tile tile0, Tile tile1);

        public abstract void Minkan();

        public abstract void Chii(Tile tile0, Tile tile1);

        public abstract void Ankan(Tile tile);

        public abstract void Kakan(Tile tile);

        public abstract void Ron();

        public abstract void Tsumo();

        public abstract void Ryuukyoku();

        public abstract void Nuku();

        public abstract void Reach(Tile tile);

        protected void InvokeOnDraw(Tile tile)
        {
            OnDraw?.Invoke(tile);
        }

        protected void InvokeOnWait(Tile tile, Player player)
        {
            OnWait?.Invoke(tile, player);
        }

        protected void InvokeOnDiscard(Player player, Tile tile)
        {
            OnDiscard?.Invoke(player, tile);
        }

        protected void InvokeOnLogin(bool resume, bool succeeded)
        {
            OnLogin?.Invoke(resume, succeeded);
        }

        protected void InvokeOnClose()
        {
            OnClose?.Invoke();
        }

        protected void InvokeOnConnectionException()
        {
            OnConnectionException?.Invoke();
        }

        protected void InvokeOnGameStart(bool continued)
        {
            OnGameStart?.Invoke(continued);
        }

        protected void InvokeOnGameEnd()
        {
            OnGameEnd?.Invoke();
        }

        protected void InvokeOnInit(bool continued, Direction direction, int seq, int seq2, Player[] players)
        {
            OnInit?.Invoke(continued, direction, seq, seq2, players);
        }

        protected void InvokeOnAgari(Player who, Player fromWho, int point, int[] pointDeltas, Player[] players)
        {
            OnAgari?.Invoke(who, fromWho, point, pointDeltas, players);
        }

        protected void InvokeOnNaki(Player player, FuuroGroup fuuro)
        {
            OnNaki?.Invoke(player, fuuro);
        }

        protected void InvokeOnReach(Player player)
        {
            OnReach?.Invoke(player);
        }

        protected void InvokeOnUnknownEvent(string data)
        {
            OnUnknownEvent?.Invoke(data);
        }
    }
}
