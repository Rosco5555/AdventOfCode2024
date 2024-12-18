using System.Reflection.Metadata.Ecma335;
using Day6;

int N = 71;
var map = new List<char[]>();
var movements = "";
int row = 0;
var start = (0, 0);
var end = (N-1, N-1);
for (var k = 0; k < N; k++)
{
    var ls = new char[N];
    for (var j = 0; j < N; j++)
    {
        ls[j] = '.';
    }

    map.Add(ls);
}
int i = 0;
var lines = File.ReadLines("../../../input.txt");
var b = 0;
foreach (var line in lines)
{
   
    var ns = line.Split(",");
    map[int.Parse(ns[1])][int.Parse(ns[0])] = '#';

    var soluble = Search();

    if (!soluble)
    {
        Console.WriteLine($"{ns[0]}, {ns[1]}");
        break;
    }

    Console.WriteLine($"bye: {b}");
    b++;
}

Console.WriteLine("READ");
bool InBounds((int, int) cell)
{
    var row = cell.Item1;
    var col = cell.Item2;
    return row >= 0 && row < map.Count && col >= 0 && col < map[0].Length;
}

bool NotWall((int, int) cell)
{
    var x = cell.Item1;
    var y = cell.Item2;
    return map[x][y] != '#';
}


List<(Position, int)> Neigbhours(Position cell, List<Position> unvisited)
{
    var row = cell.row;
    var col = cell.col;
    
    var p1 = (new Position(row, col - 1), 0);
    var p2 = (new Position(row, col + 1), 0);
    var p3 = (new Position(row - 1, col), 0);
    var p4 = (new Position(row + 1, col), 0);
    var positions = new [] { p1, p2, p3, p4 };
    var validPositions = positions.Where(p =>
        InBounds((p.Item1.row, p.Item1.col)) && NotWall((p.Item1.row, p.Item1.col))).ToList();
    return validPositions;
}

var dist = 0;

bool Search()
{
    var costs = new List<int[]>();
    var dists = new Dictionary<Position, int>();
    var prev = new Dictionary<Position?, HashSet<Position>>();
    
        
    for (int x = 0; x < map.Count; x++)
    {
        var costRow = new int[map[0].Length];
        for (int y = 0; y < map[0].Length; y++)
        {
            var p = new Position(x, y);
            if (map[x][y] != '#')
            {
                // unvisited.Add(new Position(x,y,Direction.RIGHT));
                prev[p] = new HashSet<Position>();
            }

            if (x == start.Item1 && y == start.Item2)
            {
                dists[new Position(x, y)] = 0;
                costRow[y] = 0;
            }
            else
            {
                dists[new Position(x, y)] = 99999;
                costRow[y] = 9999999;
            }
        }

        costs.Add(costRow);
    }
    
    
    Position PosWithLowestDist(List<Position> unvisited)
    {

        var poses = unvisited.ToList();
        var min = costs[poses[0].row][poses[0].col];
        var minPos = poses[0];
        foreach (var pos in poses)
        {
            if (dists.ContainsKey(pos))
            {
                var temp = dists[pos];
                if (temp < min)
                {
                    min = temp;
                    minPos = pos;
                }
            }
        }

        return minPos;
    }

    var facingMap = new List<char[]>();
    for (var x = 0; x < map.Count; x++)
    {
        var l = new char[map[0].Length];
        for (var y = 0; y < map[0].Length; y++)
        {
            l[y] = map[x][y];
        }
        facingMap.Add(l);
    }
    
    var unvisited = new List<Position>(){};
    var visited = new List<Position>();
    unvisited.Add(new Position(start.Item1, start.Item2));
    while (unvisited.Any())
    {
        var cell = PosWithLowestDist(unvisited);
        unvisited.Remove(cell);

        if (!visited.Contains(cell))
        {
            visited.Add(cell);
            if (cell.row == end.Item1 && cell.col == end.Item2)
            {
                // Console.WriteLine(dists[cell]);
                return true;
            }

            var ns = Neigbhours(cell, unvisited.ToList());

     
            foreach (var n in ns)
            {
                if (!dists.ContainsKey(n.Item1))
                {
                    dists[n.Item1] = 99999;
                }
            
                var alt = dists[cell]+n.Item2+1;
    
                // If you have to do a turn to get to next square
                if (alt <= dists[n.Item1])
                {
                    unvisited.Add(n.Item1);
                    if (alt <dists[n.Item1])
                    {
                        prev[n.Item1] = new HashSet<Position>();
                    }
                    dists[n.Item1] = alt;
                    prev[n.Item1].Add(cell);
                }
        
            }
        }
    
        
    
       
    }

    return false;
}


List<List<Position?>> GetPathRecur(Position? node, Dictionary<Position?, HashSet<Position>> prev)
{
    var visited = new HashSet<List<Position?>>();
    var queue = new Queue<List<Position?>>();
    var results = new List<List<Position?>>();
    queue.Enqueue([node]);

    while (queue.Any())
    {
        var path = queue.Dequeue();
        var v = path.Last();
        
        
        if (!prev[v].Any())
        {
            results.Add(path);
            continue;
        } else if (!visited.Contains(path))
        {
            foreach(var n in prev[v])
            {
                var newPath = new List<Position?>(path);
                newPath.Add(n);
                queue.Enqueue(newPath);
            }

            visited.Add(path);
        }
    }

    return results;
}