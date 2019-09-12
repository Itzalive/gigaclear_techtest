using NUnit.Framework;
using System;
using System.IO;

namespace Gigaclear_TechTest.Test
{
    public class GraphTests
    {
        [Test]
        public void CreateGraphEmpty()
        {
            // Arrange
            // Act
            var graph = new Graph();

            // Assert
            Assert.That(graph.Nodes.Count, Is.EqualTo(0));
            Assert.That(graph.Edges.Count, Is.EqualTo(0));
        }

        [Test]
        public void AddNode()
        {
            // Arrange
            var graph = new Graph();
            var node = new Node("A", NodeType.Cabinet);

            // Act
            graph.AddNode(node);

            // Assert
            Assert.That(graph.Nodes.Count, Is.EqualTo(1));
            Assert.That(graph.Nodes, Contains.Item(node));
            Assert.That(graph.Edges.Count, Is.EqualTo(0));
        }

        [Test]
        public void AddSecondNode()
        {
            // Arrange
            var graph = new Graph();
            var nodeA = new Node("A", NodeType.Cabinet);
            graph.AddNode(nodeA);
            var nodeB = new Node("B", NodeType.Cabinet);

            // Act
            graph.AddNode(nodeB);

            // Assert
            Assert.That(graph.Nodes.Count, Is.EqualTo(2));
            Assert.That(graph.Nodes, Contains.Item(nodeB));
            Assert.That(graph.Edges.Count, Is.EqualTo(0));
        }

        [Test]
        public void AddNodeDuplicate()
        {
            // Arrange
            var graph = new Graph();
            var node = new Node("A", NodeType.Cabinet);
            graph.AddNode(node);

            // Act
            graph.AddNode(node);

            // Assert
            Assert.That(graph.Nodes.Count, Is.EqualTo(1));
            Assert.That(graph.Nodes, Contains.Item(node));
            Assert.That(graph.Edges.Count, Is.EqualTo(0));
        }

        [Test]
        public void AddEdge()
        {
            // Arrange
            var graph = new Graph();
            var nodeA = new Node("A", NodeType.Cabinet);
            var nodeB = new Node("B", NodeType.Cabinet);
            graph.AddNode(nodeA);
            graph.AddNode(nodeB);
            var edge = new Edge(nodeA, nodeB, EdgeType.Road, 10);

            // Act
            graph.AddEdge(edge);

            // Assert
            Assert.That(graph.Nodes.Count, Is.EqualTo(2));
            Assert.That(graph.Edges.Count, Is.EqualTo(1));
            Assert.That(graph.Edges, Contains.Item(edge));
        }

        [Test]
        public void AddSecondEdge()
        {
            // Arrange
            var graph = new Graph();
            var nodeA = new Node("A", NodeType.Cabinet);
            var nodeB = new Node("B", NodeType.Cabinet);
            var nodeC = new Node("C", NodeType.Cabinet);
            graph.AddNode(nodeA);
            graph.AddNode(nodeB);
            graph.AddNode(nodeC);
            var edge1 = new Edge(nodeA, nodeB, EdgeType.Road, 10);
            graph.AddEdge(edge1);
            var edge2 = new Edge(nodeB, nodeC, EdgeType.Road, 10);

            // Act
            graph.AddEdge(edge2);

            // Assert
            Assert.That(graph.Nodes.Count, Is.EqualTo(3));
            Assert.That(graph.Edges.Count, Is.EqualTo(2));
            Assert.That(graph.Edges, Contains.Item(edge2));
        }

        [Test]
        public void AddEdgeDuplicate()
        {
            // Arrange
            var graph = new Graph();
            var nodeA = new Node("A", NodeType.Cabinet);
            var nodeB = new Node("B", NodeType.Cabinet);
            graph.AddNode(nodeA);
            graph.AddNode(nodeB);
            var edge = new Edge(nodeA, nodeB, EdgeType.Road, 10);
            graph.AddEdge(edge);

            // Act
            graph.AddEdge(edge);

            // Assert
            Assert.That(graph.Nodes.Count, Is.EqualTo(2));
            Assert.That(graph.Edges.Count, Is.EqualTo(1));
            Assert.That(graph.Edges, Contains.Item(edge));
        }

