using Fclp;
using System;
using System.Xml;

namespace Gigaclear_TechTest
{
    partial class Program
    {
        static void Main(string[] args)
        {
            var p = new FluentCommandLineParser<ApplicationArguments>();

            p.Setup(arg => arg.Cabinet)
             .As('c', "cabinet")
             .WithDescription("Cost per cabinet")
             .Required();

            p.Setup(arg => arg.Chamber)
             .As('h', "chamber")
             .WithDescription("Cost per chamber")
             .Required();

            p.Setup(arg => arg.Pot)
             .As('p', "pot")
             .WithDescription("Cost per pot")
             .Required();

            p.Setup(arg => arg.TrenchRoad)
             .As('r', "road")
             .WithDescription("Cost per metre of road trench")
             .Required();

            p.Setup(arg => arg.TrenchVerge)
             .As('v', "verge")
             .WithDescription("Cost per metre of verge trench")
             .Required();

            p.Setup(arg => arg.Filename)
             .As('f', "filename")
             .WithDescription("Filename of graph to calculate costs for")
             .Required();

            p.SetupHelp("?", "help")
             .Callback(text => Console.WriteLine(text));

            var result = p.Parse(args);

            if (result.HasErrors == false)
            {
                Run(p.Object);
            }
            else
            {
                Console.WriteLine("Error(s) parsing arguments");
                Console.WriteLine(result.ErrorText);
                Console.WriteLine();
                Console.WriteLine("Use the following arguments:");
                p.HelpOption.ShowHelp(p.Options);
            }
        }

        public static void Run(ApplicationArguments arguments)
        {
            var graph = Graph.ReadFromDotFile(arguments.Filename);

            var cost = graph.CalculateCost(arguments.RateCard);

            Console.WriteLine($"Graph loaded with {graph.Nodes.Count} nodes and {graph.Edges.Count} edges.");
            Console.WriteLine($"Cost using these rates will be £{cost}");
        }
    }
}
