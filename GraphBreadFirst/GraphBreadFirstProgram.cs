using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GraphBreadFirst
{
    class Solution
    {
        internal static void Main(string[] args)
        {
            Console.SetIn(new System.IO.StreamReader("D:\\Documentos\\Downloads\\input011.txt"));
            var gi = GraphInput.FromReader(Console.In);
            int t = 0;

            foreach (Graph<int> graph in gi.Graphs)
            {
                var result = graph.MinimalDistances(gi.StartNode[t]);
                t += 1;
                Console.WriteLine(String.Join(" ", result.OrderBy(kvp => kvp.Key.Data).Select(kvp => double.IsPositiveInfinity(kvp.Value) ? -1 : kvp.Value)));

            }

        }
    }
    public class GraphInput
    {
        public int TestCount { get; private set; }
        public Graph<int>[] Graphs { get; private set; }
        public Node<int>[] StartNode { get; private set; }

        private GraphInput(int tCount, Graph<int>[] graphs, Node<int>[] startNode)
        {
            Graphs = graphs;
            TestCount = tCount;
            StartNode = startNode;
        }

        public static GraphInput FromReader(TextReader input)
        {
            var allInput = input.ReadToEnd();
            var lines = allInput.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            int testCount;
            testCount = Convert.ToInt32(lines[0]);
            Node<int>[] startNodes = new Node<int>[testCount];
            Graph<int>[] graphs = new Graph<int>[testCount];
            int lineCount = 1;

            for (int t = 0; t < testCount; t++)
            {
                graphs[t] = new Graph<int>();
                string[] graphSizes = lines[lineCount].Split();
                lineCount++;
                int numberOfNodes = Convert.ToInt32(graphSizes[0]);
                int numberOfEdges = Convert.ToInt32(graphSizes[1]);

                for (int n = 1; n <= numberOfNodes; n++)
                {
                    graphs[t].AddNode(n);
                }


                foreach (Tuple<int, int> edge in OrderAndEliminateDupliacatesOnEdges(lines, lineCount, numberOfEdges))
                {
                    graphs[t].AddEdge(edge.Item1, edge.Item2, 6, Direction.Both);
                }


                lineCount = lineCount + numberOfEdges;
                int startNodeData = Convert.ToInt32(lines[lineCount]);
                lineCount++;
                startNodes[t] = graphs[t].GetNodeByData(startNodeData);

            }

            return new GraphInput(testCount, graphs, startNodes);
        }

        private static List<Tuple<int, int>> OrderAndEliminateDupliacatesOnEdges(string[] lines, int initialLine, int numberOfEdges)
        {
            List<Tuple<int, int>> OrderedListOfEdges = new List<Tuple<int, int>>();
            for (int e = 0; e < numberOfEdges; e++)
            {
                var edgeString = lines[initialLine + e].Split();
                var node1 = Convert.ToInt32(edgeString[0]);
                var node2 = Convert.ToInt32(edgeString[1]);
                if (node1 > node2)
                {
                    OrderedListOfEdges.Add(new Tuple<int, int>(node2, node1));
                }
                else
                {
                    OrderedListOfEdges.Add(new Tuple<int, int>(node1, node2));
                }
            }
            return OrderedListOfEdges.Distinct().OrderBy(tup => tup.Item1).ThenBy(tup => tup.Item2).ToList();
        }
    }

    public class Graph<T> where T : IComparable<T>
    {
        private List<Node<T>> _nodes;
    
        public IReadOnlyCollection<Node<T>> Nodes { get { return (IReadOnlyCollection<Node<T>>)_nodes; } }
        public Graph()
        {
            _nodes = new List<Node<T>>();
        }

        public Node<T> AddNode(T data)
        {
            var newNode = new Node<T>(data);

            if (_nodes.Contains(newNode))
            {
                throw new ArgumentException("The graph already contains this node");
            }

            _nodes.Add(newNode);
            return newNode;
        }

        public Node<T> AddNode(Node<T> node)
        {
            if (_nodes.Contains(node))
            {
                throw new ArgumentException("The graph already contains this node");
            }

            _nodes.Add(node);
            return node;
        }
       
        public void AddEdge(T startNodeData, T endNodeData, double weight, Direction dir)
        {
            var startNode = GetNodeByData(startNodeData);
            var endNode = GetNodeByData(endNodeData);

            startNode.AddNeighboor(endNode,weight);
            endNode.AddNeighboor(startNode,weight);
       
        }
       
        public Node<T> GetNodeByData(T data)
        {
            Node<T> searchedNode = new Node<T>(data);
            int indexOfNode = _nodes.IndexOf(searchedNode);
            if (_nodes.Contains(searchedNode))
            {
                return _nodes[indexOfNode];
            }
            throw new ArgumentException(String.Format("Node with data {0} is not in the graph", data));
        }

        public Dictionary<Node<T>, double> MinimalDistances(Node<T> startNode)
        {
            var minimalDistances = new Dictionary<Node<T>, double>();
            var revisedNodes = new List<Node<T>>(_nodes.Count - 1);

            InitializeVariables(startNode, minimalDistances);
         
            revisedNodes.Add(startNode);
            while (revisedNodes.Count < _nodes.Count)
            {
                var nextNode = minimalDistances.Where(kvp => !revisedNodes.Contains(kvp.Key)).OrderBy(kvp => kvp.Value).First().Key;
                revisedNodes.Add(nextNode);

                foreach (Neighboor<T> neighboor in nextNode.Neighboors.Where(nb=>  !revisedNodes.Contains(nb.Node)))
                {
                    if (minimalDistances[neighboor.Node] > minimalDistances[nextNode] + neighboor.Weight)
                    {
                        minimalDistances[neighboor.Node] = minimalDistances[nextNode] + neighboor.Weight;
                    }
                }
            }
            return minimalDistances;
        }

        private void InitializeVariables(Node<T> startNode, Dictionary<Node<T>, double> minimalDistances)
        {
            var restOfNodes = _nodes.Except(new Node<T>[] { startNode });

            foreach (Node<T> node in restOfNodes)
            {
                minimalDistances.Add(node, double.PositiveInfinity);
            }

            foreach (Neighboor<T> neighboor in startNode.Neighboors)
            {
                minimalDistances[neighboor.Node] = neighboor.Weight;
            }
        }
    }

    public class Node<T> : IComparable<Node<T>> where T : IComparable<T>
    {
        public T Data { get; private set; }

        private List<Neighboor<T>> _neighboors;
        public IReadOnlyCollection<Neighboor<T>> Neighboors { get { return (IReadOnlyCollection<Neighboor<T>>)_neighboors; } }

        public Node(T data)
        {
            Data = data;
            _neighboors = new List<Neighboor<T>>();
        }
        public void AddNeighboor(Node<T> node,double weight) {
            Neighboor<T> newNeighboor= new Neighboor<T>(node,weight);
           
            if (!_neighboors.Contains(newNeighboor))
            {
                _neighboors.Add(newNeighboor);
            }
        }
        //Overrides
        public override bool Equals(object obj)
        {

            if (obj == null || GetType() != obj.GetType()) return false;
            var other = (Node<T>)obj;
            return Data.Equals(other.Data);
        }

        public override int GetHashCode()
        {
            return Data.GetHashCode();
        }

        public int CompareTo(Node<T> other)
        {
            return this.Data.CompareTo(other.Data);
        }

        public override string ToString()
        {
            return Data.ToString();
        }
    }

    public struct Neighboor<T> where T:IComparable<T> {
        public Node<T> Node;
        public double Weight;
        public Neighboor(Node<T> node,double weight){
            Node = node;
            Weight = weight;
        }
    }
   
    [Flags]
    public enum Direction
    {
        StartToEnd = 1,
        EndToStart = 2,
        Both = StartToEnd | EndToStart

    }
}
