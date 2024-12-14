using System.Text.RegularExpressions;
using Day14;

var middleX = (Robot.maxX - 1) / 2;
var middleY = (Robot.maxY - 1) / 2;
var q1 = new int[]{-1,middleX, -1, middleY}; // startX, endX, startY, endY
var q3 = new int[]{-1,middleX, middleY, Robot.maxY};
var q2 = new int[]{middleX, Robot.maxX, -1, middleY};
var q4 = new int[] { middleX, Robot.maxX, middleY, Robot.maxY};

var quadrants = new List<int[]> { q1, q2, q3, q4 };

var robots = new List<Robot>();

foreach (var line in File.ReadLines("../../../input.txt"))
{
    string pattern = @"(-*\d+)+";
    var matches = Regex.Matches(line, pattern);

    var x = long.Parse(matches[0].Value);
    var y = long.Parse(matches[1].Value);
    
    var vx = long.Parse(matches[2].Value);
    var vy = long.Parse(matches[3].Value);

    robots.Add(new Robot((x,y),(vx,vy)));
}

bool InQuad(int[] q, (long, long) pos)
{
    var x = pos.Item1;
    var y = pos.Item2;

    return (x > q[0] && x < q[1] && y > q[2] && y < q[3]);
}

for (int i = 0; i < Robot.seconds; i++)
{
    foreach (var robot in robots)
    {
        robot.Move();
    }
    if (PrintRobots(i))
    {
        break;
    }
}

var counts = new Dictionary<int[], long>();
foreach (var q in quadrants)
{
    counts[q] = 0;
}


foreach (var robot in robots)
{
    var pos = robot.pos;
    var quadNo = 1;
    foreach (var q in quadrants)
    {
        if (InQuad(q, pos))
        {
            // Console.WriteLine($"Pos: {pos.Item1} {pos.Item2} in {quadNo}");
            counts[q] += 1;
        }

        quadNo++;
    }
}

long total = 1;
foreach (var (quad, count) in counts)
{
    total *= count;
}



bool PrintRobots(int seconds)
{
    var grid = new char[Robot.maxY,Robot.maxX];

    for (int i = 0; i < Robot.maxY; i++)
    {
        for (int j = 0; j < Robot.maxX; j++)
        {
            grid[i,j] = ' ';
        }
    }

    foreach (var robot in robots)
    {
        var x = robot.pos.Item1;
        var y = robot.pos.Item2;


        grid[y,x] = '*';
    }

    var lines =  new List<string>();
    var goodLines = 0;
    for (int i = 0; i < Robot.maxY; i++)
    {
        var s = "";
        for (int j = 0; j < Robot.maxX; j++)
        {
            s += grid[i,j];
        }
        lines.Add(s);
        if (s.Contains("***********"))
        {
            goodLines++;
        }
    }

    if (goodLines > 0)
    {
        Console.WriteLine($"---- {seconds+1} SECONDS ------");
        foreach (var line in lines)
        {
            Console.WriteLine(line);
        }
        return true;
    }

    return false;
}

Console.WriteLine(total);