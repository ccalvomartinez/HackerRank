using MatrixRotation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MatrixRotationTest
{
    
    
    /// <summary>
    ///Se trata de una clase de prueba para FrameTest y se pretende que
    ///contenga todas las pruebas unitarias FrameTest.
    ///</summary>
    [TestClass()]
    public class FrameTest
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
        ///Una prueba de GetIndexFromPositionOnMatrix
        ///</summary>
        public void GetIndexFromPositionOnMatrixTestHelper<T>(int rowCount, int columnCount)
        {

            var matrix = new MatrixRotation.Matrix<int>(rowCount, columnCount);
          
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
                var frame= Frame<int>.FromMatrix(matrix, frameIndex, 0);
                var frameAccesor =new PrivateObject( frame );
                var enumerable = new MatrixRotation.PositionOnMatrixFrameStyleEnumerable(matrix.RowCount, matrix.ColumnCount, frameIndex);
                var i = 0;

                foreach (MatrixRotation.PositionOnMatrix pos in enumerable)
                {
                    Assert.AreEqual(i,frameAccesor.Invoke("GetIndexFromPositionOnMatrix",pos));
                    i++;
                }

            }
        }

        [TestMethod()]
        public void GetIndexFromPositionOnMatrixTest()
        {
            GetIndexFromPositionOnMatrixTestHelper<int>(4,5);
        }

        /// <summary>
        ///Una prueba de GetPositionOnMatrixFromIndex
        ///</summary>
        public void GetPositionOnMatrixFromIndexTestHelper<T>(int rowCount, int columnCount)
        {
            var matrix = new MatrixRotation.Matrix<int>(rowCount, columnCount);

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
                var frame = Frame<int>.FromMatrix(matrix, frameIndex, 0);
                var frameAccesor = new PrivateObject(frame);
                var enumerable = new MatrixRotation.PositionOnMatrixFrameStyleEnumerable(matrix.RowCount, matrix.ColumnCount, frameIndex);
                var i = 0;

                foreach (MatrixRotation.PositionOnMatrix pos in enumerable)
                {
                    MatrixRotation.PositionOnMatrix newPos=(MatrixRotation.PositionOnMatrix)frameAccesor.Invoke("GetPositionOnMatrixFromIndex", i);
                    Assert.AreEqual(pos.Row,  newPos.Row, String.Format("Row.FrameIndex={1} Index={0}", i, frameIndex));
                    Assert.AreEqual(pos.Column, newPos.Column, String.Format("Column.FrameIndex={1}  Index={0}", i, frameIndex));
                    i++;
                }

            }
        }

        [TestMethod()]
        public void GetPositionOnMatrixFromIndexTest()
        {
            GetPositionOnMatrixFromIndexTestHelper<GenericParameterHelper>(4,5);
        }

       
    }
}
