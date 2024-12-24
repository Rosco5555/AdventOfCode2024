
using Day23;

var G = new Dictionary<string, HashSet<string>>();
var vertices = new HashSet<string>();

foreach (var pair in File.ReadLines("../../../input.txt"))
{
    var pairStr = pair.Split("-");
    var n1 = pairStr[0];
    var n2 = pairStr[1];


    vertices.Add(n1);
    vertices.Add(n2);
    if (!G.ContainsKey(n1))
    {
        G[n1] = new HashSet<string>() { n2 };
    }
    else
    {
        G[n1].Add(n2);
    }
    if (!G.ContainsKey(n2))
    {
        G[n2] = new HashSet<string>() { n1 };
    }
    else
    {
        G[n2].Add(n1);
    }

}

var cliques = new HashSet<Clique>();


foreach (var n in G)
{
    foreach (var conn in n.Value)
    {
        foreach (var conn2 in G[conn])
        {
            if (conn2 != n.Key && (G[conn2].Contains(n.Key)))
            {
                cliques.Add(new Clique(new List<string>(){n.Key, conn, conn2}));
            }
        }
    }
}

var ans = cliques.Count(c => c.nodes.Any(x=>x.StartsWith("t")));

Console.WriteLine(ans);
var password = "";

void GetPassword(HashSet<string> currClique, HashSet<string> candidates, HashSet<string> excluded)
{
    if (!candidates.Any() && !excluded.Any())
    {
        // Sort it 
        // Array.Sort(arr);
        var pass = String.Join(",", currClique.ToList().OrderBy(x => x));
        if (pass.Length > password.Length)
        {
            password = pass;
        }
    }

    foreach (var candidate in candidates)
    {
        GetPassword(currClique.Union(new HashSet<string>(){candidate}).ToHashSet(), candidates.Intersect(G[candidate]).ToHashSet(),
            excluded.Intersect(G[candidate]).ToHashSet());
        candidates.Remove(candidate);
        excluded.Add(candidate);
    }
}

GetPassword(new HashSet<string>(), vertices, new HashSet<string>());

Console.WriteLine(password);