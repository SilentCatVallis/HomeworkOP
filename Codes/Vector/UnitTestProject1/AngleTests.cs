using System;
using VectorMathematic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Robots;

namespace UnitTestProject1
{
    [TestClass]
    public class TestAngle
    {
        [TestMethod]
        public void TestNormalize()
        {
            var angle = new Angle(3.10);
            Assert.AreEqual(3.10, angle.Radians, 1e-9);
        }

        [TestMethod]
        public void TestInitialVector1()
        {
            var angle = new Angle(new Vector(10, 0));
            Assert.AreEqual(0, angle.Radians);
        }

        [TestMethod]
        public void TestInitialVector3()
        {
            var angle = new Angle(new Vector(-10, 0));
            Assert.AreEqual(Math.PI, angle.Radians, 1e-9);
        }

        [TestMethod]
        public void TestInitialVector4()
        {
            var angle = new Angle(new Vector(0, 10));
            Assert.AreEqual(Math.PI / 2, angle.Radians, 1e-9);
        }

        [TestMethod]
        public void TestInitialVector2()
        {
            var angle = new Angle(new Vector(10, 10));
            Assert.AreEqual(Math.PI / 4, angle.Radians, 1e-9);
        }

        [TestMethod]
        public void TestInitialRad()
        {
            var angle = new Angle(Math.PI * 2);
            Assert.AreEqual(0, angle.Radians, 1e-9);
        }

        [TestMethod]
        public void TestInitialRad2()
        {
            var angle = new Angle(Math.PI);
            Assert.AreEqual(Math.PI, angle.Radians, 1e-9);
        }

        [TestMethod]
        public void TestInitialRad3()
        {
            var angle = new Angle(-Math.PI * 2);
            Assert.AreEqual(0, angle.Radians, 1e-9);
        }

        [TestMethod]
        public void TestInitialRad4_0()
        {
            var angle = new Angle(-Math.PI);
            Assert.AreEqual(Math.PI, angle.Radians, 1e-9);
        }

        [TestMethod]
        public void TestInitialRad4_1()
        {
            var angle = new Angle(-Math.PI * 9);
            Assert.AreEqual(Math.PI, angle.Radians, 1e-9);
        }

        [TestMethod]
        public void TestInitialRad5()
        {
            var angle = new Angle(Math.PI / 4);
            Assert.AreEqual(Math.PI / 4, angle.Radians, 1e-9);
        }

        [TestMethod]
        public void TestInitialRad6()
        {
            var angle = new Angle(Math.PI * 7);
            Assert.AreEqual(Math.PI, angle.Radians, 1e-9);
        }

        [TestMethod]
        public void TestInitialRad7()
        {
            var angle = new Angle(Math.PI * -13 + Math.PI / 2);
            Assert.AreEqual(-Math.PI / 2, angle.Radians, 1e-9);
        }

        [TestMethod]
        public void TestInitialRad8()
        {
            var angle = new Angle(0);
            Assert.AreEqual(0, angle.Radians, 1e-9);
        }



        [TestMethod]
        public void TestToString()
        {
            var angle = new Angle(Math.PI);
            Assert.AreEqual(angle.ToString(), Math.PI.ToString());
        }

        [TestMethod]
        public void TestToString1()
        {
            var vect = new Vector(1, 1);
            var angle = new Angle(vect);
            Assert.AreEqual(angle.ToString(), (Math.PI / 4).ToString());
        }



        [TestMethod]
        public void TestEquals()
        {
            var vect = new Vector(1, 1);
            var angle1 = new Angle(vect);
            Assert.AreEqual(true, angle1 == angle1);
        }

        [TestMethod]
        public void TestEquals1()
        {
            var vect = new Vector(1, 1);
            var angle1 = new Angle(vect);
            var angle2 = new Angle(0.33);
            Assert.AreEqual(false, angle1 == angle2);
        }

        [TestMethod]
        public void TestEquals2()
        {
            var vect = new Vector(1, 1);
            var angle1 = new Angle(vect);
            var angle2 = new Angle(0.33);
            Assert.AreEqual(false, angle1.Equals(angle2));
        }

        [TestMethod]
        public void TestEquals3()
        {
            var angle2 = new Angle(0.33);
            Assert.AreEqual(true, angle2.Equals(angle2));
        }
    }
}
