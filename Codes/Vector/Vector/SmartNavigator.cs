using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VectorMathematic;

namespace Robots
{
    class SmartNavigator
    {
        private readonly Vector Destination;
        private readonly Robot LocalRobot;
        private double Time;

        public SmartNavigator(Vector destination, Robot robot)
        {
            Destination = destination;
            LocalRobot = robot;
            Time = 0;
        }

        private IEnumerable<RobotCommand> GetCommandsNew(Robot robot)
        {
            var radius = robot.LinearVelocity / robot.AngularVelocity;
            var middle = Destination.Subtract(robot.Position).Multiply(1.0 / 2.0);
            var triangleSideLength = Math.Sqrt(Math.Pow(radius, 2) - Math.Pow(middle.Length, 2));
            var leftAngleSize = new Angle(middle).Substract(robot.Direction);
            var sign = 0;
            if (leftAngleSize.Radians > 0)
                sign = 1;
            else
                sign = -1;
            //var sign = Math.Sign(robot.Direction.Substract(new Angle(middle)).Radians);
            var angleForVectorInNewCentre = new Angle(middle).Add(new Angle(sign * Math.PI / 2));
            var newCenter = middle.Add(new Vector(angleForVectorInNewCentre, triangleSideLength));
            var angleFromCentre = new Angle(newCenter.Subtract(robot.Position));
            var circleTangentAngle = angleFromCentre.Add(new Angle(sign * Math.PI / 2));//не хочет ехать задом и поворачивать на малый угол :(
            if (Math.Abs(circleTangentAngle.Radians) > Math.PI / 2)
                circleTangentAngle = circleTangentAngle.Add(new Angle(-1 * sign * Math.PI / 2));
            var rotateAngle = robot.Direction.Substract(circleTangentAngle);
            var normalizeRotateAngle = new Angle(-1 * rotateAngle.Radians);
            if (normalizeRotateAngle.Radians < -Math.PI / 2)
                normalizeRotateAngle = new Angle(new Angle(Math.PI).Add(normalizeRotateAngle).Radians);
            if (normalizeRotateAngle.Radians > Math.PI / 2)
                normalizeRotateAngle = new Angle(normalizeRotateAngle.Substract(new Angle(Math.PI)).Radians);
            var rotateDuration = Math.Abs(normalizeRotateAngle.Radians / robot.AngularVelocity);
            if (!Double.IsNaN(rotateDuration))
                yield return new RobotCommand(
                    rotateDuration, 0, Math.Sign(normalizeRotateAngle.Radians) * robot.AngularVelocity);
            else
            {
                yield return new RobotCommand(Double.PositiveInfinity, 0, 0);
                yield break;
            }
            var angleInCircle = 2 * Math.Asin(middle.Length / radius);
            var duration = Math.Abs(angleInCircle / robot.AngularVelocity);
            if (duration < 1e-6)
                duration = (middle.Length * 2) / robot.LinearVelocity;
            if (Double.IsNaN(duration))
                duration = Double.PositiveInfinity;
            yield return new RobotCommand(
                duration, robot.LinearVelocity, Math.Sign(normalizeRotateAngle.Radians) * robot.AngularVelocity);
        }

        public NavigatorAnswer NavigateRobot()
        {
            var commands = new List<List<RobotCommand>>();
            foreach (var newCommand in DoSmallRotate())
                commands.Add(newCommand);
            var goodCommands = commands.Where(x => x.All(y => !Double.IsNaN(y.Duration))).OrderBy(x => x.Sum(y => y.Duration)).First();
            return new NavigatorAnswer(goodCommands, Time);
        }

        public RobotCommand NavigateRobot2(Robot robot)
        {
            var commands = new List<List<RobotCommand>>();
            foreach (var newCommand in DoSmallRotate())
                commands.Add(newCommand);
            var goodCommands = commands.Where(x => x.All(y => !Double.IsNaN(y.Duration))).OrderBy(x => x.Sum(y => y.Duration)).First();
            var command = goodCommands.First(x => !(Math.Abs(x.Duration) < 1e-6));
            return command;
        }

