using System.Reflection.Metadata.Ecma335;
using Day6;

var map = new List<char[]>();
var movements = "";
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


List<(Position, int)> Neigbhours(Position cell, List<Position> unvisited, Direction facing)
{
    var row = cell.row;
    var col = cell.col;
    // var facing = cell.facing;
    
    switch (facing)
    {
        case Direction.UP:
        {
            var p1 = (new Position(row, col - 1, Direction.LEFT), 1000);
            var p2 = (new Position(row, col + 1, Direction.RIGHT), 1000);
            var p3 = (new Position(row - 1, col, Direction.UP), 0);
            var p4 = (new Position(row + 1, col, Direction.DOWN), 2000);
            var positions = new (Position, int)[] { p1, p2, p3 };
            var validPositions = positions.Where(p =>
                InBounds((p.Item1.row, p.Item1.col)) && NotWall((p.Item1.row, p.Item1.col))).ToList();
            return validPositions;
        }
        case Direction.DOWN:
        {
            var p1 = (new Position(row, col - 1, Direction.LEFT), 1000);
            var p2 = (new Position(row, col + 1, Direction.RIGHT), 1000);
            var p3 = (new Position(row + 1, col, Direction.DOWN), 0);
            var p4 = (new Position(row - 1, col, Direction.UP), 2000);
            var positions = new (Position, int)[] { p1, p2, p3 };
            var validPositions = positions.Where(p =>
                InBounds((p.Item1.row, p.Item1.col)) && NotWall((p.Item1.row, p.Item1.col))).ToList();
    
            return validPositions;
        }
        case Direction.LEFT:
        {
            var p1 = (new Position(row-1, col, Direction.UP), 1000);
            var p2 = (new Position(row+1, col, Direction.DOWN), 1000);
            var p3 = (new Position(row, col-1, Direction.LEFT), 0);
            var p4 = (new Position(row, col+1, Direction.RIGHT), 2000);
            var positions = new (Position, int)[] { p1, p2, p3};
            var validPositions = positions.Where(p =>
                InBounds((p.Item1.row, p.Item1.col)) && NotWall((p.Item1.row, p.Item1.col))).ToList();
    
            return validPositions;
        }
        case Direction.RIGHT:
        {
            var p1 = (new Position(row-1, col, Direction.UP), 1000);
            var p2 = (new Position(row+1, col, Direction.DOWN), 1000);
            var p3 = (new Position(row, col+1, Direction.RIGHT), 0);
            var p4 = (new Position(row, col-1, Direction.LEFT), 2000);
            var positions = new (Position, int)[] { p1, p2, p3 };
            var validPositions = positions.Where(p =>
                InBounds((p.Item1.row, p.Item1.col)) && NotWall((p.Item1.row, p.Item1.col))).ToList();
    
            return validPositions;
        }
    }

    return [];
}

var a = new Position(5, 1, Direction.UP);
var snipe = new Dictionary<Position, int>();
snipe[a] = 1;

Console.WriteLine(snipe.ContainsKey(new Position(5, 1, Direction.UP)));

var dist = 0;
var facing = Direction.RIGHT;

List<List<Position?>> Search()
{
    var facing = Direction.RIGHT;
    var costs = new List<int[]>();
    var dists = new Dictionary<Position, int>();
    var prev = new Dictionary<Position?, HashSet<Position>>();
    
        
    for (int x = 0; x < map.Count; x++)
    {
        var costRow = new int[map[0].Length];
        for (int y = 0; y < map[0].Length; y++)
        {
            var p = new Position(x, y, Direction.RIGHT);
            if (map[x][y] != '#')
            {
                // unvisited.Add(new Position(x,y,Direction.RIGHT));
                prev[p] = new HashSet<Position>();
            }

            if (x == start.Item1 && y == start.Item2)
            {
                dists[new Position(x, y, Direction.RIGHT)] = 0;
                costRow[y] = 0;
            }
            else
            {
                dists[new Position(x, y, Direction.RIGHT)] = 99999;
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
    unvisited.Add(new Position(start.Item1, start.Item2, Direction.RIGHT));
    while (unvisited.Any())
    {
        var cell = PosWithLowestDist(unvisited);
        facing = cell.facing;
        unvisited.Remove(cell);
    
        if (cell.row == end.Item1 && cell.col == end.Item2)
        {
            Console.WriteLine(dists[cell]);
            return GetPathRecur(cell, prev);
        }
        switch (facing)
        {
            case Direction.UP:
                facingMap[cell.row][cell.col] = '^';
                break;
            case Direction.RIGHT:
                facingMap[cell.row][cell.col] = '>';
                break;
            case Direction.LEFT:
                facingMap[cell.row][cell.col] = '<';
                break;
            case Direction.DOWN:
                facingMap[cell.row][cell.col] = 'V';
                break;
        }

        var ns = Neigbhours(cell, unvisited.ToList(), cell.facing);

     
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

    foreach (var (k, v) in prev)
    {
        Console.WriteLine($"Node: {k} Prevs: {String.Join(", ", v)}");
    }
    Console.WriteLine(prev.Keys.Count());
    
    foreach (var l in costs)
    {
        foreach (int c in l)
        {
            if (c == 9999999)
            {
                Console.Write("[####]");
            }
            else
            {
                var spaces = 3-(c/10);
                var s = $"[{c}";
                for (var i = 0; i < spaces; i++)
                {
                    s += " ";
                }
                Console.Write(s + "]");
            }
        }

        Console.WriteLine();
    }
    
    foreach (var l in facingMap)
    {
        foreach (var c in l)
        {
            Console.Write(c);
        }
    
        Console.WriteLine();
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


var r = Search();
var uniques = new HashSet<(int, int)?>();
foreach (var path in r)
{
    Console.WriteLine();
    foreach (var n in path)
    {
        uniques.Add((n.row, n.col));
    }
}

Console.WriteLine(uniques.Count());
;