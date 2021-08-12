using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MahjongAI.Models;

namespace MahjongAI
{
    class Controller
    {
        protected PlatformClient client;
        protected bool isRunning;

        public Controller(PlatformClient client)
        {
            this.client = client;
            this.isRunning = false;
        }

        ~Controller()
        {
            if (isRunning)
            {
                Stop();
            }
        }

        public virtual void Start()
        {
            client.OnDraw += OnDraw;
            client.OnWait += OnWait;
            client.OnDiscard += OnDiscard;
            client.OnGameStart += OnGameStart;
            client.OnGameEnd += OnGameEnd;
            client.OnLogin += OnLogin;
            client.OnClose += OnClose;
            client.OnConnectionException += OnConnectionException;
            client.OnInit += OnInit;
            client.OnAgari += OnAgari;
            client.OnNaki += OnNaki;
            client.OnReach += OnReach;
            client.OnUnknownEvent += OnUnknownEvent;
            isRunning = true;
        }

        public virtual void Stop()
        {
            client.OnDraw -= OnDraw;
            client.OnWait -= OnWait;
            client.OnDiscard -= OnDiscard;
            client.OnGameStart -= OnGameStart;
            client.OnGameEnd -= OnGameEnd;
            client.OnLogin -= OnLogin;
            client.OnClose -= OnClose;
            client.OnConnectionException -= OnConnectionException;
            client.OnInit -= OnInit;
            client.OnAgari -= OnAgari;
            client.OnNaki -= OnNaki;
            client.OnReach -= OnReach;
            client.OnUnknownEvent -= OnUnknownEvent;
            isRunning = false;
        }

        protected virtual void OnLogin(bool resume, bool succeeded)
        {

        }

        protected virtual void OnClose()
        {
            Stop();
        }

        protected virtual void OnConnectionException()
        {
            
        }

        public virtual void OnDraw(Tile tile)
        {
            
        }

        public virtual void OnWait(Tile tile, Player fromPlayer)
        {
            
        }

        protected virtual void OnDiscard(Player player, Tile tile)
        {
            
        }

        protected virtual void OnReach(Player player)
        {

        }

        protected virtual void OnNaki(Player player, FuuroGroup fuuro)
        {

        }

        protected virtual void OnInit(bool continued, Direction direction, int seq, int seq2, Player[] players)
        {

        }

        protected virtual void OnAgari(Player who, Player fromWho, int point, int[] pointDeltas, Player[] players)
        {

        }

        protected virtual void OnGameStart(bool continued)
        {
            
        }

        protected virtual void OnGameEnd()
        {
            
        }

        protected virtual void OnUnknownEvent(string str)
        {
            
        }
    }
}