        private List<List<RobotCommand>> DoSmallRotate()
        {
            var vectorFromRobotToDestination = Destination.Subtract(LocalRobot.Position);
            var angleFromDirectionToDestination = new Angle(vectorFromRobotToDestination).Substract(LocalRobot.Direction);
            var lastDirection = LocalRobot.Direction;
            var ans = new List<List<RobotCommand>>();
            for (int i = 0; i <= 180; i++)
            {
                var localAngle = i * Math.PI / 180;
                var angle = LocalRobot.Direction.Add(new Angle(localAngle));
                var robot = new Robot(LocalRobot.Position, angle);
                var localAns = new List<RobotCommand>();
                localAns.Add(new RobotCommand(Math.Abs(localAngle) / robot.AngularVelocity, 0, robot.AngularVelocity));
                ans.Add(localAns.Concat(GetCommands(robot)).ToList());
            }
            for (int i = 1; i <= 179; i++)
            {
                var localAngle = i * Math.PI / 180;
                var angle = LocalRobot.Direction.Substract(new Angle(localAngle));
                var robot = new Robot(LocalRobot.Position, angle);
                var localAns = new List<RobotCommand>();
                localAns.Add(new RobotCommand(Math.Abs(localAngle) / robot.AngularVelocity, 0, -1 * robot.AngularVelocity));
                ans.Add(localAns.Concat(GetCommands(robot)).ToList());
            }
            //for (int i = 0; i <= 10; i++)
            //{
            //    var localAngle = i * angleFromDirectionToDestination.Radians / 6;
            //    var angle = LocalRobot.Direction.Substract(new Angle(localAngle));
            //    var robot = new Robot(LocalRobot.Position, angle);
            //    var localAns = new List<RobotCommand>();
            //    localAns.Add(new RobotCommand(Math.Abs(localAngle) / robot.AngularVelocity, 0, robot.AngularVelocity));
            //    ans.Add(localAns.Concat(GetCommands(robot)).ToList());
            //}
            return ans;
        }

        private IEnumerable<RobotCommand> GetCommands(Robot robot)
        {
            var radius = robot.LinearVelocity / robot.AngularVelocity;
            var centerLeft = robot.Position.Add(new Vector(new Angle(robot.Direction.Radians + Math.PI / 2), radius));
            var centerRight = robot.Position.Add(new Vector(new Angle(robot.Direction.Radians - Math.PI / 2), radius));
            var middle = Destination.Subtract(robot.Position).Multiply(1.0 / 2.0);
            if (centerLeft.Subtract(Destination).Length < radius)
                yield return GetMoveInCircle(middle, 1, robot);
            else if (centerRight.Subtract(Destination).Length < radius)
                yield return GetMoveInCircle(middle, -1, robot);
            else if (centerLeft.Subtract(Destination).Length < centerRight.Subtract(Destination).Length)
                yield return GetMoveOutCircle(middle, 1, robot);
            else
                yield return GetMoveOutCircle(middle, -1, robot);
            Time = 0;
        }

        private RobotCommand GetMoveOutCircle(Vector middle, double sign1, Robot robot)
        {
            var angleLeftFromMiddle = new Angle(middle).Add(new Angle(sign1 * Math.PI / 2));
            var angleLeft = robot.Direction.Add(new Angle(sign1 * Math.PI / 2));
            double angleInTriangle = 0;
            if (Math.Sign(angleLeft.Radians) == Math.Sign(angleLeftFromMiddle.Radians))
            {
                if (Math.Abs(angleLeft.Radians) >= Math.Abs(angleLeftFromMiddle.Radians))
                    angleInTriangle = Math.Abs(angleLeft.Radians) - Math.Abs(angleLeftFromMiddle.Radians);
                else
                    angleInTriangle = Math.Abs(angleLeftFromMiddle.Radians) - Math.Abs(angleLeft.Radians);
            }
            else if (angleLeft.Radians > 0)
            {
                var locAng = angleLeftFromMiddle.Add(new Angle(Math.PI));
                if (Math.Abs(angleLeft.Radians) >= Math.Abs(locAng.Radians))
                    angleInTriangle = Math.Abs(angleLeft.Radians) - Math.Abs(locAng.Radians);
                else
                    angleInTriangle = Math.Abs(locAng.Radians) - Math.Abs(angleLeft.Radians);
            }
            else
            {
                var locAng = angleLeft.Add(new Angle(Math.PI));
                if (Math.Abs(locAng.Radians) >= Math.Abs(angleLeftFromMiddle.Radians))
                    angleInTriangle = Math.Abs(locAng.Radians) - Math.Abs(angleLeftFromMiddle.Radians);
                else
                    angleInTriangle = Math.Abs(angleLeftFromMiddle.Radians) - Math.Abs(locAng.Radians);
            }
            angleInTriangle = Math.PI / 2 - angleInTriangle;
            //angleInTriangle = Math.Abs(new Angle(middle).Add(angleLeft).Radians);
            var radiusInNewCentre = Math.Abs(middle.Length / Math.Cos(angleInTriangle));
            var newCentre = new Vector(angleLeft, radiusInNewCentre).Add(robot.Position);
            var newSpeed = robot.LinearVelocity / radiusInNewCentre;
            var angleInCircuit = Math.Pow(Destination.Subtract(newCentre).Length, 2);//теорема коминусов; решение треугольника зня 3 его стороны;
            angleInCircuit += Math.Pow(robot.Position.Subtract(newCentre).Length, 2);
            angleInCircuit -= Math.Pow(Destination.Subtract(robot.Position).Length, 2);
            angleInCircuit /= (2 * Destination.Subtract(newCentre).Length * robot.Position.Subtract(newCentre).Length);
            angleInCircuit = Math.Acos(angleInCircuit);
            var sign = Math.Sign(
                Math.PI / 2 - 
                Math.Abs(robot.Direction.Substract(new Angle(Destination.Subtract(robot.Position))).Radians));
            if (sign == 0)
                sign = 1;
            double duration = angleInCircuit / newSpeed;
            if (angleInCircuit < 1e-6 || newSpeed < 1e-6)
                duration = Destination.Subtract(robot.Position).Length / robot.LinearVelocity;
            return new RobotCommand(duration, sign * robot.LinearVelocity, sign1 * newSpeed);
        }

