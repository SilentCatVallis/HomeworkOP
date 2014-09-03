using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System;

namespace mazes
{
	public class World : IWorld
	{
        public readonly int MouthLength = 6;
        public int frogWait = 0;
        public int digestionTime = -7;
        public readonly HashSet<WorldObject> Objects = new HashSet<WorldObject>();
		public Dictionary<Point, HashSet<WorldObject>> Cells = new Dictionary<Point, HashSet<WorldObject>>();
		//public Point Cursor { get; set; }
		public long Time { get; set; }
		public int ObjectsCount { get { return Objects.Count; } }

        public bool IsFrogCanEat(Point ant)
        {
            foreach (var obj in Objects)
                if (obj is Clean)
                    if (obj.Location.X - ant.X < MouthLength && obj.Location.Y - ant.Y < MouthLength && frogWait >= 0)
                    {
                        frogWait = digestionTime;
                        return true;
                    }
            return false;
        }

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

        public void GenerateNewFood()
        {
            if (Time % 10 == 0 && ObjectsCount < 1000)
            {
                Point point = new Point(random.Next(Size.Width - 2) + 1, random.Next(Size.Height - 2) + 1);
                if (!Contains<Wall>(point) && !Contains<Clean>(point))
                    AddObject(new Dirty(point));
            }
        }

		public void MakeStep()
		{
			Time++;
            if (frogWait < 0)
                frogWait++;
			foreach (var obj in Objects.ToList())
			{
				RemoveObject(obj);
				obj.Location = obj.Destination;
				AddObject(obj);
				obj.Act(this);
			}
            GenerateNewFood();
        }

		public void FreezeWorldSize()
		{
			Size = Objects.Any() ? new Size(Objects.Max(o => o.Location.X) + 1, Objects.Max(o => o.Location.Y) + 1) : new Size(1, 1);
		}

        private static readonly Random random = new Random();
        public Size Size { get; private set; }
	}
}