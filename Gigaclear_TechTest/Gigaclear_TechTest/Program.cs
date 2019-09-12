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
             .WithDescription("Cost per cabinet");

            p.Setup(arg => arg.Chamber)
             .As('h', "chamber")
             .WithDescription("Cost per chamber");

            p.Setup(arg => arg.Pot)
             .As('p', "pot")
             .WithDescription("Cost per pot");

            p.Setup(arg => arg.TrenchRoad)
             .As('r', "road")
             .WithDescription("Cost per metre of road trench");

            p.Setup(arg => arg.TrenchVerge)
             .As('v', "verge")
             .WithDescription("Cost per metre of verge trench");

            p.Setup(arg => arg.PotFromCabinet)
             .As("potfromcabinet")
             .WithDescription("Cost per pot per metre from the cabinet");

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

            Console.WriteLine($"Graph loaded with {graph.Nodes.Count} nodes and {graph.Edges.Count} edges.");

            if (arguments.RateCard.IsNonZero)
            {
                DisplayRatesForGraph(graph, arguments.RateCard);
            }
            else
            {
                Console.WriteLine("--Rate card 1--");
                DisplayRatesForGraph(graph, new RateCard { Cabinet = 1000, TrenchVerge = 50, TrenchRoad = 100, Chamber = 200, Pot = 100 });
                Console.WriteLine();
                Console.WriteLine("--Rate card 2--");
                DisplayRatesForGraph(graph, new RateCard { Cabinet = 1200, TrenchVerge = 40, TrenchRoad = 80, Chamber = 200, PotFromCabinet = 20 });
            }
        }

        private static void DisplayRatesForGraph(Graph graph, RateCard rateCard)
        {
            Console.WriteLine("Using rates:");
            Console.WriteLine(rateCard.ToString());
            var cost = graph.CalculateCost(rateCard);
            Console.WriteLine($"Cost using these rates will be £{cost}");
        }
    }
}
