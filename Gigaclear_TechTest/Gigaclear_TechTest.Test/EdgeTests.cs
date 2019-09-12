using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gigaclear_TechTest.Test
{
    public class EdgeTests
    {
        [Test]
        public void StartEndNodeSame()
        {
            var node = new Node("A", NodeType.Chamber);
            Assert.Throws<ArgumentException>(() => new Edge(node, node, EdgeType.None, 1));
        }
    }
}
