using System;
using System.Collections.Generic;
using System.Drawing;
using mazes;

namespace Flies
{
    public class PathFinder
    {
        private static IEnumerable<Point> GetNeighbours(Point from, IWorld world)
        {
            //А если перемешивать всех соседей перед тем как вернуть, то поведение мух визуально будет естественнее.
            for (int x = from.X - 1; x <= from.X + 1; ++x)
            {
                for (int y = from.Y - 1; y <= from.Y + 1; ++y)
                {
                    var point = new Point(x, y);
                    if (world.InsideWorld(point) && !(x == from.X && y == from.Y) && (x == from.X || y == from.Y))
                        if (!world.Contains<Wall>(point) && !world.Contains<Source>(point))
                            yield return new Point(x, y);
                }
            }
        }

        private static Point ReturnPath(Point localPoint, Dictionary<Point, Point> paths, Point startPoint)
        {
            while (paths.ContainsKey(localPoint))
            {
                var point = paths[localPoint];
                if (point == startPoint)
                    return localPoint;
                else
                    localPoint = point;
            }
            return Point.Empty;
        }

        public static Point GetDirectionTo(Point source, Point target, IWorld world)
        {
            // Реализуйте поиск в ширину. Используйте функцию GetNeighbours для получения всех соседей, в которые можно перейти.
            var paths = new Dictionary<Point, Point>();
            Queue<Point> queue = new Queue<Point>();
            queue.Enqueue(source);
            while (queue.Count > 0)
            {
                var local = queue.Dequeue();
                if (local == target)
                {
                    var path = ReturnPath(local, paths, source);
                    return new Point(path.X - source.X, path.Y - source.Y);
                }
                foreach (var neighbour in GetNeighbours(local, world))
                    if (!paths.ContainsKey(neighbour))
                    {
                        queue.Enqueue(neighbour);
                        paths[neighbour] = local;
                    }
            }
            return Point.Empty;
        }
    }
}