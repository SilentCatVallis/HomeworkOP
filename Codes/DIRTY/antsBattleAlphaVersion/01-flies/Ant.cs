using System;
using System.Drawing;
using System.Linq;
using mazes;

namespace Ants
{
	public class Ant : WorldObject
	{
		public Ant(Point location) : base(location)
		{
		}

		public override void Act(IWorld world)
		{
			var destination = GetDirection(world);
			Destination = Location.Add(destination);
		}

		private Point GetDirection(IWorld world)
		{
            if (random.Next(5) == 0) return GetRandomDirection(world);
            else
                return GetDirectionTowardTarget(world);
        }

		private Point GetRandomDirection(IWorld readonlyWorld)
		{
			var directions = new[] { new Point(1, 0), new Point(-1, 0), new Point(0, 1), new Point(0, -1) }
				.Where(d => !readonlyWorld.GetObjectsAt(Location.Add(d)).Any())
				.Concat(new[] { new Point(0, 0), })
				.ToList();
			return directions[random.Next(directions.Count)];
		}

		private Point GetDirectionTowardTarget(IWorld world)
		{
            Point endPath = PathFinder.GetDirectionTo(Location, world);
            endPath.X += Location.X;
            endPath.Y += Location.Y;
            if (world.IsFrogCanEat(endPath))
            {
                world.RemoveObject(this);
            }
            return PathFinder.GetDirectionTo(Location, world);
		}

		private static readonly Random random = new Random();
	}
}