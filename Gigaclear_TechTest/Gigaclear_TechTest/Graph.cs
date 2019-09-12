using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Gigaclear_TechTest
{
    public class Graph
    {
        private const string nodeIdRegex = "[a-zA-Z0-9]+";
        private const string graphDotFileRegex = @"strict graph ""(.*)"" \{([\s\S]+)\}";
        private const string nodeDotFileRegex = @"(" + nodeIdRegex + @") \[([^\]]+)\]";
        private const string edgeDotFileRegex = @"(" + nodeIdRegex + @") -- (" + nodeIdRegex + @") +\[([^\]]+)\]";

        public List<Node> Nodes { get; } = new List<Node>();

        public List<Edge> Edges { get; } = new List<Edge>();

        public void AddNode(Node node)
        {
            if (Nodes.Contains(node))
                return;

            if (Nodes.Any((a) => a.Id == node.Id))
                throw new ArgumentException("Node with same Id already exists");

            Nodes.Add(node);
        }

        public void AddEdge(Edge edge)
        {
            if (!Nodes.Contains(edge.StartNode) || !Nodes.Contains(edge.EndNode))
                throw new ArgumentException("Edge links to non-existant node(s)");

            if (!Edges.Contains(edge))
                Edges.Add(edge);
        }

        public Node GetNodeById(string id)
        {
            return Nodes.Single((a) => a.Id == id);
        }

        public static Graph ReadFromDotFile(string filename)
        {
            Graph graph = new Graph();

            var fileContents = File.ReadAllText(filename);

            if (!Regex.IsMatch(fileContents, graphDotFileRegex))
                throw new FormatException($"File '{filename}' is not in correct format");

            var graphMatch = Regex.Match(fileContents, graphDotFileRegex);
            var lines = graphMatch.Groups[2].Value.Split(';');
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                graph.processDotFileLine(line.Trim());
            }

            return graph;
        }

        public int CalculateCost(RateCard rateCard)
        {
            var cost = 0;
            cost += rateCard.Cabinet * Nodes.Count(node => node.Type == NodeType.Cabinet);
            cost += rateCard.Chamber * Nodes.Count(node => node.Type == NodeType.Chamber);
            cost += rateCard.Pot * Nodes.Count(node => node.Type == NodeType.Pot);
            cost += rateCard.TrenchRoad * Edges.Where(edge => edge.Type == EdgeType.Road).Sum(edge => edge.Length);
            cost += rateCard.TrenchVerge * Edges.Where(edge => edge.Type == EdgeType.Verge).Sum(edge => edge.Length);
            cost += rateCard.PotFromCabinet * Nodes.Where(node => node.Type == NodeType.Pot).Sum(node => DistanceToCabinetFromNode(node));
            return cost;
        }

        public int DistanceToCabinetFromNode(Node node)
        {
            Dictionary<Node, int> distanceToNodes = new Dictionary<Node, int>();

            distanceToNodes.Add(node, 0);

            distanceToNodes = calculateDistanceNextNodes(node, distanceToNodes, 0);

            return distanceToNodes.Where((kvp) => kvp.Key.Type == NodeType.Cabinet).OrderBy(kvp => kvp.Value).First().Value;
        }

        private Dictionary<Node, int> calculateDistanceNextNodes(Node node, Dictionary<Node, int> distanceToNodes, int distanceAlready)
        {
            var linkedEdges = Edges.Where(edge => edge.StartNode.Id == node.Id || edge.EndNode.Id == node.Id);
            foreach (var edge in linkedEdges)
            {
                var otherNode = edge.StartNode.Id == node.Id ? edge.EndNode : edge.StartNode;
                if (distanceToNodes.ContainsKey(otherNode))
                    continue;
                distanceToNodes.Add(otherNode, edge.Length + distanceAlready);
                distanceToNodes = calculateDistanceNextNodes(otherNode, distanceToNodes, edge.Length + distanceAlready);
            }
            return distanceToNodes;
        }

        private void processDotFileLine(string line)
        {

            if (Regex.IsMatch(line, nodeDotFileRegex))
            {
                var nodeMatch = Regex.Match(line, nodeDotFileRegex);
                var arguments = readArgumentsList(nodeMatch.Groups[2].Value);
                var node = new Node(nodeMatch.Groups[1].Value, (NodeType)Enum.Parse(typeof(NodeType), arguments["type"]));
                AddNode(node);
            }
            else if (Regex.IsMatch(line, edgeDotFileRegex))
            {
                var edgeMatch = Regex.Match(line, edgeDotFileRegex);
                var startNode = GetNodeById(edgeMatch.Groups[1].Value);
                var endNode = GetNodeById(edgeMatch.Groups[2].Value);
                var arguments = readArgumentsList(edgeMatch.Groups[3].Value);
                var edge = new Edge(startNode, endNode, (EdgeType)Enum.Parse(typeof(EdgeType), arguments["material"], true), int.Parse(arguments["length"]));
                AddEdge(edge);
            }
            else
            {
                throw new Exception("Unknown line in DOT graph");
            }
        }

        private static IDictionary<string, string> readArgumentsList(string argumentsList)
        {
            return Regex.Matches(argumentsList, "([^?=, ]+)(=([^,]*))?").Cast<Match>().ToDictionary(x => x.Groups[1].Value, x => x.Groups[3].Value);
        }

    }
}
