using System.Data;

var map = new List<char[]>();
foreach (var line in File.ReadLines("../../../input.txt"))
{
    map.Add(line.ToCharArray());
}

var visited = new List<(int, int)>();
var regions = new List<HashSet<(int, int)>>();
var totalPrice = 0;
for (int row = 0; row < map.Count; row++)
{
    for (int col = 0; col < map[0].Length; col++)
    {
        if (!visited.Contains((row, col)))
        {
            var region = new HashSet<(int, int)>();
            var (a,p) = RegionData(map[row][col], row, col, region);
            var walls = GetWalls(region);
            totalPrice += a * walls;
            visited.AddRange(region);
            // Console.WriteLine($"{map[row][col]} {walls}");
            regions.Add(region);
        }
    }
}

Console.WriteLine($"Price: {totalPrice}");

bool InBounds(int row, int col)
{
    return (row >= 0 && row < map.Count && col >= 0 && col < map[0].Length);
}


(int, int) RegionData(char square, int row, int col, HashSet<(int, int)> visited)
{
    var area = 0;
    var perimeter = 0;
    if (InBounds(row,col) && map[row][col] == square && !visited.Contains((row,col)))
    {
        visited.Add((row, col));
        area += 1;
        perimeter += Perimeter(square, row, col);
        
        var results = new[]{(area, perimeter), RegionData(square, row + 1, col, visited),  RegionData(square, row - 1, col, visited), RegionData(square, row, col + 1, visited), RegionData(square, row, col - 1, visited)};

        return (results.Sum(x => x.Item1), results.Sum(x => x.Item2));
    }

    return (0,0);
}

bool IsEdge(char square, (int row, int col) side)
{
    if (!InBounds(side.row, side.col))
    {
        return true;
    } 
    return map[side.row][side.col] != square;
}

int Perimeter(char square, int row, int col)
{
    var sides = new[]{(row + 1,col), (row-1,col), (row,col+1), (row,col-1)};
    return sides.Count(x =>IsEdge(square,x));
}



int GetWalls(HashSet<(int, int)> region)
{
    var square = map[region.First().Item1][region.First().Item2];
    var walls = 0;
    for(int x = 0; x < map.Count(); x++)
    { 
        // Console.WriteLine($"Sitting on line x = {line.Key}");

        var topPrev = IsEdge(square, (x-1, 0)) && (region.Contains((x-1, 0)));
        var botPrev = IsEdge(square, (x+1, 0))&& (region.Contains((x+1, 0)));

            
        var topWalls =  topPrev ? 1 : 0;
        var bottomWalls = botPrev ? 1 : 0;
        
        for(int y = 0; y < map[0].Length;y++)
        {
            if (region.Contains((x, y)))
            {
                // Console.WriteLine($"{point.Item1} {point.Item2} ");
                var top = IsEdge(square, (x-1, y));
                var bottom = IsEdge(square, (x+1, y));
                if (top && !topPrev)  // above
                {
                    topWalls++;
                }

                if (bottom && !botPrev)
                {
                    bottomWalls++;
                }
            
            
                topPrev = top;
                botPrev = bottom;
            }
            else
            {
                topPrev = false;
                botPrev = false;
            }
        }
        
        walls += topWalls + bottomWalls;
        Console.WriteLine($"{topWalls + bottomWalls} Walls on line x = {x}");
    }
    for(int y = 0; y < map[0].Length; y++)
    { 
        // Console.WriteLine($"Sitting on line x = {line.Key}")

        var leftPrev = IsEdge(square, (0, y-1)) && (region.Contains((0, y-1)));
        var rightPrev = IsEdge(square, (0, y+1))&& (region.Contains((0, y+1)));

            
        var leftWalls =  leftPrev ? 1 : 0;
        var rightWalls = rightPrev ? 1 : 0;
        
        for(int x = 0; x < map[0].Length;x++)
        {
            if (region.Contains((x, y)))
            {
                // Console.WriteLine($"{point.Item1} {point.Item2} ");
                var left = IsEdge(square, (x, y-1));
                var right = IsEdge(square, (x, y+1));
                if (left && !leftPrev)  // above
                {
                    leftWalls++;
                }

                if (right && !rightPrev)
                {
                    rightWalls++;
                }
            
            
                rightPrev = right;
                leftPrev = left;
            }
            else
            {
                leftPrev = false;
                rightPrev = false;
            }
        }
        
        walls += leftWalls + rightWalls;
        Console.WriteLine($"{leftWalls + rightWalls} Walls on line y = {y}");
    }
    Console.WriteLine($"Walls {walls}");
return walls;
} 


