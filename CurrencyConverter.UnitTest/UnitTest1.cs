using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CurrencyConverter.UnitTest;

[TestClass]
public class UnitTest1
{
    CurrencyConverter.Contracts.ICurrencyConverter service;
    [TestInitialize]
    public void TestInitialize()
    {
        service = CurrencyConverter.Services.CurrencyConverter.Instance();
    }

    [TestMethod]
    public void Convert_CurrencyUnit_ReturnsConvertedAmount()
    {
        var edges = new[]{Tuple.Create("USD","CAD",1.34), Tuple.Create("CAD","GBP",0.58),
                Tuple.Create("USD","Rial",10d)};
        Graph<string> graph = Graph<string>.CreateGraph(edges);
        service.UpdateConfiguration(edges);
        var d = service.Convert("USD", "Rial", 1);
        Assert.AreEqual(10, d);
        var d1 = service.Convert("Rial", "USD", 10);
        Assert.AreEqual(1, d1);
    }

    [TestMethod]
    public void Get_BFS_ShortPath()
    {
        var edges = new[]{Tuple.Create("USD","CAD",1.34), Tuple.Create("CAD","GBP",0.58),
                Tuple.Create("USD","Rial",10d)};
        Graph<string> graph = Graph<string>.CreateGraph(edges);
        var bestPath = graph.BFS(graph, "Rial");
        Assert.AreEqual("Rial", string.Join(",", bestPath("Rial").ToList()));
        Assert.AreEqual("Rial,USD", string.Join(",", bestPath("USD").ToList()));
        Assert.AreEqual("Rial,USD,CAD", string.Join(",", bestPath("CAD").ToList()));
    }

     [TestMethod]
    public void Get_BFS_ShortPath_Better()
    {
        var edges = new[]{Tuple.Create("USD","CAD",1.34), Tuple.Create("CAD","GBP",0.58),
                Tuple.Create("USD","Rial",10d),Tuple.Create("CAD","Rial",10d)};
        Graph<string> graph = Graph<string>.CreateGraph(edges);
        var bestPath = graph.BFS(graph, "Rial");
        Assert.AreEqual("Rial,CAD", string.Join(",", bestPath("CAD").ToList()));
    }
}