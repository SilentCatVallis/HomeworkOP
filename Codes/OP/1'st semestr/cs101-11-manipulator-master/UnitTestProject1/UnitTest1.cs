using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Manipulator;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        void TestGetAngle(double C, double A, double B, double Angle)
        {
            double result = Manipulator.RobotMathematics.GetAngle(C, A, B);
            Assert.AreEqual(Angle, result, 1e-9);
            /*if (result != double.NaN)
            {
                
                    Assert.AreEqual(, 0, );
            }
            else
            {
                Assert.AreEqual(result, Angle);
            }*/
        }

        void TestMoveTo(double X, double Y, double Angle, double [] array)
        {
            var result = Manipulator.RobotMathematics.MoveTo(X, Y, Angle);
            for (int i = 0; i < 3; ++i)
            {
                    Assert.AreEqual(result[i], array[i], 1e-6);
            }
        }

        [TestMethod]
        public void TestMethod1()
        {
            TestGetAngle(2, 2, 2, Math.PI / 3);
        }

        [TestMethod]
        public void TestMethod2()
        {
            TestGetAngle(5, 4, 3, Math.PI / 2);
        }

        [TestMethod]
        public void TestMethod3()
        {
            TestGetAngle(7, 7, 99, double.NaN);
        }

        [TestMethod]
        public void TestMethod4()
        {
            TestGetAngle(99, 7, 2, double.NaN);
        }

        [TestMethod]
        public void TestMethod5()
        {
            TestGetAngle(35, 7, 88, double.NaN);
        }

        [TestMethod]
        public void TestMethod6()
        {
            TestGetAngle(7, 0, 6, double.NaN);
        }

        [TestMethod]
        public void TestMethod7()
        {
            double[] arr = new double[3] { Math.PI / 2, Math.PI / 2, Math.PI / 2 };
            TestMoveTo(12, 5, Math.PI / 2, arr);
        }

        [TestMethod]
        public void TestMethod8()
        {
            double[] arr = new double[3] { Math.PI, Math.PI, Math.PI };
            TestMoveTo(0, 37, Math.PI / 2, arr);
        }
    }
}
