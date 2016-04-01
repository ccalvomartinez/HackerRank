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


                foreach (Tuple<int, int> edge in PretratamientoEdges(lines, lineCount, numberOfEdges))
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

        private static List<Tuple<int, int>> PretratamientoEdges(string[] lines, int initialLine, int numberOfEdges)
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
            return OrderedListOfEdges.OrderBy(tup => tup.Item1).ThenBy(tup => tup.Item2).ToList();
        }
    }

    public class Graph<T> where T : IComparable<T>
    {
        private List<Node<T>> _nodes;
        private List<List<Node<T>>> _neighboorGroups;

        public IReadOnlyCollection<Node<T>> Nodes { get { return (IReadOnlyCollection<Node<T>>)_nodes; } }
        public Graph()
        {
            _nodes = new List<Node<T>>();
            _neighboorGroups = new List<List<Node<T>>>();
        }

        public Node<T> AddNode(T data)
        {
            var newNode = new Node<T>(data);

            if (_nodes.Contains(newNode))
            {
                throw new ArgumentException("The graph already contains this node");
            }

            _nodes.Add(newNode);
            _neighboorGroups.Add(new List<Node<T>>() { newNode });
            return newNode;
        }

        public Node<T> AddNode(Node<T> node)
        {

            if (_nodes.Contains(node))
            {
                throw new ArgumentException("The graph already contains this node");
            }

            _nodes.Add(node);
            _neighboorGroups.Add(new List<Node<T>>() { node });
            return node;
        }

        public void AddEdge(T startNodeData, T endNodeData, double weight, Direction dir)
        {
            var startNode = GetNodeByData(startNodeData);
            var endNode = GetNodeByData(endNodeData);

            startNode.AddNeighboor(endNode);
            endNode.AddNeighboor(startNode);
            AddToNeighboorGroups(startNode, endNode);
        }

        private void AddToNeighboorGroups(Node<T> startNode, Node<T> endNode)
        {
            List<Node<T>> startNeighboorGroup = _neighboorGroups.Find(delegate(List<Node<T>> list) { return list.Contains(startNode); });
            List<Node<T>> endNeighboorGroup = _neighboorGroups.Find(delegate(List<Node<T>> list) { return list.Contains(endNode); });

            if (startNeighboorGroup != endNeighboorGroup)
            {
                startNeighboorGroup.AddRange(endNeighboorGroup);
                startNeighboorGroup.Sort();
                _neighboorGroups.Remove(endNeighboorGroup);
            }
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

     

        private IReadOnlyList<Node<T>> GetNeighbourGroup(Node<T> node)
        {
            var result = new List<Node<T>>();

            var neighboorGroup = _neighboorGroups.Find(delegate(List<Node<T>> list) { return list.Contains(node); });



            return result;
        }

        public SortedDictionary<Node<T>, double> MinimalDistances(Node<T> startNode)
        {


            var minimalDistances = new SortedDictionary<Node<T>, double>();

            //var restOfNodes = _nodes.Except(new Node<T>[] { startNode });

            //foreach (Node<T> node in restOfNodes)
            //{
            //    var edge = GetEdgeByNodes(startNode, node, Direction.Both);

            //    if (edge == null)
            //    {
            //        minimalDistances.Add(node, double.PositiveInfinity);
            //    }
            //    else
            //    {
            //        minimalDistances.Add(node, edge.Weight);
            //    }
            //}


            //var revisedNodes = new List<Node<T>>(_nodes.Count - 1);

            //var neighboorGroup = _neighboorGroups.Find(delegate(List<Node<T>> list) { return list.Contains(startNode); });
            //var notNeighboorGroups = _neighboorGroups.Except(new List<Node<T>>[] { neighboorGroup });

            //revisedNodes.AddRange(notNeighboorGroups.SelectMany(list => list));
            //revisedNodes.Add(startNode);
            //while (revisedNodes.Count < _nodes.Count)
            //{
            //    var nextNode = minimalDistances.Where(kvp => !revisedNodes.Contains(kvp.Key)).Aggregate((kvp1, kvp2) => kvp1.Value < kvp2.Value ? kvp1 : kvp2).Key;
            //    revisedNodes.Add(nextNode);

            //    foreach (Node<T> neighboorNode in GetNeighbourGroup(nextNode).Except(revisedNodes))
            //    {

            //        if (minimalDistances[neighboorNode] > minimalDistances[nextNode] + GetEdgeByNodes(nextNode, neighboorNode, Direction.Both).Weight)
            //        {
            //            minimalDistances[neighboorNode] = minimalDistances[nextNode] + GetEdgeByNodes(nextNode, neighboorNode, Direction.Both).Weight;
            //        }
            //    }
            //}
            return minimalDistances;
        }
    }

    public class Node<T> : IComparable<Node<T>> where T : IComparable<T>
    {
        public T Data { get; private set; }

        public List<Node<T>> _neighboors;
        public IReadOnlyCollection<Node<T>> NeighboorsNodes { get { return (IReadOnlyCollection<Node<T>>) _neighboors; } }

        public Node(T data)
        {
            Data = data;
            _neighboors = new List<Node<T>>();
        }
        public void AddNeighboor(Node<T> node) {
            if (!_neighboors.Contains(node))
            {
                _neighboors.Add(node);
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

    public class Edge<T> : IComparable<Edge<T>> where T : IComparable<T>
    {
        public Node<T> StartNode { get; private set; }
        public Node<T> EndNode { get; private set; }
        public Double Weight { get; private set; }
        public Direction Direction { get; private set; }

        public Edge(Node<T> start, Node<T> end, double weight, Direction dir)
        {

            StartNode = start;
            EndNode = end;
            Weight = weight;
            Direction = dir;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            var other = (Edge<T>)obj;
            if (Direction == other.Direction && Direction == Direction.Both)
            {
                return (StartNode.Equals(other.StartNode) && EndNode.Equals(other.EndNode)) ||
                    (StartNode.Equals(other.EndNode) && EndNode.Equals(other.StartNode));
            }
            return StartNode.Equals(other.StartNode) && EndNode.Equals(other.EndNode) && Direction == other.Direction;
        }

        public override int GetHashCode()
        {
            return StartNode.Data.GetHashCode() + EndNode.Data.GetHashCode();
        }

        public int CompareTo(Edge<T> other)
        {
            if (this.Equals(other))
            {
                return 0;
            }
            Node<T> thisStartNode, thisEndNode, otherStartNode, otherEndNode;

            thisStartNode = StartNode;
            thisEndNode = EndNode;
            otherStartNode = other.StartNode;
            otherEndNode = other.EndNode;

            if (Direction == Direction.Both)
            {
                if (StartNode.CompareTo(EndNode) > 0)
                {
                    thisStartNode = EndNode;
                    thisEndNode = StartNode;
                }
            }


            if (other.Direction == Direction.Both)
            {
                if (other.StartNode.CompareTo(other.EndNode) > 0)
                {
                    otherStartNode = other.EndNode;
                    otherEndNode = other.StartNode;
                }
            }

            if (thisStartNode.CompareTo(otherStartNode) != 0)
            {
                return thisStartNode.CompareTo(otherStartNode);
            }
            if (thisEndNode.CompareTo(otherEndNode) != 0)
            {
                return thisEndNode.CompareTo(otherEndNode);
            }
            return Direction.CompareTo(other.Direction);
        }

        public override string ToString()
        {
            return StartNode.ToString() + " " + EndNode.ToString();
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
