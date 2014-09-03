using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WindowsFormsApplication2;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestZeroFound1()
        {
            var ans = Program.GetZeroLocation(new[,] {{1, 2, 0}, {4, 6, 3}, {5, 7, 8}});
            Assert.AreEqual(ans, new Point(2, 0));
        } 
        [TestMethod]
        public void TestZeroFound2()
        {
            var ans = Program.GetZeroLocation(new[,] {{1, 2, 3}, {4, 6, 0}, {5, 7, 8}});
            Assert.AreEqual(ans, new Point(2, 1));
        }
        [TestMethod]
        public void TestZeroFound3()
        {
            var ans = Program.GetZeroLocation(new[,] {{1, 2, 55}, {4, 6, 3}, {5, 0, 8}});
            Assert.AreEqual(ans, new Point(1, 2));
        }

        [TestMethod]
        public void TestFieldToHash1()
        {
            var ans = Program.GetHash(new[,] {{1, 2, 0}, {4, 6, 3}, {5, 7, 8}});
            Assert.AreEqual(ans, "120463578");
        }

        [TestMethod]
        public void TestFieldToHash2()
        {
            var ans = Program.GetHash(new[,] {{1, 2, 9}, {44, 6, 3}, {5, 7, 8}});
            Assert.AreEqual(ans, "1294463578");
        }

        [TestMethod]
        public void TestFieldToHash3()
        {
            var ans = Program.GetHash(new[,] {{0, 2, 0}, {4, 6, 3}, {5, 0, 0}});
            Assert.AreEqual(ans, "020463500");
        }

        [TestMethod]
        public void TestHashToField1()
        {
            var ans = Program.FromHash("120463578");
            Assert.AreEqual(true, Program.IsEqual(ans, new[,] {{1, 2, 0}, {4, 6, 3}, {5, 7, 8}}));
        }

        [TestMethod]
        public void TestHashToField2()
        {
            var ans = Program.FromHash("120463578");
            Assert.AreEqual(true, Program.IsEqual(ans, new[,] {{1, 2, 0}, {4, 6, 3}, {5, 7, 8}}));
        }

        [TestMethod]
        public void TestHashToField3()
        {
            var ans = Program.FromHash("120463578");
            Assert.AreEqual(true, Program.IsEqual(ans, new[,] {{1, 2, 0}, {4, 6, 3}, {5, 7, 8}}));
        }

        [TestMethod]
        public void TestEquals1()
        {
            var ans = Program.IsEqual(new[,] {{1, 2, 0}, {4, 6, 3}, {5, 7, 8}}, new[,] {{1, 2, 0}, {4, 6, 3}, {5, 7, 8}});
            Assert.AreEqual(true, ans);
        }

        [TestMethod]
        public void TestEquals2()
        {
            var ans = Program.IsEqual(new[,] {{1, 2, 0}, {4, 99, 3}, {5, 7, 8}},
                new[,] {{1, 2, 0}, {4, 6, 3}, {5, 7, 8}});
            Assert.AreEqual(false, ans);
        }

        [TestMethod]
        public void TestEquals3()
        {
            var ans = Program.IsEqual(new[,] {{1, 2, 0}, {4, 6, 3}, {5, 7, 8}}, new[,] {{1, 7, 0}, {4, 6, 7}, {5, 7, 8}});
            Assert.AreEqual(false, ans);
        }

        [TestMethod]
        public void TestAlgo1()
        {
            var ans = Program.AssembleThePuzzle(new[,] {{1, 5, 4}, {6, 3, 8}, {0, 2, 7}});
            Assert.AreEqual(13, ans.Count);
        }

        [TestMethod]
        public void TestAlgo2()
        {
            var ans = Program.AssembleThePuzzle(new[,] {{1, 0, 2}, {3, 4, 5}, {6, 7, 8}});
            Assert.AreEqual(2, ans.Count);
        }

        [TestMethod]
        public void TestAlgo3()
        {
            var ans = Program.AssembleThePuzzle(new[,] { { 1, 0, 2 }, { 4, 3, 5 }, { 6, 7, 8 } });
            Assert.AreEqual(null, ans);
        }

        [TestMethod]
        public void TestAlgo4()
        {
            var ans = Program.AssembleThePuzzle(new[,] { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 } });
            Assert.AreEqual(1, ans.Count);
        }

        [TestMethod]
        public void TestAlgo5()
        {
            var ans = Program.AssembleThePuzzle(new[,] { { 1, 0, 2 }, { 3, 4, 5 }, { 6, 7, 8 } });
            var correct = new List<string>(new[] {"012345678", "102345678"});
            CollectionAssert.AreEqual(correct, ans);
        }

        [TestMethod]
        public void TestAlgo6()
        {
            var ans = Program.AssembleThePuzzle(new[,] { { 1, 4, 2 }, { 6, 3, 5 }, { 0, 7, 8 } });
            var correct = new List<string>(new[] { "012345678", "102345678", "142305678", "142035678", "142635078" });
            Assert.AreEqual(correct.Count, ans.Count);
            for (var i = 0; i < ans.Count; ++i)
                Assert.AreEqual(correct[i], ans[i]);
        }

        [TestMethod]
        public void TestAlgo7()
        {
            var ans = Program.AssembleThePuzzle(new[,] { { 1, 2, 4 }, { 6, 3, 5 }, { 0, 7, 8 } });
            Assert.AreEqual(null, ans);
        }
    }
}
