using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GraphBreadFirst;
using System.IO;
using System.Linq;
using System.Collections.Generic;

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
            var outputNodes = new List<string>(2) { "1 2 3 4", "1 2 3" };
            var outputEdges = new List<string>(2) { "2 3|1|1|", "|3|2" };
            var reader = new StringReader(input);
            var gi = GraphInput.FromReader(reader);
            var actualOutputNodes = new List<string>(2);
            var actualOutputEdges = new List<string>(2);

            foreach (Graph<int> graph in gi.Graphs)
            {
                actualOutputNodes.Add(String.Join(" ", graph.Nodes.OrderBy(node => node.Data).Select(node => node.Data)));
                actualOutputEdges.Add(String.Join("|", graph.Nodes.OrderBy(node => node.Data).Select(node => String.Join(" ", node.Neighboors.Select(nnode => nnode.Node.Data)))));
            }

            for (int i = 0; i < actualOutputNodes.Count; i++)
            {
                Assert.AreEqual(outputNodes[i], actualOutputNodes[i]);
            }
            for (int i = 0; i < actualOutputEdges.Count; i++)
            {
                Assert.AreEqual(outputEdges[i], actualOutputEdges[i]);
            }
        }

        [TestMethod]
        public void GraphInputTestLarge()
        {
            Console.SetIn(new System.IO.StreamReader("D:\\Documentos\\Downloads\\input011.txt"));
            var gi = GraphInput.FromReader(Console.In);
        }
        [TestMethod]
        public void GraphInputTestVeryLarge()
        {
            Console.SetIn(new System.IO.StreamReader("D:\\Documentos\\Downloads\\input015.txt"));
            var gi = GraphInput.FromReader(Console.In);
        }
        [TestMethod]
        public void MininalDistanceTest()
        {
            var input = @"2
7 7
1 2
1 4
2 5
2 3
4 5
3 5
6 7
1
7 7
1 2
1 4
2 5
2 3
4 5
3 5
6 7
6";
            var outputs = new List<string>(2) { "6 12 6 12 -1 -1", "-1 -1 -1 -1 -1 6" };
            var actualOutputs = new List<string>();
            var reader = new StringReader(input);
            var gi = GraphInput.FromReader(reader);
            int t = 0;

            foreach (Graph<int> graph in gi.Graphs)
            {
                var result = graph.MinimalDistances(gi.StartNode[t]);
                t += 1;
                actualOutputs.Add(String.Join(" ", result.OrderBy(kvp => kvp.Key.Data).Select(kvp => double.IsPositiveInfinity(kvp.Value) ? -1 : kvp.Value)));
            }

            for (int i = 0; i < actualOutputs.Count; i++)
            {
                Assert.AreEqual(outputs[i], actualOutputs[i]);
            }
        }

        [TestMethod]
        public void MininalDistanceTestLarge()
        {
            Console.SetIn(new System.IO.StreamReader("D:\\Documentos\\Downloads\\input011.txt"));
            var gi = GraphInput.FromReader(Console.In);
            var output = @"6 6 6 6 12 6 12 6 12 12 6 6 6 6 6 12 12 6 6 6 6 12 6 12 6 12 6 12 12 12 12 6 12 12 6 12 12 6 12 6 12 6 12 12 6 6 12 6 6 6 6 12 12 12 12 6 6 6 12 6 6 12 12 12 12 12 12 6 6";
            int t = 0;

            foreach (Graph<int> graph in gi.Graphs)
            {
                var result = graph.MinimalDistances(gi.StartNode[t]);
                t += 1;
                Assert.AreEqual(output, String.Join(" ", result.OrderBy(kvp => kvp.Key.Data).Select(kvp => double.IsPositiveInfinity(kvp.Value) ? -1 : kvp.Value)));
            }
        }
        [TestMethod]
        public void MininalDistanceTestVeryLarge()
        {
            Console.SetIn(new System.IO.StreamReader("D:\\Documentos\\Downloads\\input015.txt"));
            var gi = GraphInput.FromReader(Console.In);
            var reader = new System.IO.StreamReader("D:\\Documentos\\Downloads\\output015.txt");
            int t = 0;

            foreach (Graph<int> graph in gi.Graphs)
            {
                var result = graph.MinimalDistances(gi.StartNode[t]);
                t += 1;
                Assert.AreEqual(reader.ReadLine(), String.Join(" ", result.OrderBy(kvp => kvp.Key.Data).Select(kvp => double.IsPositiveInfinity(kvp.Value) ? -1 : kvp.Value)));
            }
        }
    }

    [TestClass]
    public class NodeTestClass {
        [TestMethod]
        public void AddNeighboorTest() { 
            Node<int> node = new Node<int>(3);
            Node<int> neightboorNode = new Node<int>(2);
            var weight = 3;
            node.AddNeighboor(neightboorNode,weight);
            Assert.IsNotNull(node.Neighboors);
            Assert.AreEqual(1, node.Neighboors.Count());
            Assert.AreEqual(neightboorNode, node.Neighboors.First().Node);
            Assert.AreEqual(weight, node.Neighboors.First().Weight);
        }
        [TestMethod]
        public void EqualsTest() {
            Node<int> node1 = new Node<int>(1);
            Node<int> node2=new Node<int>(1);
            Assert.IsTrue(node1.Equals(node2));
            Assert.IsFalse(node1 == node2);
        }
        [TestMethod]
        public void NotEqualsTest()
        {
            Node<int> node1 = new Node<int>(1);
            Node<int> node2 = new Node<int>(2);
            Assert.IsFalse(node1.Equals(node2));
            Assert.IsFalse(node1 == node2);
        }
        [TestMethod]
        public void CompareToDistinctNodesTest() {
            Node<int> node1 = new Node<int>(1);
            Node<int> node2 = new Node<int>(2);
            Assert.IsTrue(node2.CompareTo(node1) > 0);
            Assert.IsTrue(node1.CompareTo(node2) < 0);
        }
        [TestMethod]
        public void CompareToEqualNodesTest()
        {
            Node<int> node1 = new Node<int>(1);
            Node<int> node2 = new Node<int>(1);
            Assert.IsTrue(node2.CompareTo(node1) == 0);
          
        }
        [TestMethod]
        public void ToStringTest() {
            Node<int> node1 = new Node<int>(1);
            Assert.AreEqual("1", node1.ToString());        
        }
    }

    [TestClass]
    public class GraphTestClass {
        [TestMethod]
        public void AddNodeDataTest() {
            Graph<int> graph = new Graph<int>();
            graph.AddNode(3);
            Assert.AreEqual(1, graph.Nodes.Count);
            Assert.AreEqual(3, graph.Nodes.First().Data);
        }
        [TestMethod]
        public void AddNodeTest()
        {
            Graph<int> graph = new Graph<int>();
            Node<int> node = new Node<int>(3);
            graph.AddNode(node);
            Assert.AreEqual(1, graph.Nodes.Count);
            Assert.AreEqual(node, graph.Nodes.First());
        }
        [TestMethod]
        public void GetNodeByDataTest() {
            Graph<int> graph = new Graph<int>();
            graph.AddNode(1);
            graph.AddNode(2);
            graph.AddNode(5);
        
            Assert.AreEqual(5, graph.GetNodeByData(5).Data);
        }
        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void GetNodeByDataFailingTest()
        {
            Graph<int> graph = new Graph<int>();
            graph.AddNode(1);
            graph.AddNode(2);
            graph.AddNode(5);

            Assert.AreEqual(3, graph.GetNodeByData(3).Data);
        }
    }
}
