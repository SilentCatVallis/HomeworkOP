using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Robots;
using VectorMathematic;

namespace UnitTestProject1
{
    [TestClass]
    public class TestRobot
    {
        public static Robot CreateRobot(double x, double y, double duration)
        {
            return new Robot(new Vector(x, y), new Angle(duration));
        }

        [TestMethod]
        public void TestLinearMove1()
        {
            var robot = CreateRobot(1, 1, Math.PI / 2);
            var newRobot = robot.Move(new RobotCommand(10, 1, 0));
            Assert.AreEqual(new Vector(1, 11).X, newRobot.Position.X, 1e-6);
            Assert.AreEqual(new Vector(1, 11).Y, newRobot.Position.Y, 1e-6);
        }

        [TestMethod]
        public void TestLinearMove2()
        {
            var robot = CreateRobot(1, 1, Math.PI);
            var newRobot = robot.Move(new RobotCommand(10, 1, 0));
            Assert.AreEqual(new Vector(-9, 1).X, newRobot.Position.X, 1e-6);
            Assert.AreEqual(new Vector(-9, 1).Y, newRobot.Position.Y, 1e-6);
        }

        [TestMethod]
        public void TestLinearMove3()
        {
            var robot = CreateRobot(1, 1, Math.PI + Math.PI / 4);
            var newRobot = robot.Move(new RobotCommand(10, 1, 0));
            Assert.AreEqual(new Vector(1 - Math.Sqrt(50), 1 - Math.Sqrt(50)).X, newRobot.Position.X, 1e-6);
            Assert.AreEqual(new Vector(1 - Math.Sqrt(50), 1 - Math.Sqrt(50)).Y, newRobot.Position.Y, 1e-6);
        }

        [TestMethod]
        public void TestLinearMove4()
        {
            var robot = CreateRobot(1, 1, Math.PI + Math.PI / 4);
            var newRobot = robot.Move(new RobotCommand(10, -1, 0));
            Assert.AreEqual(new Vector(1 + Math.Sqrt(50), 1 + Math.Sqrt(50)).X, newRobot.Position.X, 1e-6);
            Assert.AreEqual(new Vector(1 + Math.Sqrt(50), 1 + Math.Sqrt(50)).Y, newRobot.Position.Y, 1e-6);
        }

        [TestMethod]
        public void TestLinearMove5()
        {
            var robot = CreateRobot(1, 1, Math.PI + Math.PI / 4);
            var newRobot = robot.Move(new RobotCommand(10, 1000000, 0));
            Assert.AreEqual(new Vector(1 - Math.Sqrt(50), 1 - Math.Sqrt(50)).X, newRobot.Position.X, 1e-6);
            Assert.AreEqual(new Vector(1 - Math.Sqrt(50), 1 - Math.Sqrt(50)).Y, newRobot.Position.Y, 1e-6);
            Assert.AreEqual(new Angle(Math.PI + Math.PI / 4).Radians, newRobot.Direction.Radians, 1e-6);
        }

        [TestMethod]
        public void TestRotation1()
        {
            var robot = CreateRobot(1, 1, Math.PI);
            var newRobot = robot.Move(new RobotCommand(10, 0, 0.05));
            Assert.AreEqual(new Angle(Math.PI + 0.5), newRobot.Direction);
        }

        [TestMethod]
        public void TestRotation2()
        {
            var robot = CreateRobot(1, 1, Math.PI);
            var newRobot = robot.Move(new RobotCommand(10, 0, -0.01));
            Assert.AreEqual(new Angle(Math.PI - 0.1), newRobot.Direction);
        }

        [TestMethod]
        public void TestRotation3()
        {
            var robot = CreateRobot(1, 1, Math.PI);
            var newRobot = robot.Move(new RobotCommand(10, 0, 1));
            Assert.AreEqual(new Angle(Math.PI + 0.5), newRobot.Direction);
        }