        private RobotCommand GetMoveInCircle(Vector middle, double sign1, Robot robot)
        {
            var angleLeftFromMiddle = new Angle(middle).Add(new Angle(sign1 * Math.PI / 2));
            var angleLeft = robot.Direction.Add(new Angle(sign1 * Math.PI / 2));
            double angleInTriangle = 0;
            if (Math.Sign(angleLeft.Radians) == Math.Sign(angleLeftFromMiddle.Radians))
            {
                if (Math.Abs(angleLeft.Radians) >= Math.Abs(angleLeftFromMiddle.Radians))
                    angleInTriangle = Math.Abs(angleLeft.Radians) - Math.Abs(angleLeftFromMiddle.Radians);
                else
                    angleInTriangle = Math.Abs(angleLeftFromMiddle.Radians) - Math.Abs(angleLeft.Radians);
            }
            else if (angleLeft.Radians > 0)
            {
                var locAng = angleLeftFromMiddle.Add(new Angle(Math.PI));
                if (Math.Abs(angleLeft.Radians) >= Math.Abs(locAng.Radians))
                    angleInTriangle = Math.Abs(angleLeft.Radians) - Math.Abs(locAng.Radians);
                else
                    angleInTriangle = Math.Abs(locAng.Radians) - Math.Abs(angleLeft.Radians);
            }
            else
            {
                var locAng = angleLeft.Add(new Angle(Math.PI));
                if (Math.Abs(locAng.Radians) >= Math.Abs(angleLeftFromMiddle.Radians))
                    angleInTriangle = Math.Abs(locAng.Radians) - Math.Abs(angleLeftFromMiddle.Radians);
                else
                    angleInTriangle = Math.Abs(angleLeftFromMiddle.Radians) - Math.Abs(locAng.Radians);
            }
            angleInTriangle = Math.PI / 2 - angleInTriangle;
            //var angleInTriangle = Math.Abs(new Angle(middle).Add(angleLeft).Radians);
            var radiusInNewCentre = Math.Abs(middle.Length / Math.Cos(angleInTriangle));
            var newCentre = new Vector(angleLeft, radiusInNewCentre).Add(robot.Position);
            var newSpeed = Math.Min(radiusInNewCentre * robot.AngularVelocity, robot.LinearVelocity);
            var angleInCircuit = Math.Acos((
                Math.Pow(Destination.Subtract(newCentre).Length, 2) +
                Math.Pow(robot.Position.Subtract(newCentre).Length, 2) -
                Math.Pow(Destination.Subtract(robot.Position).Length, 2)) /
                (2 * Destination.Subtract(newCentre).Length * robot.Position.Subtract(newCentre).Length));
            var sign = Math.Sign(
                Math.PI / 2 - Math.Abs(robot.Direction.Substract(new Angle(Destination.Subtract(robot.Position))).Radians));
            if (sign == 0)
                sign = 1;
            return new RobotCommand(
                angleInCircuit / robot.AngularVelocity, sign * newSpeed, sign1 * robot.AngularVelocity);
        }
    }
}
