using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace mazes
{
	public class World : IWorld
	{
		public readonly HashSet<WorldObject> Objects = new HashSet<WorldObject>();
		public readonly Dictionary<Point, HashSet<WorldObject>> Cells = new Dictionary<Point, HashSet<WorldObject>>();
		public Point Cursor { get; set; }
		public long Time { get; set; }
		public int ObjectsCount { get { return Objects.Count; } }

		public void AddObject(WorldObject obj)
		{
			Objects.Add(obj);
			if (!Cells.ContainsKey(obj.Location)) Cells.Add(obj.Location, new HashSet<WorldObject>());
			Cells[obj.Location].Add(obj);
		}

		public bool InsideWorld(Point location)
		{
			return location.X >= 0 && location.Y >= 0 && location.X < Size.Width && location.Y < Size.Height;
		}

		public void RemoveObject(WorldObject obj)
		{
			Objects.Remove(obj);
			Cells[obj.Location].Remove(obj);
		}

		public IEnumerable<WorldObject> GetObjectsAt(Point location)
		{
			return Cells.ContainsKey(location) ? Cells[location] : Enumerable.Empty<WorldObject>();
		}

		public bool Contains<T>(Point location)
		{
			return GetObjectsAt(location).Any(o => o is T);
		}

		public void MakeStep()
		{
			Time++;
			foreach (var obj in Objects.ToList())
			{
				RemoveObject(obj);
				obj.Location = obj.Destination;
				AddObject(obj);
				obj.Act(this);
			}
		}

		public void FreezeWorldSize()
		{
			Size = Objects.Any() ? new Size(Objects.Max(o => o.Location.X) + 1, Objects.Max(o => o.Location.Y) + 1) : new Size(1, 1);
		}

		public Size Size { get; private set; }
	}
}