        [Test]
        public void AddEdgeMissingStartNode()
        {
            // Arrange
            var graph = new Graph();
            var nodeA = new Node("A", NodeType.Cabinet);
            var nodeB = new Node("B", NodeType.Cabinet);
            graph.AddNode(nodeA);
            var edge = new Edge(nodeA, nodeB, EdgeType.Road, 10);

            // Act / Assert
            Assert.Throws<ArgumentException>(() => graph.AddEdge(edge));
        }

        [Test]
        public void AddEdgeMissingEndNode()
        {
            // Arrange
            var graph = new Graph();
            var nodeA = new Node("A", NodeType.Cabinet);
            var nodeB = new Node("B", NodeType.Cabinet);
            graph.AddNode(nodeB);
            var edge = new Edge(nodeA, nodeB, EdgeType.Road, 10);

            // Act / Assert
            Assert.Throws<ArgumentException>(() => graph.AddEdge(edge));
        }

        [Test]
        public void ReadFromDotFileSingleNode()
        {
            // Arrange
            var lines = "A [type=Cabinet];";

            // Act
            var graph = readDotFileHelper(lines);

            // Assert
            Assert.That(graph.Nodes.Count, Is.EqualTo(1));
            Assert.That(graph.Edges.Count, Is.EqualTo(0));
            Assert.That(graph.Nodes[0].Id, Is.EqualTo("A"));
            Assert.That(graph.Nodes[0].Type, Is.EqualTo(NodeType.Cabinet));
        }

        [Test]
        public void ReadFromDotFileSingleNodeAlternate()
        {
            // Arrange
            var lines = "C [type=Pot];";

            // Act
            var graph = readDotFileHelper(lines);

            // Assert
            Assert.That(graph.Nodes.Count, Is.EqualTo(1));
            Assert.That(graph.Edges.Count, Is.EqualTo(0));
            Assert.That(graph.Nodes[0].Id, Is.EqualTo("C"));
            Assert.That(graph.Nodes[0].Type, Is.EqualTo(NodeType.Pot));
        }

        [Test]
        public void ReadFromDotFileMultipleNodes()
        {
            // Arrange
            var lines = "C [type=Pot];\r\n" +
                        "ASD [type=Chamber];";

            // Act
            var graph = readDotFileHelper(lines);

            // Assert
            Assert.That(graph.Nodes.Count, Is.EqualTo(2));
            Assert.That(graph.Edges.Count, Is.EqualTo(0));
            Assert.That(graph.Nodes[0].Id, Is.EqualTo("C"));
            Assert.That(graph.Nodes[0].Type, Is.EqualTo(NodeType.Pot));
            Assert.That(graph.Nodes[1].Id, Is.EqualTo("ASD"));
            Assert.That(graph.Nodes[1].Type, Is.EqualTo(NodeType.Chamber));
        }

        [Test]
        public void ReadFromDotFileSingleEdge()
        {
            // Arrange
            var lines = "START [type=Pot];\r\n" +
                        "End [type=Chamber];\r\n" +
                        "START -- End  [material=verge, length=100];";

            // Act
            var graph = readDotFileHelper(lines);

            // Assert
            Assert.That(graph.Nodes.Count, Is.EqualTo(2));
            Assert.That(graph.Edges.Count, Is.EqualTo(1));
            Assert.That(graph.Edges[0].StartNode.Id, Is.EqualTo("START"));
            Assert.That(graph.Edges[0].EndNode.Id, Is.EqualTo("End"));
            Assert.That(graph.Edges[0].Type, Is.EqualTo(EdgeType.Verge));
            Assert.That(graph.Edges[0].Length, Is.EqualTo(100));
        }

