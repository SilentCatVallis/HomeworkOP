using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VectorMathematic;

namespace Robots
{
    public class Robot
    {
        private const double maxLinearVelocity = 1.0;
        private const double maxAngularVelocity = 0.05;

        public override string ToString()
        {
            return "Position: " + Position + " Direction: " + Direction;
        }

        public Robot(Vector position, Angle direction)
        {
            Position = position;
            Direction = direction;
        }

        public Vector Position { get; private set; }
        public Angle Direction { get; private set; }
        public double LinearVelocity { get { return maxLinearVelocity; } }
        public double AngularVelocity { get { return maxAngularVelocity; } }

        public Robot Move(RobotCommand command)
        {
            var angularVelocity =
                Math.Min(maxAngularVelocity, Math.Abs(command.AngularVelocity)) * Math.Sign(command.AngularVelocity);
            if (Math.Abs(angularVelocity) < 1e-7)
                angularVelocity = 0;
            var velocity =
                Math.Min(maxLinearVelocity, Math.Abs(command.Velocity)) * Math.Sign(command.Velocity);
            var duration = command.Duration;

            if (angularVelocity == 0)
                return DoLinearMove(velocity, duration);
            if (velocity == 0)
                return DoRotate(angularVelocity, duration);
            else
                return DoAngularMove(angularVelocity, velocity, duration);
        }

        private Robot DoAngularMove(double angularVelocity, double velocity, double duration)
        {
            var radius = Math.Abs(velocity / angularVelocity);
            var angle = new Angle((Math.PI / 2) * Math.Sign(angularVelocity));
            var directionToCircleCenter = Direction.Add(angle);
            var vector = new Vector(directionToCircleCenter, radius);
            var center = Position.Add(vector);
            var rotateAngle = angularVelocity * duration;
            var angleInCircle = new Angle(angularVelocity * duration);
            var newPosition = Position.Rotate(center, new Angle(Math.Sign(velocity)*angleInCircle.Radians));
            var newDirection = Direction.Add(new Angle(angularVelocity * duration));
            return new Robot(newPosition, newDirection);
        }

        public Robot Copy()
        {
            return new Robot(Position.Copy(), Direction.Copy());
        }

        private Robot DoRotate(double angularVelocity, double duration)
        {
            var angle =
                new Angle(angularVelocity * duration);
            var newDirection = Direction.Add(angle);
            return new Robot(Position, newDirection);
        }

        private Robot DoLinearMove(double velocity, double duration)
        {
            var moveLength = duration * velocity;
            var path =
                new Vector(Direction, moveLength);
            var newPosition = Position.Add(path);
            return new Robot(newPosition, Direction);
        }
    }
}
