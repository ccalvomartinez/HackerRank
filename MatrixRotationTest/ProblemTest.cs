using MatrixRotation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace MatrixRotationTest
{
    
    
    /// <summary>
    ///Se trata de una clase de prueba para ProblemTest y se pretende que
    ///contenga todas las pruebas unitarias ProblemTest.
    ///</summary>
    [TestClass()]
    public class ProblemTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Obtiene o establece el contexto de la prueba que proporciona
        ///la información y funcionalidad para la ejecución de pruebas actual.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Atributos de prueba adicionales
        // 
        //Puede utilizar los siguientes atributos adicionales mientras escribe sus pruebas:
        //
        //Use ClassInitialize para ejecutar código antes de ejecutar la primera prueba en la clase 
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup para ejecutar código después de haber ejecutado todas las pruebas en una clase
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize para ejecutar código antes de ejecutar cada prueba
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup para ejecutar código después de que se hayan ejecutado todas las pruebas
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///Una prueba de Solve
        ///</summary>
        [TestMethod()]
        public void SolveTest00()
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
        [TestMethod]
        public void SolveTest01()
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
        public void SolveTest02()
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
        public void SolveTest03()
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
        public void SolveTest04()
        {
            Test(
@"2 2 3
1 1
1 1",
@"1 1
1 1");
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
    }

}
