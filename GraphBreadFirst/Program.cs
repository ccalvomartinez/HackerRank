using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphBreadFirst
{
    class Solution
    {
        static void Main(string[] args)
        {
        }
    }
    public class GraphInput
    {
        public int TestCount { get; private set; }
        public Graph<int>[] Graphs { get; private set; }
        public Node<int>[] StartNode { get; private set; }
        private GraphInput(int tCount, List<int>[] nodes, int[][][] edges, int[] startNode)
        {
            Graphs = new Graph<int>[tCount];
            TestCount = tCount;
            StartNode = new Node<int>[tCount];

            for (int t = 0; t < TestCount; t++)
            {
                var graph = new Graph<int>();

                foreach (int nodeName in nodes[t])
                {
                    graph.AddNode(nodeName);
                }
                for (int r = 0; r < edges[t].Length; r++)
                {
                    var starNode = graph.GetNodeByData(edges[t][r][0]);
                    if (starNode == null)
                    {
                        throw new ArgumentException("edges contains nodes not present in nodes");
                    }
                    var endNode = graph.GetNodeByData(edges[t][r][1]);
                    if (endNode == null)
                    {
                        throw new ArgumentException("edges contains nodes not present in nodes");
                    }
                    graph.AddEdge(starNode, endNode, 6, Direcction.Both);
                }
                Graphs[t] = graph;
            }

        }
        public static GraphInput FromReader(TextReader input)
        {
            var allInput = input.ReadToEnd();
            var lines = allInput.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            int testCount;
            testCount = Convert.ToInt32(lines[0]);
            List<int>[] nodes = new List<int>[testCount];
            int[][][] edges = new int[testCount][][];
            int lineCount = 1;
            for (int t = 0; t < testCount; t++)
            {
                string[] graphSizes = lines[lineCount].Split();
                lineCount++;
                nodes[t] = new List<int>();
                edges[t] = new int[Convert.ToInt32(graphSizes[1])][];

                for (int e = 0; e < edges[t].Length; e++)
                {
                    var nodesString = lines[lineCount].Split();
                    lineCount++;
                    edges[t][e] = new int[2];
                    edges[t][e][0] = Convert.ToInt32(nodesString[0]);
                    edges[t][e][1] = Convert.ToInt32(nodesString[1]);
                    if (!nodes[t].Contains(edges[t][e][0]))
                    {
                        nodes[t].Add(edges[t][e][0]);
                    }
                    if (!nodes[t].Contains(edges[t][e][1]))
                    {
                        nodes[t].Add(edges[t][e][1]);
                    }
                }

            }

            return new GraphInput(testCount, nodes, edges);
        }
    }
    public class Graph<T>
    {
        public List<Node<T>> Nodes { get; private set; }
        public List<Edge<T>> Edges { get; private set; }
        public Graph()
        {
            Nodes = new List<Node<T>>();
            Edges = new List<Edge<T>>();
        }
        public Node<T> AddNode(T data)
        {
            var newNode = new Node<T>(data);
            if (Nodes.Contains(newNode))
            {
                throw new ArgumentException("The graph already contains this node");
            }
            Nodes.Add(newNode);
            return newNode;
        }
        public Edge<T> AddEdge(Node<T> start, Node<T> end, double weight, Direcction dir)
        {
            if (!Nodes.Contains(start))
            {
                throw new ArgumentException("Start node is not in the graph");
            }
            if (!Nodes.Contains(end))
            {
                throw new ArgumentException("End node is not in the graph");
            }
            var newEdge = new Edge<T>(start, end, weight, dir);
            Edges.Add(newEdge);
            return newEdge;
        }
        public Node<T> GetNodeByData(T data)
        {
            return Nodes.SingleOrDefault(nod => nod.Data.Equals(data));
        }
    }
    public class Node<T>
    {
        public T Data { get; private set; }
        public Node(T data)
        {
            Data = data;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            var other = (Node<T>)obj;
            return Data.Equals(other.Data);
        }
        public override int GetHashCode()
        {
            return Data.GetHashCode();
        }
    }
    public class Edge<T>
    {
        public Node<T> StartNode { get; private set; }
        public Node<T> EndNode { get; private set; }
        public Double Weight { get; private set; }
        public Direcction Direcction { get; private set; }
        public Edge(Node<T> start, Node<T> end, double weight, Direcction dir)
        {

            StartNode = start;
            EndNode = end;
            Weight = weight;
            Direcction = dir;
        }
    }
    public enum Direcction
    {
        StartToEnd,
        EndToStart,
        Both

    }
}
