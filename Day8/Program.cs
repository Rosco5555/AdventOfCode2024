using System.Text.RegularExpressions;

var map = new List<char[]>();

var bigStr = "";
foreach (string line in File.ReadLines("../../../input.txt"))
{
    map.Add(line.ToCharArray());
    bigStr += line;
}

List<(int, int)> FindAll(char c, List<char[]> map)
{
    var positions = new List<(int, int)>();
    for (int i = 0; i < map.Count; i++)
    {
        for (int j = 0; j < map[0].Length; j++)
        {
            if (map[i][j] == c)
            {
                Console.WriteLine($"{i} {j}");
                positions.Add((i,j));
            }
        }
    }

    return positions;
}

(int, int) Diff((int, int) p1, (int, int) p2)
{
    return (p2.Item1 - p1.Item1, p2.Item2 - p1.Item2);
}

bool InBounds((int, int) pos)
{
    return (pos.Item1 >= 0 && pos.Item1 < map.Count && pos.Item2 >= 0 && pos.Item2 < map[0].Length);
}

(int, int) Transform((int, int) pos)
{
    if (pos.Item1 < 0)
    {
        pos.Item1 = map.Count + pos.Item1;
    }

    if (pos.Item1 >= map.Count)
    {
        pos.Item1 = pos.Item1 - map.Count;
    }
    
    if (pos.Item2 < 0)
    {
        pos.Item2 = map[0].Length + pos.Item2;
    }

    if (pos.Item2 >= map[0].Length)
    {
        pos.Item2 = pos.Item2 - map[0].Length;
    }

    return pos;
}

void CheckAntinode(HashSet<(int,int)> antinodes, (int,int) pos, (int, int) diff)
{

    var possibleAntinode = Add(pos, diff);
    if (InBounds(possibleAntinode))
    {
        antinodes.Add((possibleAntinode.Item1, possibleAntinode.Item2));
        // Console.WriteLine($"Valid antinode: {possibleAntinode.Item1} {possibleAntinode.Item2}");
        CheckAntinode(antinodes, possibleAntinode, diff);
    }
    
}

(int, int) Add((int,int) pos1, (int,int) pos2)
{
    return (pos1.Item1 + pos2.Item1, pos1.Item2 + pos2.Item2);
}

int total = 0;
void GetDiffs(List<(int, int)> positions, HashSet<(int,int)> antinodes)
{
    for (int i = 0; i < positions.Count; i++)
    {
        for (int j = i+1; j < positions.Count; j++)
        {
            var distFromIToJ = Diff(positions[i], positions[j]);
            CheckAntinode(antinodes, positions[j], distFromIToJ);
            CheckAntinode(antinodes, positions[i], distFromIToJ);
            
            var distFromJToI = Diff(positions[j], positions[i]);
            CheckAntinode(antinodes, positions[i], distFromJToI);
            CheckAntinode(antinodes, positions[j], distFromJToI);
        }
    }
}

var distinctChars = new String(bigStr.Distinct().ToArray());

var antinodes = new HashSet<(int, int)>();
foreach(var c in distinctChars){
    if (c != '.')
    {
        var positions = FindAll(c, map);
        GetDiffs(positions, antinodes);
    }
}

foreach (var antinode in antinodes)
{
    Console.WriteLine($"{antinode.Item1} {antinode.Item2}");
}
Console.WriteLine($" ans {antinodes.Count()}");