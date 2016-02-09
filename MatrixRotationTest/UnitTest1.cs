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
        public void MatrixFrameworkStyleEnumeratorTest()
        {
            var matrix = new MatrixRotation.Matrix<int>(7, 6);
            var i = 1;
            for (int r = 0; r < matrix.RowCount; r++) {
                for (int c = 0; c < matrix.ColumnCount; c++) {
                    matrix[r, c] = i;
                    i += 1;
                }
            }

          

            var listOfElements = new List<int>();

            var matrixEnumerator = matrix.GetFrameworkStyleEnumerator();

            while (matrixEnumerator.MoveNext()) {

                listOfElements.Add(((IEnumerator<int>)matrixEnumerator).Current);
            }

            Assert.AreEqual(
@"1 2 3 4 5 6 12 18 24 30 36 42 41 40 39 38 37 31 25 19 13 7 8 9 10 11 17 23 29 35 34 33 32 26 20 14 15 16 22 28 27 21",String.Join(" ",listOfElements));
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

            var matrixEnumerator0 = matrix.GetFrameworkStyleEnumerator(0);

            while (matrixEnumerator0.MoveNext())
            {

                listOfElements.Add(((IEnumerator<int>)matrixEnumerator0).Current);
            }

            Assert.AreEqual(
@"1 2 3 4 5 6 12 18 24 30 36 42 41 40 39 38 37 31 25 19 13 7", String.Join(" ", listOfElements));

            listOfElements.Clear();
            var matrixEnumerator1 = matrix.GetFrameworkStyleEnumerator(1);

            while (matrixEnumerator1.MoveNext())
            {

                listOfElements.Add(((IEnumerator<int>)matrixEnumerator1).Current);
            }

            Assert.AreEqual(
@"8 9 10 11 17 23 29 35 34 33 32 26 20 14", String.Join(" ", listOfElements));

            listOfElements.Clear();
            var matrixEnumerator2 = matrix.GetFrameworkStyleEnumerator(2);

            while (matrixEnumerator2.MoveNext())
            {

                listOfElements.Add(((IEnumerator<int>)matrixEnumerator2).Current);
            }

            Assert.AreEqual(
@"15 16 22 28 27 21", String.Join(" ", listOfElements));
        }
    }
}
