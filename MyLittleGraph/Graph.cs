using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLittleGraph
{
    public class Graph
    {
        private readonly Dictionary<int, HashSet<int>> _graph;

        public Graph()
        {
            _graph = new Dictionary<int, HashSet<int>>();
        }

        public void AddNode(int node)
        {
            _graph[node] = new HashSet<int>();
        }

        public void AddEdge(int firstNode, int secondNode)
        {
            _graph[firstNode].Add(secondNode);
            _graph[secondNode].Add(firstNode);
        }

        public int GetTop()
        {
            return _graph.Count;
        }

        public IEnumerable<int> GetGraphInfo()
        {
            return _graph.Select(e => e.Value.Count);
        }

        public IEnumerable<string> GetAllEdges()
        {
            var used = new HashSet<int>();
            //yield return "\n";
            //foreach (var e in _graph)
            //{
            //    yield return e.Key + "[fixedsize = \"true\", width = \"" + e.Value.Count + "\", height = \"" + e.Value.Count + "\"]";
            //}
            //yield return "\n";
            foreach (var node in _graph)
            {
                foreach (var neighbour in node.Value.Where(x => !used.Contains(x)))
                    yield return "\t" + node.Key + " -- " + neighbour + ";";
                used.Add(node.Key);
            }
        }
    }
}
