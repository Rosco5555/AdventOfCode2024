var lines = File.ReadLines("../../../input.txt").ToList();

var towels = lines[0].Split(", ");


// Go through string
// If it starts with a towel good, otherwise get rid of it
// Now check the rest of it
// recur...

var cache = new Dictionary<string, long>();

long Ways(string target)
{
    if (cache.ContainsKey(target))
    {
        return cache[target];
    }
    if (target == "")
    {
        return 1;
    }

    long ways = 0;
    foreach(var towel in towels)
    {
        if (target.StartsWith(towel))
        {
            var tar = target.Substring(towel.Length);
            ways += Ways(tar);
        }
    }

    cache[target] = ways;

    return ways;
}

long possible = 0;

foreach (var design in lines.Skip(2))
{
   
    possible += Ways(design);

}


Console.WriteLine(possible);