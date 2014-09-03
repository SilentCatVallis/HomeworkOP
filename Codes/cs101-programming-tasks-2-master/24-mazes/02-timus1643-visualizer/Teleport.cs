using System.Drawing;
using mazes;

namespace Timus1643.Visualization
{
	public class Teleport : WorldObject
	{
		public Teleport(Point loc, char name) : base(loc)
		{
			Name = name;
		}

		public char Name { get; private set; }

		public override Image GetImage(Images images, long time)
		{
			return images.GetChar(Name);

		}
	}
}