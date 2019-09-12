using System;
using System.Collections.Generic;
using System.Text;

namespace Gigaclear_TechTest
{
    public class Graph
    {
        public List<Node> Nodes { get; } = new List<Node>();

        public List<Edge> Edges { get; } = new List<Edge>();

        public void AddNode(Node node)
        {
            if (!Nodes.Contains(node))
                Nodes.Add(node);
        }

        public void AddEdge(Edge edge)
        {
            if (!Nodes.Contains(edge.StartNode) || !Nodes.Contains(edge.EndNode))
                throw new ArgumentException("Edge links to non-existant node(s)");

            if (!Edges.Contains(edge))
                Edges.Add(edge);
        }
    }
}
