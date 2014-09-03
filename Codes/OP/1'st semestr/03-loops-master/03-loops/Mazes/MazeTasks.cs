using System;

namespace Mazes
{
	public static class MazeTasks
	{
        static void RobotMovedToRight(Robot robot, int Width)
        {
            for (int i = 0; i < Width; i++)
                {
                    robot.MoveRight();
                }
        }

        static void RobotMovedToLeft(Robot robot, int Width)
        {
            for (int i = 0; i < Width; i++)
            {
                robot.MoveLeft();
            }
        }

        static void RobotMovedToDown(Robot robot, int Height)
        {
            for (int i = 0; i < Height; i++)
            {
                robot.MoveDown();
            }
        }

        static void RobotMovedToUp(Robot robot, int Height)
        {
            for (int i = 0; i < Height; i++)
            {
                robot.MoveUp();
            }
        }

		public static void MoveOutFromEmptyMaze(Robot robot, int width, int height)
		{
            for (int i = 0; i < width - 3; ++i)
            {
                robot.MoveTo(1, 1);
            }
            robot.MoveDown();
		}

        public static void MoveOutFromSnakeMaze(Robot robot, int width, int height)
		{
            int h = 0;
            while (true)
            {
                if ((h / (width - 1)) % 2 == 0)
                {
                    RobotMovedToRight(robot, width - 3);
                    h += (width - 3);
                    if (h >= (height / 2) * (width) + height)
                        break;
                    RobotMovedToDown(robot, 2);
                    h += 2;
                }
                else
                {
                    RobotMovedToLeft(robot, width - 3);
                    h += width + 3;
                    if (h >= (height / 2) * (width - 3))
                        break;
                    RobotMovedToDown(robot, 2);
                    h += 2;
                }
            }
		}

        public static void MoveOutFromPyramidMaze(Robot robot, int width, int height)
		{
            int horizontal = width - 2 * (height / 2);
            for (int i = 0; horizontal < width; ++i)
            {
                if (i % 4 == 0)
                {
                    RobotMovedToRight(robot, width - 3);
                    width -= 2;
                }
                else if (i % 4 == 2)
                {
                    RobotMovedToLeft(robot, width - 3);
                    width -= 2;
                }
                else
                {
                    RobotMovedToUp(robot, 2);
                }
            }
		}

        public static void MoveOutFromSpiralMaze(Robot robot, int width, int height)
		{
            for (int i = 0; height > 2 && width > 2; ++i)
            {
                if (i % 4 == 0)
                {
                    RobotMovedToRight(robot, width - 3);
                    width -= 2;
                }
                if (i % 4 == 2)
                {
                    RobotMovedToLeft(robot, width - 3);
                    width -= 2;
                }
                if (i % 4 == 1)
                {
                    RobotMovedToDown(robot, height - 3);
                    height -= 2;
                }
                if (i % 4 == 3)
                {
                    RobotMovedToUp(robot, height - 3);
                    height -= 2;
                }
            }
		}

        public static void MoveOutFromDiagonalMaze(Robot robot, int width, int height)
        {
            int i = 0;
            if (width > height)
            {
                while (i < width + height - 7)
                {
                    RobotMovedToRight(robot, (width - 2) / (height - 2));
                    i += (width - 2) / (height - 2);
                    if (i >= width + height - 7)
                        break;
                    robot.MoveDown();
                    i++;
                }
            }
            else
            {
                while (i < width + height - 7)
                {
                    RobotMovedToDown(robot, (height - 2) / (width - 2));
                    i += (height - 2) / (width - 2);
                    if (i >= width + height - 7)
                        break;
                    robot.MoveRight();
                    i++;
                }
            }
        }
	}
}