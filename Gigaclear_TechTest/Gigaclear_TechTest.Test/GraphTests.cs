using NUnit.Framework;
using System;

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
    }
}