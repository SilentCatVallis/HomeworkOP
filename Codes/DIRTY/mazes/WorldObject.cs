using System.Drawing;

namespace mazes
{
	public class WorldObject
	{
		public WorldObject(Point location)
		{
			Location = location;
			Destination = location;
		}

		public virtual void Act(IWorld world)
		{
		}

		public Point Location { get; set; }
		public Point Destination { get; protected set; }
		public virtual Image GetImage(Images images, long time)
		{
			return images.GetImage(GetType().Name);
		}
	}
}