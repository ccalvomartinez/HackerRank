using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using InsertSorting;

namespace InsertSortingTest
{
    [TestClass]
    public class InsertSortingTestUnit
    {
        [TestMethod]
        public void TestMethod1()
        {
            Test(
@"6
1 4 3 5 6 2",
@"1 4 3 5 6 2
1 3 4 5 6 2
1 3 4 5 6 2
1 3 4 5 6 2
1 2 3 4 5 6");
        }
        [TestMethod]
        public void TestMethod2()
        {
            Test(
@"6
1 -4 3 -5 6 2",
@"-4 1 3 -5 6 2
-4 1 3 -5 6 2
-5 -4 1 3 6 2
-5 -4 1 3 6 2
-5 -4 1 2 3 6");
        }
        [TestMethod]
        public void TestMethod3()
        {
            Test(
@"5
2 1 3 1 2",
@"1 2 3 1 2
1 2 3 1 2
1 1 2 3 2
1 1 2 2 3");
        }
        private void Test(string input, string expectedOutput)
        {
            var reader = new StringReader(input);
            var writer = new StringWriter();
           
            Problem.Solve(reader, writer);
            var actualOutput = writer.ToString().Trim();

            Assert.AreEqual(expectedOutput, actualOutput);
        }
        [TestMethod]
        public void BigArrayTest()
        {

            const int size = 1000;
            const int bound = 10000;
            File.Delete("biginputArray.txt");
            using (var writer = File.CreateText("biginputArray.txt"))
            {
                writer.WriteLine(size);

                var rd = new Random();
                int element = rd.Next(-bound, bound);
                writer.Write(element);
                for (int i = 1; i < size; i++)
                {
                     element = rd.Next(-bound, bound);
                    writer.Write(" " + element);
                }
                writer.WriteLine();
            }

            using (var reader = File.OpenText("biginputArray.txt"))
            {
                File.Delete("bigoutputArray.txt");
                using (var writer = File.CreateText("bigoutputArray.txt"))
                {
                    Problem.Solve(reader, writer);
                }
            }
        }
    }
}
