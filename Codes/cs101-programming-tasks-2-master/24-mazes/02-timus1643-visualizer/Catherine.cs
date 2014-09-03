using System.Drawing;
using mazes;

namespace Timus1643.Visualization
{
	public class PathObject : WorldObject
	{
		public PathObject(Point location) : base(location)
		{
			Path = new Point[0];
		}

		public void SetPathPlan(Point[] path)
		{
			Path = path;
		}

		private Point[] Path { get; set; }

		public override void Act(IWorld world)
		{
			var i = world.Time - 1;
			if (i < 0 || i >= Path.Length) return;
			Destination = Path[i];
		}
	}

	public class Catherine : PathObject
	{
		public Catherine(Point location)
			: base(location)
		{
		}

	}
}