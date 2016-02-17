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
            Console.SetIn(new System.IO.StreamReader("D:\\Documentos\\Downloads\\input015.txt"));
            var gi=GraphInput.FromReader(Console.In);
            int t=0;
            
            foreach (Graph<int> graph in gi.Graphs) {
                var result=graph.MinimalDistances(gi.StartNode[t]);
                t += 1;
                Console.WriteLine(String.Join(" ", result.OrderBy(kvp => kvp.Key.Data).Select(kvp => double.IsPositiveInfinity(kvp.Value)?-1:kvp.Value)));
                
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

                    if (graphs[t].GetNodeByData(n) == null)
                    {
                        graphs[t].AddNode(n);
                    }

                }

                for (int e = 0; e < numberOfEdges; e++)
                {
                    var nodesString = lines[lineCount].Split();
                    lineCount++;
                    var startNode = graphs[t].GetNodeByData(Convert.ToInt32(nodesString[0]));
                    var endNode = graphs[t].GetNodeByData(Convert.ToInt32(nodesString[1]));
                    graphs[t].AddEdge(startNode, endNode, 6, Direction.Both);
                }

                int startNodeData = Convert.ToInt32(lines[lineCount]);
                lineCount++;
                startNodes[t] = graphs[t].GetNodeByData(startNodeData);

            }
            
            return new GraphInput(testCount, graphs, startNodes);
        }
    }

    public class Graph<T> where T : IComparable<T>
    {
        private List<Node<T>> _nodes;
        private List<Edge<T>> _edges;
        private List<List<Node<T>>> _neighboorGroups;

        public IReadOnlyCollection<Node<T>> Nodes { get { return (IReadOnlyCollection<Node<T>>)_nodes; } }
        public IReadOnlyCollection<Edge<T>> Edges { get { return (IReadOnlyCollection<Edge<T>>)_edges; } }
        public Graph()
        {
            _nodes = new List<Node<T>>();
            _edges = new List<Edge<T>>();
            _neighboorGroups =new List<List<Node<T>>>();
        }

        public Node<T> AddNode(T data)
        {
            var newNode = new Node<T>(data);

            if (_nodes.BinarySearch(newNode)>=0)
            {
                throw new ArgumentException("The graph already contains this node");
            }

            _nodes.Add(newNode);
            _neighboorGroups.Add(new List<Node<T>>() {newNode});
            return newNode;
        }

        public Node<T> AddNode(Node<T> node)
        {

            if (_nodes.BinarySearch(node)>=0)
            {
                throw new ArgumentException("The graph already contains this node");
            }

            _nodes.Add(node);
            _neighboorGroups.Add(new List<Node<T>>() {node});
            return node;
        }

        public Edge<T> AddEdge(Node<T> startNode, Node<T> endNode, double weight, Direction dir)
        {

            if (_nodes.BinarySearch(startNode) < 0)
            {
                throw new ArgumentException("Start node is not in the graph");
            }

            if (_nodes.BinarySearch(endNode) < 0)
            {
                throw new ArgumentException("End node is not in the graph");
            }
            
            var newEdge = GetEdgeByNodes(startNode, endNode,dir);
            
            if (newEdge == null)
            {
                newEdge = new Edge<T>(startNode, endNode, weight, dir);
                _edges.Add(newEdge);
            }
            
            List<Node<T>> startNeighboorGroup = _neighboorGroups.Find(delegate(List<Node<T>> list) { return list.BinarySearch(startNode)>=0; });


            List<Node<T>> endNeighboorGroup = _neighboorGroups.Find(delegate(List<Node<T>> list) { return list.BinarySearch(endNode) >= 0; });

            if (startNeighboorGroup != endNeighboorGroup) {
                startNeighboorGroup.AddRange(endNeighboorGroup);
                startNeighboorGroup.Sort();
                _neighboorGroups.Remove(endNeighboorGroup);
            }
           
            return newEdge;
        }

        public Node<T> GetNodeByData(T data)
        {
            Node<T> foundNode=new Node<T>(data);
            int indexOfNode = _nodes.BinarySearch(foundNode);
            if (indexOfNode>=0) {
                return _nodes[indexOfNode];
            }
            return null;
        }

        private Edge<T> GetEdgeByNodes(Node<T> startNode, Node<T> endNode,Direction dir)
            
        {
            Edge<T> foundEdge =new Edge<T>(startNode,endNode,0,dir);
            int indexOfEdge = _edges.BinarySearch(foundEdge);
            if (indexOfEdge >= 0) {
                return _edges[indexOfEdge];
            }
            return null;
        }

        private IReadOnlyList<Node<T>> GetNeighbourNodes(Node<T> node)
        {
            var result = new List<Node<T>>();
            
             var neighboorGroup = _neighboorGroups.Find(delegate(List<Node<T>> list) { return list.BinarySearch(node) >= 0; });

             foreach (Node<T> nNode in neighboorGroup) {
                 var nEdge = new Edge<T>(node, nNode, 0, Direction.Both);
                 var indexOfEdge = _edges.BinarySearch(nEdge);
                 if (indexOfEdge >= 0)
                 {
                     result.Add(nNode);
                 }
                 else {
                     nEdge = new Edge<T>(node, nNode, 0, Direction.StartToEnd);
                     indexOfEdge = _edges.BinarySearch(nEdge);
                     if (indexOfEdge >= 0)
                     {
                         result.Add(nNode);
                     }
                     else {
                         nEdge = new Edge<T>(nNode, node, 0, Direction.EndToStart);
                         indexOfEdge = _edges.BinarySearch(nEdge);
                         if (indexOfEdge >= 0)
                         {
                             result.Add(nNode);
                         }
                     }
                 }

             }

            return result;
        }
        public SortedDictionary<Node<T>, double> MinimalDistances(Node<T> startNode)
        {
            _nodes.Sort();
            _edges.Sort();
            
            var minimalDistances = new SortedDictionary<Node<T>, double>();
         
            var restOfNodes = _nodes.Except(new Node<T>[] { startNode });

            foreach (Node<T> node in restOfNodes)
            {
                var edge = GetEdgeByNodes(startNode, node,Direction.Both);

                if (edge == null)
                {
                    minimalDistances.Add(node, double.PositiveInfinity);
                }
                else
                {
                    minimalDistances.Add(node, edge.Weight);
                }
            }


            var revisedNodes = new List<Node<T>>(_nodes.Count - 1);
            
            var neighboorGroup = _neighboorGroups.Find(delegate(List<Node<T>> list) { return list.BinarySearch(startNode) >= 0; });
            var notNeighboorGroups=_neighboorGroups.Except(new List<Node<T>>[] {neighboorGroup});
            
            revisedNodes.AddRange(notNeighboorGroups.SelectMany(list => list));
           
            while (revisedNodes.Count < _nodes.Count - 1)
            {
                var nextNode = minimalDistances.Where(kvp=>!revisedNodes.Contains(kvp.Key)).Aggregate((kvp1, kvp2) => kvp1.Value < kvp2.Value ? kvp1 : kvp2).Key;
                revisedNodes.Add(nextNode);

                foreach (Node<T> neighboorNode in GetNeighbourNodes(nextNode).Except(new Node<T>[] {startNode}).Except(revisedNodes))
                {

                    if (minimalDistances[neighboorNode] > minimalDistances[nextNode] + GetEdgeByNodes(nextNode, neighboorNode,Direction.Both).Weight)
                    {
                        minimalDistances[neighboorNode] = minimalDistances[nextNode] + GetEdgeByNodes(nextNode, neighboorNode,Direction.Both).Weight;
                    }
                }
            }
            return minimalDistances;
        }
    }

    public class Node<T>:IComparable<Node<T>> where T:IComparable<T>
    {
        public T Data { get; private set; }

        public Node(T data)
        {
            Data = data;
        }

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
            if(Direction==other.Direction && Direction==Direction.Both){
                return (StartNode.Equals(other.StartNode) && EndNode.Equals(other.EndNode)) ||
                    (StartNode.Equals(other.EndNode) && EndNode.Equals(other.StartNode));
            }
            return StartNode.Equals(other.StartNode) && EndNode.Equals(other.EndNode) && Direction==other.Direction;
        }

        public override int GetHashCode()
        {
            return StartNode.Data.GetHashCode() + EndNode.Data.GetHashCode();
        }

        public int CompareTo(Edge<T> other)
        {
            if (StartNode.CompareTo(other.StartNode) != 0) {
                return StartNode.CompareTo(other.StartNode);
            }
            if (EndNode.CompareTo(other.EndNode) != 0)
            {
                return EndNode.CompareTo(other.EndNode);
            }
            return Direction.CompareTo(other.Direction);
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
