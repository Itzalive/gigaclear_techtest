﻿using System;
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
