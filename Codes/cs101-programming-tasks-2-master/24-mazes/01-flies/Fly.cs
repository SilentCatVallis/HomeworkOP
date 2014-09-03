using System;
using System.Drawing;
using System.Linq;
using mazes;

namespace Flies
{
	public class Fly : WorldObject
	{
		public Fly(Point location) : base(location)
		{
		}

		public override void Act(IWorld world)
		{
			if (world.Cursor == Location) world.RemoveObject(this);
			else
			{
				var destination = GetDirection(world);
				Destination = Location.Add(destination);
			}
		}

		private Point GetDirection(IWorld world)
		{
			if (random.Next(5) == 0) return GetRandomDirection(world);
			else return GetDirectionTowardTarget(world);
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
			return PathFinder.GetDirectionTo(Location, world.Cursor, world);
		}

		private static readonly Random random = new Random();
	}
}