using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using Day6;

var map = new List<char[]>();
int row = 0;
var start = (0, 0);
var end = (0, 0);


int i = 0;
foreach (var line in File.ReadLines("../../../input.txt"))
{
    map.Add(line.ToCharArray());
    Console.WriteLine(line);
    for (int j = 0; j < line.Length; j++)
    {
        if (line[j] == 'S')
        {
            start = (i, j);
        }
        if (line[j] == 'E')
        {
            end = (i, j);
        }
    }

    i++;
}

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


List<Position> Neigbhours(Position cell, bool cheating = false)
{
    var row = cell.row;
    var col = cell.col;

    var p1 = new Position(row, col - 1);
    var p2 = new Position(row, col + 1);
    var p3 = new Position(row - 1, col);
    var p4 = new Position(row + 1, col);
    var positions = new [] { p1, p2, p3, p4 };
    if (!cheating)
    {
        var validPositions = positions.Where(p =>
            InBounds((p.row, p.col)) && NotWall((p.row, p.col))).ToList();
        return validPositions;
    }
    else
    {
        var validPositions = positions.Where(p =>
            InBounds((p.row, p.col))).ToList();
        return validPositions;
    }
}

var dist = 0;
var dists = new Dictionary<Position, int>();
List<List<Position?>> Search()
{
    var prev = new Dictionary<Position?, HashSet<Position>>();
        
    for (int x = 0; x < map.Count; x++)
    {
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
            }
            else
            {
                dists[new Position(x, y)] = 99999;
            }
        }
    }
    
    
    Position PosWithLowestDist(List<Position> unvisited)
    {

        var poses = unvisited.ToList();
        var min = dists[poses[0]];
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
                Console.WriteLine(dists[cell]);
                return GetPathRecur(cell, prev);
            }

            var ns = Neigbhours(cell);

     
            foreach (var n in ns)
            {
                if (!dists.ContainsKey(n))
                {
                    dists[n] = 99999;
                }
            
                var alt = dists[cell]+1;
    
                // If you have to do a turn to get to next square
                if (alt <= dists[n])
                {
                    unvisited.Add(n);
                    if (alt <dists[n])
                    {
                        prev[n] = new HashSet<Position>();
                    }
                    dists[n] = alt;
                    prev[n].Add(cell);
                }
        
            }
        }
    
        
    
       
    }

    return null;
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


int ManhattanDistance(Position p1, Position p2)
{
    return Math.Abs(p1.row - p2.row) + Math.Abs(p1.col - p2.col);
}

var cheats = new Dictionary<int, int>();
int NumDistNAwayThatSaveAtLeastM(Position p, List<Position?> path, Position nextPos, int n, int m)
{
    var ans = 0;
    var cheatPaths = new HashSet<Cheat>();
    var distToEnd = dists[new Position(end.Item1, end.Item2)];
    var d1 = distToEnd - dists[p];
    foreach (var pos in path)
    {
        var d2 = distToEnd - dists[pos];
        var md = ManhattanDistance(pos, p);
        var cheatPath = new Cheat((p.row, p.col), (pos.row, pos.col));
        if (md <= n && pos != nextPos && !cheatPaths.Contains(cheatPath))
        {
            var saved = d1 - d2-md;
            if (saved >= m)
            {
                // Console.WriteLine($"d1: {d1} d2: {d2} saved: {saved} start: ({p.row},{p.col}) end: ({pos.row}, {pos.col})");
                if (cheats.ContainsKey(saved))
                {
                    cheats[saved] += 1;
                }
                else
                {
                    cheats[saved] = 1;
                }
                ans++;
                cheatPaths.Add(cheatPath);
            }
        }
    }

    return ans;
}


var res = Search();
var path = res[0];
path.Reverse();
foreach (var pos in path)
{
    Console.Write($"({pos.row} , {pos.col}) ");
    map[pos.row][pos.col] = 'X';
}
Console.WriteLine();
foreach (var r in map)
{
    Console.WriteLine(String.Join("",r));
}

// SC is something that moves you further ahead in path...
var total = 0;
// var poses = new Position[]{new Position(1, 7)};
for(int x = 0; x < path.Count-1; x++)
{
    var pos = path[x];
    var nextPos = path[x + 1];
    total+= NumDistNAwayThatSaveAtLeastM(pos, path, nextPos, 20,100);
}

foreach (var (k, v) in cheats.OrderBy(x=>x.Key))
{
    Console.WriteLine($"There are {v} cheats that save {k} picoseconds.");
}

Console.WriteLine(total);


