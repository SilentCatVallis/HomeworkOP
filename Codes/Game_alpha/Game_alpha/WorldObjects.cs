using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Game_alpha
{
    public class WorldObjects
    {
        public HashSet<Tuple<Point, Point>> Walls;
        public HashSet<Point> Floor;
        public HashSet<Point> Patrons;
        public Point Exit;
        public Dictionary<int, Bridge> Bridges;
        public Dictionary<int, Switch> Switches;
        public WorldObjects()
        {
            Walls = new HashSet<Tuple<Point, Point>>();
            Floor = new HashSet<Point>();
            Patrons = new HashSet<Point>();
            Bridges = new Dictionary<int, Bridge>();
            Switches = new Dictionary<int, Switch>();
        }

    //public List<>
    }
}
