using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
namespace MatrixRotationTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void PositionOnMatrixFramewokStyleEnumeratorTest()
        {
            var matrix = new MatrixRotation.Matrix<int>(7, 6);
            var i = 1;
            for (int r = 0; r < matrix.RowCount; r++)
            {
                for (int c = 0; c < matrix.ColumnCount; c++)
                {
                    matrix[r, c] = i;
                    i += 1;
                }
            }



            var listOfElements = new List<int>();

            var positionEnumerator = new MatrixRotation.PositionOnMatrixFramewokStyleEnumerator(matrix.RowCount, matrix.ColumnCount);

            while (positionEnumerator.MoveNext())
            {

                listOfElements.Add(matrix[positionEnumerator.Current.Row, positionEnumerator.Current.Column]);
            }

            Assert.AreEqual(
@"1 2 3 4 5 6 12 18 24 30 36 42 41 40 39 38 37 31 25 19 13 7 8 9 10 11 17 23 29 35 34 33 32 26 20 14 15 16 22 28 27 21", String.Join(" ", listOfElements));
        }
        [TestMethod]
        public void MatrixFrameworkStyleEnumeratorTestFrameworkSet()
        {
            var matrix = new MatrixRotation.Matrix<int>(7, 6);
            var i = 1;
            for (int r = 0; r < matrix.RowCount; r++)
            {
                for (int c = 0; c < matrix.ColumnCount; c++)
                {
                    matrix[r, c] = i;
                    i += 1;
                }
            }



            var listOfElements = new List<int>();

            var positionEnumerator = new MatrixRotation.PositionOnMatrixFramewokStyleEnumerator(matrix.RowCount, matrix.ColumnCount, 0);

            while (positionEnumerator.MoveNext())
            {

                listOfElements.Add(matrix[positionEnumerator.Current.Row, positionEnumerator.Current.Column]);
            }

            Assert.AreEqual(
@"1 2 3 4 5 6 12 18 24 30 36 42 41 40 39 38 37 31 25 19 13 7", String.Join(" ", listOfElements));

            listOfElements.Clear();
            var positionEnumerator1 = new MatrixRotation.PositionOnMatrixFramewokStyleEnumerator(matrix.RowCount, matrix.ColumnCount, 1);

            while (positionEnumerator1.MoveNext())
            {

                listOfElements.Add(matrix[positionEnumerator1.Current.Row, positionEnumerator1.Current.Column]);
            }
            Assert.AreEqual(
@"8 9 10 11 17 23 29 35 34 33 32 26 20 14", String.Join(" ", listOfElements));

            listOfElements.Clear();
            var positionEnumerator2 = new MatrixRotation.PositionOnMatrixFramewokStyleEnumerator(matrix.RowCount, matrix.ColumnCount, 2);

            while (positionEnumerator2.MoveNext())
            {

                listOfElements.Add(matrix[positionEnumerator2.Current.Row, positionEnumerator2.Current.Column]);
            }

            Assert.AreEqual(
@"15 16 22 28 27 21", String.Join(" ", listOfElements));
        }

        [TestMethod]
        public void Test00()
        {
            Test(
@"4 4 1
1   2  3  4
5   6  7  8
9  10 11 12
13 14 15 16",
@"2 3 4 8
1 7 11 12
5 6 10 16
9 13 14 15");
        }
        [TestMethod]
        public void Test01()
        {
            Test(
@"4 4 2
1 2 3 4
5 6 7 8
9 10 11 12
13 14 15 16",
@"3 4 8 12
2 11 10 16
1 7 6 15
5 9 13 14");
        }

        [TestMethod]
        public void Test02()
        {
            Test(
@"5 4 7
1 2 3 4
7 8 9 10
13 14 15 16
19 20 21 22
25 26 27 28",
@"28 27 26 25
22 9 15 19
16 8 21 13
10 14 20 7
4 3 2 1");
        }

        [TestMethod]
        public void Test03()
        {
            Test(
@"2 2 3
1 1
1 1",
@"1 1
1 1");
        }

        [TestMethod]
        public void Test04()
        {
            Test(
@"4 8 3
  1  2  3  4  5  6  7  8
  9 10 11 12 13 14 15 16
 17 18 19 20 21 22 23 24
 25 26 27 28 29 30 31 32",
@"4 5 6 7 8 16 24 32
3 13 14 15 23 22 21 31
2 12 11 10 18 19 20 30
1 9 17 25 26 27 28 29");
        }
        private void Test(string input, string expectedOutput)
        {
            var reader = new StringReader(input);
            var writer = new StringWriter();
            writer.NewLine = "\n";
            MatrixRotation.Problem.Solve(reader, writer);
            var actualOutput = writer.ToString().Trim();

            Assert.AreEqual(expectedOutput, actualOutput);
        }

        [TestMethod]
        public void BigMatrixTest()
        {

            const int m = 300;
            const int n = 300;
            const int r = 1000000000;
            File.Delete("biginput.txt");
            using (var writer = File.CreateText("biginput.txt"))
            {
                writer.WriteLine("{0} {1} {2}", m, n, r);

                int element = 100000000 - m * n - 1;
                for (int row = 0; row < m; row++)
                {
                    writer.Write(element);
                    element++;
                    for (int column = 1; column < n; column++)
                    {
                        writer.Write(" " + element);
                        element++;
                    }

                    writer.WriteLine();
                }
            }

            using (var reader = File.OpenText("biginput.txt"))
            {
                File.Delete("bigoutput.txt");
                using (var writer = File.CreateText("bigoutput.txt"))
                {
                    MatrixRotation.Problem.Solve(reader, writer);
                }
            }
        }
    }
}
