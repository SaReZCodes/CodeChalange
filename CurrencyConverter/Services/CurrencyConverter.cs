using System;
using CurrencyConverter.Contracts;

namespace CurrencyConverter.Services
{
    public sealed class CurrencyConverter : ICurrencyConverter
    {
        private static readonly Lazy<CurrencyConverter> _instance = new Lazy<CurrencyConverter>(() => new CurrencyConverter(), true);

        Dictionary<string, Dictionary<string, double>> exchangeRatesList = new Dictionary<string, Dictionary<string, double>>();
        List<Tuple<string, string, double>> exchangeRates = new List<Tuple<string, string, double>>();

        public static CurrencyConverter Instance() => _instance.Value;

        private CurrencyConverter()
        { }
        public void ClearConfiguration()
        {
            exchangeRatesList.Clear();
            exchangeRates.Clear();
        }

        public double Convert(string fromCurrency, string toCurrency, double amount)
        {
                var value = exchangeRatesList[fromCurrency];
                return amount * value[toCurrency];
         
        }
        double getExchangeRate(string currencyFrom, string currencyTo)
        {
            double? result = exchangeRates.Find(x => x.Item1 == currencyFrom && x.Item2 == currencyTo)?.Item3;
            if (result == null)
                result = 1 / exchangeRates.Find(x => x.Item1 == currencyTo && x.Item2 == currencyFrom).Item3;
            return (double)result;
        }
        public void UpdateConfiguration(IEnumerable<Tuple<string, string, double>> exchangeRates)
        {
            exchangeRatesList.Clear();
            addExchangeRates(exchangeRates);
            var currencyGraph = Graph<string>.CreateGraph(exchangeRates);
            foreach (var nodeItem in currencyGraph.Nodes)
            {
                exchangeRatesList.Add(nodeItem, new Dictionary<string, double>());
                var bestPath = currencyGraph.BFS(currencyGraph, nodeItem);
                foreach (var endNode in currencyGraph.Nodes)
                {
                    var shortPaths = bestPath(endNode);
                    double r = 1;
                    for (int i = 0; i < shortPaths.Count() - 1; i++)
                    {
                        r *= getExchangeRate(shortPaths[i], shortPaths[i + 1]);
                    }
                    exchangeRatesList[nodeItem].Add(endNode, r);
                }
            }
        }

        void addExchangeRates(IEnumerable<Tuple<string, string, double>> exchangeRate)
        {
            foreach (var item in exchangeRate)
            {
                exchangeRates.Add(item);
            }
        }
    }
}