using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace timus_1325
{
    class Point
    {
        public Point(int y, int x)
        {
            X = x;
            Y = y;
        }
        public int X;
        public int Y;
    }

    class Program
    {
        static char[,] ReadMap(int height, int width)
        {
            var map = new char[height, width];
            for (int i = 0; i < height; ++i)
            {
                var line = Console.ReadLine();
                for (int j = 0; j < width; ++j)
                    map[i, j] = line[j];
            }
            return map;
        }

        static bool IsCellClean(Point point, char[,] map)
        {
            return (map[point.Y, point.X] == '1');
        }

        static IEnumerable<Point> GetNeighbours(Point mainPoint, char[,] map)
        {
            for (int i = mainPoint.Y - 1; i <= mainPoint.Y + 1; ++i)
                for (int j = mainPoint.X - 1; j <= mainPoint.X + 1; ++j)
                    if (i >= 0 && j >= 0 && i < map.GetLength(0) && j < map.GetLength(1))
                        if (!(i == mainPoint.Y && j == mainPoint.X))
                            if (map[i, j] != '0')
                                yield return new Point(i, j);
        }

        static Queue<Point> GenerateNewQueue(Queue<Point> queue, HashSet<Tuple<int, int>> set, int[,] colourMap, char[,] map)
        {
            var newQueue = new Queue<Point>();
            while (queue.Count > 0)
            {
                var local = queue.Dequeue();
                foreach (var neighbour in GetNeighbours(local, map)
                    .Where(neighbour => !set.Contains(new Tuple<int, int>(neighbour.Y, neighbour.X))))
                {
                    if (IsCellClean(local, map) == IsCellClean(neighbour, map))
                    {
                        set.Add(new Tuple<int, int>(neighbour.Y, neighbour.X));
                        queue.Enqueue(neighbour);
                        colourMap[neighbour.Y, neighbour.X] = colourMap[local.Y, local.X];
                    }
                    else
                    {
                        set.Add(new Tuple<int, int>(neighbour.Y, neighbour.X));
                        newQueue.Enqueue(neighbour);
                        colourMap[neighbour.Y, neighbour.X] = colourMap[local.Y, local.X] + 1;
                    }
                }
            }
            return newQueue;
        }

        static int[,] ColourMap(char[,] map, Point computer)
        {
            var colourMap = new int[map.GetLength(0), map.GetLength(1)];
            colourMap[computer.Y, computer.X] = 1;
            var queue = new Queue<Point>();
            var set = new HashSet<Tuple<int, int>>();
            queue.Enqueue(computer);
            set.Add(new Tuple<int, int>(computer.Y, computer.X));
            while (queue.Count > 0)
            {
                queue = GenerateNewQueue(queue, set, colourMap, map);
            }
            return colourMap;
        }

        static int FindPathLength(int[,] colourMap, Point computer, Point fridge, char[,] map)
        {
            var queue = new Queue<Tuple<Point, int>>();
            var set = new HashSet<Tuple<int, int>>();
            queue.Enqueue(new Tuple<Point, int>(fridge, 1));
            while (queue.Count > 0)
            {
                var local = queue.Dequeue();
                if (local.Item1.Y == computer.Y && local.Item1.X == computer.X)
                    return local.Item2;
                foreach (var neighbour in GetNeighbours(local.Item1, map)
                    .Where(neighbour => !set.Contains(new Tuple<int, int>(neighbour.Y, neighbour.X)))
                    .Where(neighbour => colourMap[neighbour.Y, neighbour.X] <= colourMap[local.Item1.Y, local.Item1.X]))
                {
                    set.Add(new Tuple<int, int>(neighbour.Y, neighbour.X));
                    queue.Enqueue(new Tuple<Point, int>(neighbour, local.Item2 + 1));
                }
            }
            return 0;
        }

        static void Main()
        {
            var sizes = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
            int height = sizes[0], width = sizes[1];
            var computer = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
            var fridge = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
            var map = ReadMap(height, width);
            var colourMap = ColourMap(map, new Point(computer[0] - 1, computer[1] - 1));
            if (colourMap[fridge[0] - 1, fridge[1] - 1] == 0)
            {
                Console.WriteLine("0 0");
                return;
            }
            var length = FindPathLength(colourMap,
                new Point(computer[0] - 1, computer[1] - 1),
                new Point(fridge[0] - 1, fridge[1] - 1), map);
            Console.WriteLine("{0} {1}", length, colourMap[fridge[0] - 1, fridge[1] - 1] - 1);
        }
    }
}