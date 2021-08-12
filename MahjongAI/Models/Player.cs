using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MahjongAI.Models
{
    class Player
    {
        public int id;
        public string name;
        public int level;
        public Direction direction;
        public int point;
        public bool reached;
        public Hand hand = new Hand();
        public Graveyard graveyard = new Graveyard();
        public Fuuro fuuro = new Fuuro();
        public SafeTiles safeTiles;

        public Player()
        {
            safeTiles = new SafeTiles(this);
        }
    }
}
