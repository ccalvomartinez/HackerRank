using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GraphBreadFirst;
using System.IO;

namespace GraphBreadFirstTest
{
    [TestClass]
    public class GraphBreadFirstTestClass
    {
        [TestMethod]
        public void GraphInputTest()
        {
            var input = @"2
4 2
1 2
1 3
1
3 1
2 3
2";
               var reader = new StringReader(input);
               var gi = GraphInput.FromReader(reader);
               Assert.IsTrue(true);
        }
    }
}
