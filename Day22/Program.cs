

using Day22;

long Mix(long secret, long value)
{
    return secret ^ value;
}

long Prune(long secret)
{
    return secret % 16777216;
}

//
long NextSecret(long secret)
{
    var res = secret * 64;
    secret = Mix(secret, res);
    secret = Prune(secret);
    res = (long)(secret / 32);
    secret = Mix(secret, res);
    secret = Prune(secret);
    res = secret * 2048;
    secret = Mix(secret, res);
    secret = Prune(secret);
    return secret;
}


// Could keep a hashmap of sequences and values
Dictionary<Sequence, int> GetSequencesAndBananas(int[] differences, List<int> bananas)
{
    var res = new Dictionary<Sequence, int>();
    for (int i = 0; i <= differences.Length - 4; i++)
    {
        var currSeq = new Sequence(differences[i], differences[i + 1], differences[i + 2], differences[i + 3]);
        if (!res.ContainsKey(currSeq))
        {
            res[currSeq] = bananas[i + 4];
        }
    }

    return res;
}

long sum = 0;
int length = 2000;
var maxBananas = 0;
var diffLists = new List<int[]>();
var bananaLists = new List<List<int>>();
foreach (var n in File.ReadLines("../../../input.txt").ToList().Select(x => long.Parse(x)))
{
    var bananas = new List<int>();
    var secret = n;
    for (int i = 0; i < length; i++)
    {
        bananas.Add((int)secret%10);
        secret = NextSecret(secret);
    }
    sum += secret;
    // Console.WriteLine($"{n} : {secret}");


    var differences = new List<int>();
    
    for (int i = 0; i < length-1; i++)
    {
        differences.Add(bananas[i+1] - bananas[i]);
    }

    diffLists.Add(differences.ToArray());
    bananaLists.Add(bananas);
}


// All unique sequences
var best = 0;
var bestSeq = new Sequence(0, 0, 0, 0);
var dicts = new List<Dictionary<Sequence, int>>();
var uniqueSeqs = new HashSet<Sequence>();
for (int i = 0; i < diffLists.Count; i++)
{
    var d = GetSequencesAndBananas(diffLists[i], bananaLists[i]);
    dicts.Add(d);
    uniqueSeqs.UnionWith(d.Keys.ToHashSet());
}

//
var iters = 1;
var total = uniqueSeqs.Count;
foreach (var seq in uniqueSeqs)
{
    Console.WriteLine($"{iters} / {total} best: {best}"); // tried it once best hadn't increased in a while, lol...
    var temp = 0;
    foreach (var d in dicts)
    {
        if (d.ContainsKey(seq))
        {
            temp += d[seq];
        }
    }

    if (temp > best)
    {
        best = temp;
        bestSeq = seq;
    }

    iters++;
}


Console.WriteLine(best);
Console.WriteLine($"{bestSeq.one} {bestSeq.two} {bestSeq.three} {bestSeq.four}");