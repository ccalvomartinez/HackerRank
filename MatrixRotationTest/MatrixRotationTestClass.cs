using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;

namespace MatrixRotationTest
{
    [TestClass]
    public class MatrixRotationTestClass
    {


        [TestMethod]
        public void PositionOnMatrixFramewokStyleEnumeratorTestNoFrameIndex()
        {
            PositionOnMatrixFramewokStyleEnumeratorTest(7, 6,
            @"1 2 3 4 5 6 12 18 24 30 36 42 41 40 39 38 37 31 25 19 13 7 8 9 10 11 17 23 29 35 34 33 32 26 20 14 15 16 22 28 27 21");
        }

        [TestMethod]
        public void PositionOnMatrixFramewokStyleEnumeratorTestFrameIndexSet0()
        {
            PositionOnMatrixFramewokStyleEnumeratorTest(7, 6,
            @"1 2 3 4 5 6 12 18 24 30 36 42 41 40 39 38 37 31 25 19 13 7", 0);
        }

        [TestMethod]
        public void PositionOnMatrixFramewokStyleEnumeratorTestFrameIndexSet1()
        {
            PositionOnMatrixFramewokStyleEnumeratorTest(7, 6,
            @"8 9 10 11 17 23 29 35 34 33 32 26 20 14", 1);
        }

        [TestMethod]
        public void PositionOnMatrixFramewokStyleEnumeratorTestFrameIndexSet2()
        {
            PositionOnMatrixFramewokStyleEnumeratorTest(7, 6,
            @"15 16 22 28 27 21", 2);
        }

        public void PositionOnMatrixFramewokStyleEnumeratorTest(int rowCount, int columnCount, string expectedOutput, int? frameIndex = null)
        {
            var matrix = new MatrixRotation.Matrix<int>(rowCount, columnCount);
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
            var positionEnumerator = new MatrixRotation.PositionOnMatrixFramewokStyleEnumerator(matrix.RowCount, matrix.ColumnCount, frameIndex);

            while (positionEnumerator.MoveNext())
            {
                listOfElements.Add(matrix[positionEnumerator.Current.Row, positionEnumerator.Current.Column]);
            }

            Assert.AreEqual(expectedOutput, string.Join(" ", listOfElements));
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

        private static void Test(string input, string expectedOutput)
        {
            var reader = new StringReader(input);
            var writer = new StringWriter();

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

        [TestMethod]
        public void TestGetValueFromPositionOnMatrix0()
        {
            FrameGetIndexFromPositionOnMatrixTest(4, 5);
        }

        [TestMethod]
        public void TestGetValueFromPositionOnMatrix1()
        {
            FrameGetIndexFromPositionOnMatrixTest(7, 8);
        }

        public void FrameGetIndexFromPositionOnMatrixTest(int rowcount, int columncount)
        {
            var matrix = new MatrixRotation.Matrix<int>(rowcount, columncount);

            for (int r = 0; r < matrix.RowCount; r++)
            {
                for (int c = 0; c < matrix.ColumnCount; c++)
                {
                    matrix[r, c] = r * 10 + c;
                }
            }

            int frameCount = Math.Min(matrix.RowCount, matrix.ColumnCount) / 2;

            for (int frameIndex = 0; frameIndex < frameCount; frameIndex++)
            {
                var frame = MatrixRotation.Frame<int>.FromMatrix(matrix, frameIndex, 0);
                var enumerable = new MatrixRotation.PositionOnMatrixFrameStyleEnumerable(matrix.RowCount, matrix.ColumnCount, frameIndex);
                var i = 0;

                foreach (MatrixRotation.PositionOnMatrix pos in enumerable)
                {
                    Assert.AreEqual(i, frame.GetIndexFromPositionOnMatrix(pos));
                    i++;
                }

            }
        }
    }
}
