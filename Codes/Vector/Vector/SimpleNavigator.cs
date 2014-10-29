using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VectorMathematic;

namespace Robots
{
    public class SimpleNavigator
    {
        private readonly Vector Destination;
        private Robot Robot;
        private double Time;

        public SimpleNavigator(Vector destination, Robot robot)
        {
            Destination = destination;
            Robot = robot;
            Time = 0;
        }

        public NavigatorAnswer NavigateRobot()
        {
            var commands = GetCommands();
            Time = 0;
            return new NavigatorAnswer(commands, commands.Sum(x => x.Duration));
        }

        public RobotCommand NavigateRobot2(Robot robot)
        {
            Robot = robot;
            var commands = GetCommands();
            return commands.First(x => !(Math.Abs(x.Duration) < 1e-6));
        }

        private List<RobotCommand> GetCommands()
        {
            return new List<RobotCommand> { RotateRobot(), MoveRobot() };
        }

        private RobotCommand RotateRobot()
        {
            var requiredAngle = new Angle(Destination.Subtract(Robot.Position));
            Angle rotationAngle;
            if (requiredAngle > Robot.Direction)
                rotationAngle = requiredAngle.Substract(Robot.Direction);
            else
                rotationAngle = Robot.Direction.Substract(requiredAngle);
            var duration = Math.Abs(rotationAngle.Radians / Robot.AngularVelocity);
            Time += duration;
            return new RobotCommand(duration, 0, Robot.AngularVelocity);
        }

        private RobotCommand MoveRobot()
        {
            var pathLength = Destination.Subtract(Robot.Position).Length;
            var duration = pathLength / Robot.LinearVelocity;
            Time += duration;
            return new RobotCommand(duration, Robot.LinearVelocity, 0);
        }
    }
}
