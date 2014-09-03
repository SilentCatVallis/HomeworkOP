using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace mazes
{
    public class Dirty : WorldObject
    {
        public Dirty(Point location) : base(location)
        {
        }

        public override void Act(IWorld world)
        {
            //if (world.Time % 10 == 0 && world.ObjectsCount < 1000)
            //    world.AddObject(new Food(new Point(random.Next(world.Size.Width), random.Next(world.Size.Height))));
        }

        private static readonly Random random = new Random();
    }
}
