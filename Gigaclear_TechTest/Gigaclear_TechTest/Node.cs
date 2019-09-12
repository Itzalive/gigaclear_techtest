using System;
using System.Collections.Generic;
using System.Text;

namespace Gigaclear_TechTest
{
    public struct Node
    {
        public string Id { get; }
        public NodeType Type { get; }

        public Node(string id, NodeType type)
        {
            Id = id;
            Type = type;
        }
    }
}
