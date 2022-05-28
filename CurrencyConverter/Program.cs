CurrencyConverter.Contracts.ICurrencyConverter currencyConverter = CurrencyConverter.Services.CurrencyConverter.Instance();
var edges = new[]{Tuple.Create("USD","CAD",1.34), Tuple.Create("CAD","GBP",0.58),
                Tuple.Create("USD","Rial",10d)};

currencyConverter.UpdateConfiguration(edges);
var d = currencyConverter.Convert("USD", "Rial", 1);
Console.WriteLine(d);
var d1 = currencyConverter.Convert("Rial", "USD", 10);
Console.WriteLine(d1);


var edges2 = new[]{Tuple.Create("USD","CAD",1.34), Tuple.Create("CAD","GBP",0.58),
                Tuple.Create("USD","Rial",10d),};
Graph<string> graph2 = Graph<string>.CreateGraph(edges2);
var f = graph2.BFS(graph2, "Rial");
foreach (var item in f("Rial").ToList())
{
    Console.WriteLine(item);
}
