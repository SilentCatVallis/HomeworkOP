using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VectorMathematic;

namespace Robots
{
    class Program
    {
        static void Main(string[] args)
        {
            var testData = new TestData();
            RunTests(new TestData());
            Console.WriteLine("\n\n\n\n");
            var totalSimpleTime = 0.0;
            var totalSmartTime = 0.0;
            foreach (var test in testData.robotTests)
            {
                Console.WriteLine("Robot: " + test.Robot + ". Destination: " + test.Destination);
                var simple = new SimpleNavigator(test.Destination, test.Robot).NavigateRobot();
                var smart = new SmartNavigator(test.Destination, test.Robot).NavigateRobot();
                totalSimpleTime += simple.Commands.Sum(x => x.Duration);
                Console.WriteLine("SimpleNavigate duration: " + simple.Commands.Sum(x => x.Duration));
                Console.WriteLine();
                var testRobot = test.Robot;
                foreach (var command in smart.Commands)
                    testRobot = testRobot.Move(command);
                totalSmartTime += smart.Commands.Sum(x => x.Duration);
                Console.WriteLine("SmartNavigate duration: " + smart.Commands.Sum(x => x.Duration));
                Console.WriteLine("-------------------");
            }
            Console.WriteLine("Total simple time: {0}\nTotal smart time: {1}", totalSimpleTime, totalSmartTime);
        }

        private static void RunTests(TestData tests)
        {
            var totalSimpleTime = 0.0;
            var totalSmartTime = 0.0;
            foreach (var test in tests.robotTests)
            {
                double time = 0;
                Console.WriteLine("Robot: " + test.Robot + ". Destination: " + test.Destination);
                var simpleRobot = test.Robot.Copy();
                var simpleNavigator = new SimpleNavigator(test.Destination, simpleRobot);
                while (simpleRobot.Position.Subtract(test.Destination).Length > 1e-6)
                {
                    var command = simpleNavigator.NavigateRobot2(simpleRobot);
                    simpleRobot = simpleRobot.Move(command);
                    time += command.Duration;
                }
                Console.WriteLine(time);
                var smartRobot = test.Robot.Copy();
                var smartNavigator = new SmartNavigator(test.Destination, smartRobot);
                //while (smartRobot.Position.Subtract(test.Destination).Length > 1e-6)
                //{
                //    var command = smartNavigator.NavigateRobot2(smartRobot);
                //    smartRobot = smartRobot.Move(command);
                //    time += command.Duration;
                //}
                Console.WriteLine("-------------------");
            }
            Console.WriteLine("Total simple time: {0}\nTotal smart time: {1}", totalSimpleTime, totalSmartTime);
        }
    }

    class TestData
    {
        public List<TestRobot> robotTests = new List<TestRobot>
        {
            //new TestRobot(new Robot(new Vector(0, 0), new Angle(Math.PI / 2)), new Vector(-15, -15)),
            //new TestRobot(new Robot(new Vector(0, 0), new Angle(Math.PI / 2)), new Vector(-15, 15)),
            //new TestRobot(new Robot(new Vector(0, 0), new Angle(Math.PI / 2)), new Vector(15, 15)),
            //new TestRobot(new Robot(new Vector(0, 0), new Angle(Math.PI / 2)), new Vector(15, -15)),
            //new TestRobot(new Robot(new Vector(0, 0), new Angle(0)), new Vector(5, -10)),
            //new TestRobot(new Robot(new Vector(0, 0), new Angle(0)), new Vector(0, 10)),
            new TestRobot(new Robot(new Vector(0, 0), new Angle(0)), new Vector(10, 0)),
            new TestRobot(new Robot(new Vector(0, 0), new Angle(0)), new Vector(-10, 0)),
            new TestRobot(new Robot(new Vector(0, 0), new Angle(0)), new Vector(0, 100)),
            new TestRobot(new Robot(new Vector(10, 20), new Angle(0)), new Vector(10.1, 20.1)),
            new TestRobot(new Robot(new Vector(10, 20), new Angle(3.0)), new Vector(-100, -20)),
            new TestRobot(new Robot(new Vector(0, -10), new Angle(-1.0)), new Vector(10, 100))
        };
    }

    class TestRobot
    {
        public Robot Robot { get; private set; }
        public Vector Destination { get; private set; }

        public TestRobot(Robot robot, Vector destination)
        {
            Robot = robot;
            Destination = destination;
        }
    }

    public class RobotCommand
    {
        public double Duration { get; private set; } // продолжительность команды в секундах
        public double Velocity { get; private set; } // линейная скорость
        public double AngularVelocity { get; private set; } // угловая скорость

        public RobotCommand(double duration, double velocity, double angularVelocity)
        {
            Duration = duration;
            Velocity = velocity;
            AngularVelocity = angularVelocity;
        }

        public override string ToString()
        {
            return String.Format(
                "Duration: {0}. Velocity: {1}. AngularVelocity: {2}.", Duration, Velocity, AngularVelocity);
        }
    }

    public class NavigatorAnswer
    {
        public IEnumerable<RobotCommand> Commands { get; private set; }
        public double Time { get; private set; }

        public NavigatorAnswer(IEnumerable<RobotCommand> commands, double time)
        {
            Commands = commands;
            Time = time;
        }
    }
}
