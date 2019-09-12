using System;
using System.Collections.Generic;
using System.Text;

namespace Gigaclear_TechTest
{
    public struct Edge
    {
        public Node StartNode { get; }
        public Node EndNode { get; }
        public EdgeType Type { get; }
        public int Length { get; }

        public Edge(Node startNode, Node endNode, EdgeType type, int length)
        {
            if (startNode.Equals(endNode))
                throw new ArgumentException("Start and end node cannot be the same");
            StartNode = startNode;
            EndNode = endNode;
            Type = type;
            Length = length;
        }
    }
}
