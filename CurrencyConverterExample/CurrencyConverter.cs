using System;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyConverterExample
{
    public class CurrencyConverter : ICurrencyConverter
    {
        private readonly List<GraphNode> _graph = new List<GraphNode>();

        public void ClearConfiguration()
        {
            _graph.Clear();
        }

        public void UpdateConfiguration(IEnumerable<Tuple<string, string, double>> conversionRates)
        {
            foreach (var (fromCurrency, toCurrency, exchangeRate) in conversionRates)
            {
                _graph.Add(new GraphNode(fromCurrency, toCurrency, (decimal) Math.Round(exchangeRate, 4)));
                _graph.Add(new GraphNode(toCurrency, fromCurrency, (decimal) Math.Round(1 / exchangeRate, 4)));
            }
        }

        public double Convert(string fromCurrency, string toCurrency, double amount)
        {
            var exchangeRate = (double) ShortestPath(Children(Adjacency(_graph), fromCurrency), toCurrency)
                .Aggregate(decimal.One, (current, node) =>
                    decimal.Round(current * node.ExchangeRate, 4)
                );

            return Math.Ceiling(amount * exchangeRate);
        }


        private Dictionary<string, List<GraphNode>> Adjacency(IReadOnlyCollection<GraphNode> graph) =>
            graph.ToLookup(node => node.FromCurrency,
                    node => graph.Where(node1 => node.FromCurrency == node1.FromCurrency).ToList())
                .ToDictionary(grouping => grouping.Key, grouping => grouping.First());


        private static List<GraphNode> Children(IReadOnlyDictionary<string, List<GraphNode>> adjacency, string root)
        {
            var search = new List<string> {root};

            // Useful to transform to directed graph
            var children = new List<GraphNode> {new GraphNode("", root, decimal.One)};

            while (search.Count > 0)
            {
                var rootNode = search.Last();
                search.Remove(rootNode);

                // Visit all adjacent nodes that are not already seen
                adjacency[rootNode].ForEach(adjacent =>
                {
                    if (children.Select(child => child.ToCurrency).Contains(adjacent.ToCurrency)) return;

                    search.Add(adjacent.ToCurrency);
                    children.Add(adjacent);
                });
            }

            return children;
        }

        private static IEnumerable<GraphNode> ShortestPath(IReadOnlyCollection<GraphNode> children, string to)
        {
            var predecessor = children.First(node => node.ToCurrency == to);

            return predecessor.FromCurrency != ""
                ? ShortestPath(children, predecessor.FromCurrency).Append(predecessor)
                : new List<GraphNode>();
        }
    }
}