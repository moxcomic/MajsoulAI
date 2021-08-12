using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MahjongAI.Models
{
    class Fuuro: List<FuuroGroup>
    {
        public IEnumerable<Tile> Tiles
        {
            get
            {
                return this.SelectMany(group => group);
            }
        }

        public int VisibleCount
        {
            get
            {
                return this.Where(group => group.type != FuuroType.ankan).Count();
            }
        }
        public int AnkanCount
        {
            get
            {
                return this.Where(group => group.type == FuuroType.ankan).Count();
            }
        }
        public int MinkanCount
        {
            get
            {
                return this.Where(group => (group.type == FuuroType.minkan) || (group.type == FuuroType.kakan)).Count();
            }
        }
    }

    class FuuroGroup : List<Tile>
    {
        public FuuroType type;

        public FuuroGroup(): base() { }

        public FuuroGroup(FuuroType type): base()
        {
            this.type = type;
        }
    }

    enum FuuroType
    {
        chii,
        pon,
        minkan,
        ankan,
        kakan
    }
}
