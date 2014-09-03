using System.Collections.Generic;
using System.Drawing;

namespace mazes
{
	public interface IWorld
	{
		Size Size { get; }
        bool IsFrogCanEat(Point ant);
		bool InsideWorld(Point location);
		IEnumerable<WorldObject> GetObjectsAt(Point location);
		bool Contains<T>(Point location);
		int ObjectsCount { get; }
		void AddObject(WorldObject obj);
		void RemoveObject(WorldObject obj);
		long Time { get; }
		//Point Cursor { get; }
		void MakeStep();
		void FreezeWorldSize();
	}
}