        [TestMethod]
        public void TestRotation4()
        {
            var robot = CreateRobot(1, 1, Math.PI / 2);
            var newRobot = robot.Move(new RobotCommand(100, 0, 0.04));
            Assert.AreEqual(new Angle(Math.PI / 2 + 4), newRobot.Direction);
        }

        [TestMethod]
        public void TestMove1()
        {
            var robot = CreateRobot(0, 0, 0);
            var newRobot = robot.Move(new RobotCommand(50, 1, Math.PI / 100));
            Assert.AreEqual(new Angle(Math.PI / 2).Radians, newRobot.Direction.Radians, 1e-6);
            Assert.AreEqual(new Vector(31.8309886183791, 31.8309886183791).X, newRobot.Position.X, 1e-6);
            Assert.AreEqual(new Vector(31.8309886183791, 31.8309886183791).Y, newRobot.Position.Y, 1e-6);
        }

        [TestMethod]
        public void TestMove2()
        {
            var robot = CreateRobot(0, 0, 0);
            var newRobot = robot.Move(new RobotCommand(50, 1, Math.PI / 100));
            newRobot = newRobot.Move(new RobotCommand(50, 1, Math.PI / 100));
            Assert.AreEqual(new Angle(Math.PI), newRobot.Direction);
            Assert.AreEqual(new Vector(0, 2 * 31.8309886183791).X, newRobot.Position.X, 1e-6);
            Assert.AreEqual(new Vector(0, 2 * 31.8309886183791).Y, newRobot.Position.Y, 1e-6);
        }

        [TestMethod]
        public void TestMove3()
        {
            var robot = CreateRobot(0, 0, 0);
            var newRobot = robot.Move(new RobotCommand(50, 1, Math.PI / 100));
            newRobot = newRobot.Move(new RobotCommand(50, 1, Math.PI / 100));
            newRobot = newRobot.Move(new RobotCommand(50, 1, Math.PI / 100));
            Assert.AreEqual(new Angle(-Math.PI / 2), newRobot.Direction);
            Assert.AreEqual(new Vector(-31.8309886183791, 31.8309886183791).X, newRobot.Position.X, 1e-6);
            Assert.AreEqual(new Vector(-31.8309886183791, 31.8309886183791).Y, newRobot.Position.Y, 1e-6);
        }

        [TestMethod]
        public void TestMove4()
        {
            var robot = CreateRobot(0, 0, 0);
            var newRobot = robot.Move(new RobotCommand(50, 1, Math.PI / 100));
            newRobot = newRobot.Move(new RobotCommand(50, 1, Math.PI / 100));
            newRobot = newRobot.Move(new RobotCommand(50, 1, Math.PI / 100));
            newRobot = newRobot.Move(new RobotCommand(50, 1, Math.PI / 100));
            Assert.AreEqual(new Angle(0).Radians, newRobot.Direction.Radians, 1e-6);
            Assert.AreEqual(new Vector(0, 0).X, newRobot.Position.X, 1e-6);
            Assert.AreEqual(new Vector(0, 0).Y, newRobot.Position.Y, 1e-6);
        }

        [TestMethod]
        public void TestMove5()
        {
            var robot = CreateRobot(0, 0, 0);
            var newRobot = robot.Move(new RobotCommand(50, -1, Math.PI / 100));
            Assert.AreEqual(new Angle(Math.PI / 2).Radians, newRobot.Direction.Radians, 1e-6);
            Assert.AreEqual(new Vector(-31.8309886183791, 31.8309886183791).X, newRobot.Position.X, 1e-6);
            Assert.AreEqual(new Vector(-31.8309886183791, 31.8309886183791).Y, newRobot.Position.Y, 1e-6);
        }

        [TestMethod]
        public void TestMove6()
        {
            var robot = CreateRobot(0, 0, Math.PI / 2);
            var newRobot = robot.Move(new RobotCommand(31.41592653, -0.75, 0.05));
            Assert.AreEqual(new Vector(-15, -15).X, newRobot.Position.X, 1e-6);
            Assert.AreEqual(new Vector(-15, -15).Y, newRobot.Position.Y, 1e-6);
        }
    }
}
