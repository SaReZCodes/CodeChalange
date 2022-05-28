public class Graph<T>
{
    private Dictionary<T, HashSet<T>> adjacentNodesList { get; }
    public HashSet<T> Nodes { get; }
    public Graph(HashSet<T> nodes, IEnumerable<Edge<T>> edges)
    {
        Nodes = nodes;
        adjacentNodesList = new Dictionary<T, HashSet<T>>();
        foreach (var node in nodes)
            adjacentNodesList.Add(node, new HashSet<T>());

        foreach (var edge in edges)
        {
            adjacentNodesList[edge.NodeA].Add(edge.NodeB);
            adjacentNodesList[edge.NodeB].Add(edge.NodeA);
        }
    }
    public static Graph<T> CreateGraph(IEnumerable<Edge<T>> edges)
    {
        HashSet<T> nodes = new HashSet<T>();
        foreach (var edge in edges)
        {
            nodes.Add(edge.NodeA);
            nodes.Add(edge.NodeB);
        }
        return new Graph<T>(nodes, edges);
    }
    public static Graph<T> CreateGraph(IEnumerable<Tuple<T, T, double>> rates)
    {
        HashSet<T> nodes = new HashSet<T>();
        List<Edge<T>> edges = new List<Edge<T>>();
        foreach (var rate in rates)
        {
            nodes.Add(rate.Item1);
            nodes.Add(rate.Item2);
            edges.Add(new Edge<T>(rate.Item1, rate.Item2));
        }
        return new Graph<T>(nodes, edges);
    }

    //Breadth-first search (BFS)
    public Func<T, IList<T>> BFS<T>(Graph<T> graph, T start)
    {
        var previous = new Dictionary<T, T>();

        var queue = new Queue<T>();
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            var vertex = queue.Dequeue();
            foreach (var adjacent in graph.adjacentNodesList[vertex])
            {
                if (previous.ContainsKey(adjacent))
                    continue;

                previous[adjacent] = vertex;
                queue.Enqueue(adjacent);
            }
        }

        Func<T, IList<T>> shortestPath = v =>
        {
            var path = new List<T> { };

            var current = v;
            while (!current.Equals(start))
            {
                path.Add(current);
                current = previous[current];
            };

            path.Add(start);
            path.Reverse();

            return path;
        };

        return shortestPath;
    }
}

public struct Edge<T>
{
    public T NodeA;
    public T NodeB;
    public Edge(T nodeA, T nodeB)
    {
        NodeA = nodeA;
        NodeB = nodeB;
    }
}