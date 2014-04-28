using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace MyLittleGraph
{
    static class DataAnalysis
    {
        public static IEnumerable<DataPoint> ToHistogramData(IEnumerable<int> data)
        {
            int count = 0;
            foreach (var i in data)
            {
                count++;
                yield return new DataPoint(i, count);
            }
        }
    }

    class Program
    {
        private const int NodeCount = 10000; //при n = 10^6 работает секунд 10, при n <= 10^5 очень быстро
        private const int M = 2;

        static void Main(string[] args)
        {
            var nodeThree = new EdgeThree(NodeCount); //это дерево отрезков, с ним за log(n) можно узнавать номер вершины по случайному числу
            var graph = new Graph();
            AddStartNode(graph, nodeThree);
            MakeThree(nodeThree, graph);
            File.WriteAllLines("graph.dot", CreateDotGraph(graph)); //граф получился уродский...
            Chart.ShowHistogram("Graph", DataAnalysis.ToHistogramData(graph.GetGraphInfo())); //диаграмка тоже :(
        }

        private static List<string> CreateDotGraph(Graph graph)
        {
            var lines = new List<string>();
            lines.Add("graph MyLittleGraph {");
            //lines.Add("dpi=\"600\";");
            lines.Add(
                "node[shape= \"plaintext\", shape = \"circle\", style = \"filled\", fillcolor = \"white\", fontcolor = \"white\"];");
            lines.Add("edge[color = \"orange\"];");
            lines.AddRange(graph.GetAllEdges());
            lines.Add("}");
            return lines;
        }

        private static void MakeThree(EdgeThree edgeThree, Graph graph)
        {
            for (var currentNode = 2; currentNode < NodeCount; currentNode++)
            {
                graph.AddNode(currentNode);
                var used = new HashSet<int>(new[] {currentNode});
                for (var i = 0; i < M; i++)
                {
                    var randomNumber = GetRandomNumber(edgeThree);
                    var selectedNode = edgeThree.GetIndexForRandomNumber(randomNumber);
                    if (used.Contains(selectedNode))
                        selectedNode = SelectEmptyNode(selectedNode, graph);
                    used.Add(selectedNode);
                    graph.AddEdge(currentNode, selectedNode);
                    edgeThree.AddEdge(currentNode, selectedNode);
                }
            }
        }

        private static int SelectEmptyNode(int selectedNode, Graph graph)
        {
            if (selectedNode == 0)
                return 1;
            if (selectedNode >= graph.GetTop() - 2)
                return selectedNode - 1;
            return selectedNode + 1;
        }

        private static int GetRandomNumber(EdgeThree edgeThree)
        {
            var random = new Random();
            return random.Next(edgeThree.GetTop()) + 1;
        }

        private static void AddStartNode(Graph graph, EdgeThree edgeThree)
        {
            graph.AddNode(0);
            graph.AddNode(1);
            graph.AddEdge(1, 0);
            edgeThree.AddEdge(1, 0);
        }
    }
}
