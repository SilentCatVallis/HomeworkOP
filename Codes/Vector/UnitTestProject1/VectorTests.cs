using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VectorMathematic;
using Robots;

namespace UnitTestProject1
{
    [TestClass]
    public class TestVector
    {
        [TestMethod]
        public void TestMult()
        {
            var vector = new Vector(100, 7);
            var result = vector.Multiply(3);
            Assert.AreEqual(300, result.X);
            Assert.AreEqual(21, result.Y);
        }

        [TestMethod]
        public void TestAdd()
        {
            var vect1 = new Vector(15, 11);
            var vect2 = new Vector(4.5, 33.9);
            var ans = vect1.Add(vect2);
            Assert.AreEqual(19.5, ans.X, 1e-9);
            Assert.AreEqual(44.9, ans.Y, 1e-9);
        }

        [TestMethod]
        public void TestLen()
        {
            var vect = new Vector(1, -1);
            var ans = vect.Length;
            Assert.AreEqual(ans, Math.Sqrt(2), 1e-9);
        }

        [TestMethod]
        public void TestLen1()
        {
            var vect = new Vector(0, 0);
            var ans = vect.Length;
            Assert.AreEqual(ans, 0, 1e-9);
        }

        [TestMethod]
        public void TestLen2()
        {
            var vect = new Vector(100, 0);
            var ans = vect.Length;
            Assert.AreEqual(ans, 100, 1e-9);
        }

        [TestMethod]
        public void TestToString()
        {
            var vect = new Vector(100, 0);
            Assert.AreEqual(vect.ToString(), "100 ; 0");
        }

        [TestMethod]
        public void TestToString1()
        {
            var vect = new Vector(1, 13);
            Assert.AreEqual(vect.ToString(), "1 ; 13");
        }

        [TestMethod]
        public void TestEquals()
        {
            var vect1 = new Vector(1, 13);
            var vect2 = new Vector(1, 13);
            Assert.AreEqual(true, vect1 == vect2);
        }

        [TestMethod]
        public void TestEquals1()
        {
            var vect1 = new Vector(1, 1);
            var vect2 = new Vector(1, 13);
            Assert.AreEqual(vect1 == vect2, false);
        }

        [TestMethod]
        public void TestEquals2()
        {
            var vect1 = new Vector(1, 13);
            var vect2 = new Vector(0, 13);
            Assert.AreEqual(vect1 == vect2, false);
        }

        [TestMethod]
        public void TestEquals3()
        {
            var vect1 = new Vector(1, 13);
            var vect2 = new Vector(5, 5);
            Assert.AreEqual(vect1 == vect2, false);
        }

        [TestMethod]
        public void TestEquals4()
        {
            var vect1 = new Vector(1, 13);
            var vect2 = new Vector(5, 5);
            Assert.AreEqual(vect1.Equals(vect2), false);
        }

        [TestMethod]
        public void TestEquals5()
        {
            var vect1 = new Vector(5, 5);
            var vect2 = new Vector(5, 5);
            Assert.AreEqual(vect1.Equals(vect2), true);
        }
    }
}