        [Test]
        public void ReadFromDotFileMultipleEdge()
        {
            // Arrange
            var lines = "START [type=Pot];\r\n" +
                        "End [type=Chamber];\r\n" +
                        "Node [type=Cabinet];\r\n" +
                        "START -- Node  [material=verge, length=100];\r\n" +
                        "End -- Node  [material=road, length=56];";

            // Act
            var graph = readDotFileHelper(lines);

            // Assert
            Assert.That(graph.Nodes.Count, Is.EqualTo(3));
            Assert.That(graph.Edges.Count, Is.EqualTo(2));
            Assert.That(graph.Edges[0].StartNode.Id, Is.EqualTo("START"));
            Assert.That(graph.Edges[0].EndNode.Id, Is.EqualTo("Node"));
            Assert.That(graph.Edges[0].Type, Is.EqualTo(EdgeType.Verge));
            Assert.That(graph.Edges[0].Length, Is.EqualTo(100));
            Assert.That(graph.Edges[1].StartNode.Id, Is.EqualTo("End"));
            Assert.That(graph.Edges[1].EndNode.Id, Is.EqualTo("Node"));
            Assert.That(graph.Edges[1].Type, Is.EqualTo(EdgeType.Road));
            Assert.That(graph.Edges[1].Length, Is.EqualTo(56));
        }

        [TestCase(1, 10, 2, 100, 3, 1000, 3210)]
        [TestCase(2, 3, 4, 4, 2, 5, 32)]
        public void CalculateCostsNodeTypes(int numCabinets, int costCabinet, int numChambers, int costChamber, int numPots, int costPot, int totalCost)
        {
            // Arrange
            var rateCard = new RateCard { Cabinet = costCabinet, Chamber = costChamber, Pot = costPot };
            var graph = new Graph();
            var n = 0;
            for (int i = 0; i < numCabinets; i++)
                graph.AddNode(new Node((++n).ToString(), NodeType.Cabinet));
            for (int i = 0; i < numChambers; i++)
                graph.AddNode(new Node((++n).ToString(), NodeType.Chamber));
            for (int i = 0; i < numPots; i++)
                graph.AddNode(new Node((++n).ToString(), NodeType.Pot));

            // Act
            var cost = graph.CalculateCost(rateCard);

            // Assert
            Assert.That(cost, Is.EqualTo(totalCost));
        }

        [Test]
        public void CalculateCostsVergeEdge()
        {
            // Arrange
            var rateCard = new RateCard { TrenchVerge = 8 };
            var graph = new Graph();
            var n = 0;
            for (int i = 0; i < 10; i++)
            graph.AddNode(new Node((++n).ToString(), NodeType.Chamber));

            graph.AddEdge(new Edge(graph.GetNodeById("1"), graph.GetNodeById("2"), EdgeType.Verge, 10));
            graph.AddEdge(new Edge(graph.GetNodeById("1"), graph.GetNodeById("3"), EdgeType.Verge, 30));
            graph.AddEdge(new Edge(graph.GetNodeById("1"), graph.GetNodeById("4"), EdgeType.Road, 30));

            // Act
            var cost = graph.CalculateCost(rateCard);

            // Assert
            Assert.That(cost, Is.EqualTo(320));
        }

        [Test]
        public void CalculateCostsRoadEdge()
        {
            // Arrange
            var rateCard = new RateCard { TrenchRoad = 6 };
            var graph = new Graph();
            var n = 0;
            for (int i = 0; i < 10; i++)
                graph.AddNode(new Node((++n).ToString(), NodeType.Chamber));

            graph.AddEdge(new Edge(graph.GetNodeById("1"), graph.GetNodeById("2"), EdgeType.Verge, 10));
            graph.AddEdge(new Edge(graph.GetNodeById("1"), graph.GetNodeById("3"), EdgeType.Verge, 30));
            graph.AddEdge(new Edge(graph.GetNodeById("1"), graph.GetNodeById("4"), EdgeType.Road, 30));

            // Act
            var cost = graph.CalculateCost(rateCard);

            // Assert
            Assert.That(cost, Is.EqualTo(180));
        }

        private Graph readDotFileHelper(string dotFileLines)
        {
            string contents = $"strict graph \"\" {{\r\n{dotFileLines}\r\n}}\r\n";
            string filename = Path.GetTempFileName();
            File.WriteAllText(filename, contents);
            return Graph.ReadFromDotFile(filename);
        }
    }
}