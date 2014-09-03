using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiParser
{
    public class Component
    {
        public int NodeCount { get { return Nodes.Count; } }
        public double NodesPersent { get; set; }
        public string TopNode { get; set; }
        private int MaxNodeDegree;
        public HashSet<string> Nodes;

        public Component()
        {
            MaxNodeDegree = 0;
            NodesPersent = 0;
            TopNode = "";
            Nodes = new HashSet<string>();
        }

        public void ChangeTopNode(string node, int nodeDegree)
        {
            if (nodeDegree <= MaxNodeDegree) return;
            MaxNodeDegree = nodeDegree;
            TopNode = node;
        }
    }

    public class Node
    {
        private readonly HashSet<string> _neigbours;

        public Node()
        {
            _neigbours = new HashSet<string>();
        }

        public void AddNeigbour(string neigbour)
        {
            _neigbours.Add(neigbour);
        }

        public IEnumerable<string> GetNeighbours()
        {
            return _neigbours;
        }

        public void RemovedNode(string node)
        {
            _neigbours.Remove(node);
        }

        public bool Contain(string node)
        {
            return _neigbours.Contains(node);
        }

        public int EdgesCount {get { return _neigbours.Count; } }

    }

    class Program
    {
        private const string Path = @"C:\Users\213\Desktop\graph.txt";
        private const int ComponentsCount = 20;

        static void Main()
        {
            var graph = BuildGraph(ReadPair());
            var components = BuildComponents(graph);
            foreach (var component in components.OrderByDescending(x => x.NodeCount).Take(ComponentsCount))
                PrintComponents(component, graph);

            Console.WriteLine("\n\n\n");

            for (var i = 0; i < 1000; i++)
            {
                var topNode = "";
                var neighbourCount = 0;
                foreach (var node in graph.Where(node => node.Value.EdgesCount > neighbourCount))
                {
                    neighbourCount = node.Value.EdgesCount;
                    topNode = node.Key;
                }
                foreach (var node in graph[topNode].GetNeighbours())
                    graph[node].RemovedNode(topNode);
                graph.Remove(topNode);
            }

            var newComponents = BuildComponents(graph);
            foreach (var component in newComponents.OrderByDescending(x => x.NodeCount).Take(ComponentsCount))
                PrintComponents(component, graph);
            //TestRobustness(graph, components);
        }

        private static void PrintComponents(Component component, Dictionary<string, Node> graph)
        {
            var myTest = TestLengthFromTopNode(component, graph);
            Console.WriteLine("{0} {1}% {2}\t{3}", component.NodeCount, component.NodesPersent, component.TopNode, myTest);
            
        }

        private static int TestLengthFromTopNode(Component component, Dictionary<string, Node> graph)
        {
            var visitedNodes = new HashSet<string>();
            var answer = 0;
            foreach (var node in graph.Where(x => !visitedNodes.Contains(x.Key)).Where(x => component.Nodes.Contains(x.Key)))
            {
                var queue = new Queue<Tuple<string, int>>();
                queue.Enqueue(Tuple.Create(node.Key, 0));
                while (queue.Count != 0)
                {
                    var localNode = queue.Dequeue();
                    foreach (var neighbour in graph[localNode.Item1].GetNeighbours()
                        .Where(x => !visitedNodes.Contains(x))
                        .Where(graph.ContainsKey)
                        .Where(x => component.Nodes.Contains(x)))
                    {
                        visitedNodes.Add(neighbour);
                        queue.Enqueue(Tuple.Create(neighbour, localNode.Item2 + 1));
                    }
                    answer = Math.Max(answer, localNode.Item2);
                }
            }
            return answer;
        }

        private static IEnumerable<Component> SplitComponent(Component oldComponent, Dictionary<string, Node> graph)
        {
            var visitedNodes = new HashSet<string>();
            var components = new HashSet<Component>();
            foreach (var node in oldComponent.Nodes.Where(x => !visitedNodes.Contains(x)).Where(graph.ContainsKey))
            {
                var component = new Component();
                var queue = new Queue<string>();
                queue.Enqueue(node);
                while (queue.Count != 0)
                {
                    var localNode = queue.Dequeue();
                    component.Nodes.Add(localNode);
                    component.ChangeTopNode(localNode, graph[localNode].EdgesCount);
                    foreach (var neighbour in graph[localNode].GetNeighbours()
                        .Where(x => !visitedNodes.Contains(x))
                        .Where(graph.ContainsKey)
                        .Where(oldComponent.Nodes.Contains))
                    {
                        visitedNodes.Add(neighbour);
                        queue.Enqueue(neighbour);
                    }
                }
                component.NodesPersent = (double)component.NodeCount * 100 / graph.Count;
                components.Add(component);
            }
            return components;
        }


        private static void TestRobustness(IDictionary<string, Node> graph, HashSet<Component> components)
        {
            var graphCopy = new Dictionary<string, Node>(graph);
            var newComponents = new HashSet<Component>(components);
            var postAnswer = GetFirstComponents(graphCopy, newComponents);
            var removedNodes = GetTopNodesFromGraph(graph);
            components = BuildComponents(graph);
            var bigComponents = new HashSet<HashSet<Component>>();
            foreach (var comp in components)
                bigComponents.Add(new HashSet<Component>(new[] { comp }));
            var dict = new Dictionary<string, HashSet<Component>>();
            foreach (var component in bigComponents)
                foreach (var node in component.SelectMany(smallComponent => smallComponent.Nodes))
                    dict[node] = component;
            var preAnswer = GetLastComponents(removedNodes, bigComponents, dict);
            PrintAnswer(preAnswer.Concat(postAnswer.ToArray().Reverse()));
        }

        private static void PrintAnswer(IEnumerable<Tuple<int, int>> answer)
        {
            foreach (var ans in answer)
                Console.WriteLine("{0}\t{1}", ans.Item1, ans.Item2);
        }

        private static List<Tuple<int, int>> GetFirstComponents(
            Dictionary<string, Node> graphCopy, HashSet<Component> newComponents)
        {
            var answer = new List<Tuple<int, int>>();
            for (var deletedNodeCount = 1; deletedNodeCount <= 50; deletedNodeCount++)
            {
                var topNode = "";
                topNode = GetTopNode(graphCopy, topNode);
                foreach (var comp in newComponents.ToList().Where(comp => comp.Nodes.Contains(topNode)))
                {
                    newComponents.Remove(comp);
                    foreach (var newComponent in SplitComponent(comp, graphCopy))
                        newComponents.Add(newComponent);
                }
                answer.Add(Tuple.Create(deletedNodeCount, newComponents.Max(x => x.NodeCount)));
            }
            return answer;
        }

        private static string GetTopNode(IDictionary<string, Node> graph, string topNode)
        {
            var neighbourCount = 0;
            foreach (var node in graph.Where(node => node.Value.EdgesCount > neighbourCount))
            {
                neighbourCount = node.Value.EdgesCount;
                topNode = node.Key;
            }
            foreach (var node in graph[topNode].GetNeighbours())
                graph[node].RemovedNode(topNode);
            graph.Remove(topNode);
            return topNode;
        }

        private static IEnumerable<Tuple<int, int>> GetLastComponents(
            List<Tuple<string, Node>> removedNodes, HashSet<HashSet<Component>> bigComponents, Dictionary<string, HashSet<Component>> dict)
        {
            var preAnswer = new List<Tuple<int, int>>();
            for (var deletedNodeCount = 999; deletedNodeCount > 50; deletedNodeCount -= 1)
            {
                var concat = new HashSet<Component>();
                foreach (var node in removedNodes[deletedNodeCount].Item2.GetNeighbours().Where(dict.ContainsKey))
                {
                    foreach (var comp in dict[node])
                        concat.Add(comp);
                    bigComponents.Remove(dict[node]);
                    dict[node] = concat;
                }
                dict[removedNodes[deletedNodeCount].Item1] = concat;
                bigComponents.Add(concat);
                preAnswer.Add(Tuple.Create(deletedNodeCount, bigComponents.Max(x => x.Aggregate(0, AddCount))));
            }
            return preAnswer;
        }

        private static List<Tuple<string, Node>> GetTopNodesFromGraph(IDictionary<string, Node> graph)
        {
            var removedNodes = new List<Tuple<string, Node>>();
            for (var i = 0; i < 1000; i++)
            {
                var topNode = "";
                var neighbourCount = 0;
                foreach (var node in graph.Where(node => node.Value.EdgesCount > neighbourCount))
                {
                    neighbourCount = node.Value.EdgesCount;
                    topNode = node.Key;
                }
                removedNodes.Add(Tuple.Create(topNode, graph[topNode]));
                foreach (var node in graph[topNode].GetNeighbours())
                    graph[node].RemovedNode(topNode);
                graph.Remove(topNode);
            }
            return removedNodes;
        }

        private static int AddCount(int accumulator, Component comp)
        {
            return accumulator + comp.NodeCount;
        }


        private static Component ConcatComponents(Component accumulator, Component y)
        {
            foreach (var node in y.Nodes)
                accumulator.Nodes.Add(node);
            return accumulator;
        }

        private static HashSet<Component> BuildComponents(IDictionary<string, Node> graph)
        {
            var visitedNodes = new HashSet<string>();
            var components = new HashSet<Component>();
            foreach (var node in graph.Where(x => !visitedNodes.Contains(x.Key)))
            {
                var component = new Component();
                var queue = new Queue<string>();
                queue.Enqueue(node.Key);
                while (queue.Count != 0)
                {
                    var localNode = queue.Dequeue();
                    component.Nodes.Add(localNode);
                    component.ChangeTopNode(localNode, graph[localNode].EdgesCount);
                    foreach (var neighbour in graph[localNode].GetNeighbours()
                        .Where(x => !visitedNodes.Contains(x))
                        .Where(graph.ContainsKey))
                    {
                        visitedNodes.Add(neighbour);
                        queue.Enqueue(neighbour);
                    }
                }
                component.NodesPersent = (double)component.NodeCount*100/graph.Count;
                components.Add(component);
            }
            return components;
        }

        private static Dictionary<string, Node> BuildGraph(IEnumerable<Tuple<string, string>> edges)
        {
            var graph = new Dictionary<string, Node>();
            foreach (var edge in edges)
            {
                if (!graph.ContainsKey(edge.Item1))
                    graph[edge.Item1] = new Node();
                graph[edge.Item1].AddNeigbour(edge.Item2);
                if (!graph.ContainsKey(edge.Item2))
                    graph[edge.Item2] = new Node();
                graph[edge.Item2].AddNeigbour(edge.Item1);
            }
            return graph;
        }

        private static IEnumerable<Tuple<string, string>> ReadPair()
        {
            return File.ReadLines(Path).Select(x => x.Split(',')).Select(x => Tuple.Create(x[0], x[1]));
        }
    }
}
