using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Mazes
{
	public class Robot
	{
		private readonly Maze maze;
		private readonly List<Point> path = new List<Point>();

		public Robot(Maze maze)
		{
			this.maze = maze;
			path.Add(maze.Robot);
		}

		public IEnumerable<Point> Path { get { return path; } }

		public Point Pos
		{
			get { return path.Last(); }
		}

		public void MoveTo(int dx, int dy)
		{
			TryMoveTo(new Point(Pos.X + Math.Sign(dx), Pos.Y + Math.Sign(dy)));
		}

		public void MoveUp()
		{
			TryMoveTo(new Point(Pos.X, Pos.Y - 1));
		}

		public void MoveDown()
		{
			TryMoveTo(new Point(Pos.X, Pos.Y + 1));
		}

		public void MoveLeft()
		{
			TryMoveTo(new Point(Pos.X - 1, Pos.Y));
		}

		public void MoveRight()
		{
			TryMoveTo(new Point(Pos.X + 1, Pos.Y));
		}

		private void TryMoveTo(Point destination)
		{
			if (path.Count > 1000 || maze.IsWall(destination)) 
				throw new Exception("Robot is broken!");
			else path.Add(destination);
		}
	}
}