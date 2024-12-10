var map = new List<char[]>();
foreach (var line in File.ReadLines("../../../input.txt"))
{
    map.Add(line.ToCharArray());
}


int Search((int, int) pos, int prev)
{
    Console.WriteLine($"basic {pos.Item1} {pos.Item2} {prev}");
    var row = pos.Item1;
    var col = pos.Item2;

    if (row < 0 || row >= map.Count || col < 0 || col >= map[0].Length)
    {
        Console.WriteLine("AGDSGAS");
        return 0;
    }

    var c = map[row][col];


    if (map[row][col]  == '.')
    {
        Console.WriteLine("IS A DOT");
        return 0;
    }
    
    if (c-'0' == prev + 1)
    {
        if (c == '9')
        {
            return 1;
        }
        
        Console.WriteLine($"Deeper {pos.Item1} {pos.Item2}");
        var up = Search((pos.Item1 - 1, pos.Item2), c-'0');
        var down = Search((pos.Item1 + 1, pos.Item2), c-'0');
        var left = Search( (pos.Item1, pos.Item2-1), c-'0');
        var right = Search((pos.Item1, pos.Item2+1), c-'0');

        return up + down + left + right;

    }

    return 0;


}

var total = 0;

for (int i = 0; i < map.Count; i++)
{
    for (int j = 0; j < map[0].Length; j++)
    {
        var trailheads = new HashSet<(int, int)>();

        if (map[i][j] == '0')
        {
            total += Search((i, j), -1);
        }
    }
}

Console.WriteLine(total);