using Day15;
using System;
using System.Collections;

var map = new List<char[]>();
var movements = "";
var robotPos = (0, 0);
int row = 0;

foreach (var line in File.ReadLines("../../../input.txt"))
{
    if (!string.IsNullOrEmpty(line) && line.Contains("#")) {
        map.Add(line.ToCharArray());
        var col = 0;
        foreach (var c in line)
        {
            if (c == '@')
            {
                robotPos = (row, col);
            }

            col++;
        }
        Console.WriteLine(line);
    }
    else
    {
        movements += line;
    }

    row++;
}

(int, int) GetMove((int row, int col) pos, char move)
{
    switch (move)
    {
        case '^':
            return (pos.row - 1, pos.col);
        case 'v':
            return (pos.row + 1, pos.col);
        case '<':
            return (pos.row,  pos.col-1);
        case '>':
            return (pos.row,  pos.col+1);
    }

    return (0, 0);
}

// bool PushBox(List<char[]> map, (int row, int col) pos, char direction)
// {
//     char c = map[pos.row][pos.col];
//     var boxes = new List<(int, int)>();
//     while (c is '[' or ']')
//     {
//         boxes.Add(pos);
//         pos = GetMove(pos,direction);
//         c = map[pos.row][pos.col];
//     }
//     
//     
//     if (c == '.')
//     {
//         map[boxes[0].Item1][boxes[0].Item2] = '@';
//         switch (direction)
//         {
//             case '<':
//                 map[pos.Item1][pos.Item2] = '[';
//                 break;
//             case '>':
//                 map[pos.Item1][pos.Item2] = ']';
//                 break;
//                 
//         }
//         
//         map[boxes[0].Item1][boxes[0].Item2] = '@';
//         map[pos.Item1][pos.Item2] = 'O';
//         return true;
//     }
//
//     return false;
// }



// for(int i = 0; i < map.Count; i++)
// {
//     for(var j = 0; j < map[0].Length; j++)
//     {
//         if (map[i][j] == 'O')
//         {
//             total += (100 * i) + j;
//         }
//     }
// }

List<char[]> ScaleUp(List<char[]> map)
{
    var newMap = new List<char[]>();

    foreach (var row in map)
    {
        var newRow = "";
        foreach (var c in row)
        {
            if (c == '#')
            {
                newRow += "##";
            }

            if (c == 'O')
            {
                newRow += "[]";
            }
            if(c == '.')
            {
                newRow += "..";
            }

            if (c == '@')
            {
                newRow += "@.";
            }
        }
        newMap.Add(newRow.ToCharArray());
    }

    return newMap;
}

var newMap = ScaleUp(map);

foreach (var line in newMap)
{
    Console.WriteLine(line);
}

robotPos = (robotPos.Item1, robotPos.Item2*2);


// Assumes you can in fact move it up
bool CanMoveBigBoxes(List<char[]> map, (int row, int col) pos, char direction, List<(int,int)> oldEdges, List<Edge> newEdges)
{
    var c = map[pos.row][pos.col];
    switch (direction)
    {
        case '^':
            if (c == '[')
            {
                newEdges.Add(new Edge(pos, c));
                newEdges.Add(new Edge((pos.Item1, pos.Item2+1), ']'));
                // Wall above either slot
                if (map[pos.row-1][pos.col] == '#' || map[pos.row-1][pos.col + 1] == '#')
                {
                    return false;
                }
                // Free space above
                if (map[pos.row-1][pos.col] == '.' && map[pos.row-1][pos.col + 1] == '.')
                {
                    return true;
                }
                
                return CanMoveBigBoxes(map, (pos.row-1,pos.col), direction, oldEdges, newEdges) && CanMoveBigBoxes(map, (pos.row-1,pos.col+1), direction, oldEdges, newEdges);

            } if (c == ']')
            {
                newEdges.Add(new Edge(pos, c)); 
                newEdges.Add(new Edge((pos.Item1, pos.Item2-1), '['));
                if (map[pos.row-1][pos.col] == '#' || map[pos.row-1][pos.col - 1] == '#')
                {
                    return false;
                }
                if (map[pos.row-1][pos.col] == '.' && map[pos.row-1][pos.col - 1] == '.')
                {
                    return true;
                }
                
                return CanMoveBigBoxes(map, (pos.row-1,pos.col), direction, oldEdges, newEdges) && CanMoveBigBoxes(map, (pos.row-1,pos.col-1), direction, oldEdges, newEdges);
            }

            if (c == '.')
            {
                return true;
            }

            break;
        
        case 'v':
            if (c == '[')
            {
                newEdges.Add(new Edge(pos, c));
                newEdges.Add(new Edge((pos.Item1, pos.Item2+1), ']'));
                if (map[pos.row+1][pos.col] == '#' || map[pos.row+1][pos.col + 1] == '#')
                {
                    return false;
                }
                
                if (map[pos.row+1][pos.col] == '.' && map[pos.row+1][pos.col + 1] == '.')
                {
                    return true;
                }
                
                return CanMoveBigBoxes(map, (pos.row+1,pos.col), direction, oldEdges, newEdges) && CanMoveBigBoxes(map, (pos.row+1,pos.col+1), direction, oldEdges, newEdges);
            }

            if (c == ']')
            {
                newEdges.Add(new Edge(pos, c));
                newEdges.Add(new Edge((pos.Item1, pos.Item2-1), '['));
                if (map[pos.row+1][pos.col] == '#' || map[pos.row+1][pos.col - 1] == '#')
                {
                    return false;
                }
                
                if (map[pos.row+1][pos.col] == '.' && map[pos.row+1][pos.col - 1] == '.')
                {
                    return true;
                }
                
                return CanMoveBigBoxes(map, (pos.row+1,pos.col), direction, oldEdges, newEdges) && CanMoveBigBoxes(map, (pos.row+1,pos.col-1), direction, oldEdges, newEdges);
            }

            if (c == '.')
            {
                return true;
            }

            break;
        
        case '<':
            if (c == '[')
            {
                newEdges.Add(new Edge(pos, c));
                newEdges.Add(new Edge((pos.Item1, pos.Item2+1), ']'));
                if (map[pos.row][pos.col - 1] == '#')
                {
                    return false;
                }
                
                if (map[pos.row][pos.col - 1] == '.')
                {
                    return true;
                }
                
                return CanMoveBigBoxes(map, (pos.row, pos.col - 1), direction, oldEdges, newEdges);
            }

            if (c == ']')
            {
                newEdges.Add(new Edge(pos, c));
                newEdges.Add(new Edge((pos.Item1, pos.Item2-1), '['));
                if (map[pos.row][pos.col - 2] == '#')
                {
                    return false;
                }
                
                if (map[pos.row][pos.col - 2] == '.')
                {
                    return true;
                }
                return CanMoveBigBoxes(map, (pos.row, pos.col - 2), direction, oldEdges, newEdges);
            }

            if (c == '.')
            {
                return true;
            }

            break;
        case '>':
            if (c == ']')
            {
                newEdges.Add(new Edge(pos, c));
                newEdges.Add(new Edge((pos.Item1, pos.Item2-1), '['));
                if (map[pos.row][pos.col + 1] == '#')
                {
                    return false;
                }
                
                if (map[pos.row][pos.col + 1] == '.')
                {
                    return true;
                }
                
                return CanMoveBigBoxes(map, (pos.row, pos.col + 1), direction, oldEdges, newEdges);
            }

            if (c == '[')
            {
                newEdges.Add(new Edge(pos, c));
                newEdges.Add(new Edge((pos.Item1, pos.Item2+1), ']'));
                if (map[pos.row][pos.col + 2] == '#')
                {
                    return false;
                }
                
                if (map[pos.row][pos.col + 2] == '.')
                {
                    return true;
                }
                return CanMoveBigBoxes(map, (pos.row, pos.col + 2), direction, oldEdges, newEdges);
            }

            if (c == '.')
            {
                return true;
            }

            break;
    }

    return false;
}

void PrintState(char move, List<char[]> map)
{
    Console.WriteLine();
    Console.WriteLine($"Move {move}:");
    foreach (var line in map)
    {
        Console.WriteLine(String.Join("", line));
    }
}

string ShowString(char move, List<char[]> map)
{
    var s = $"Move {move}: \n";
    foreach (var line in map)
    {
        s += String.Join("", line) + "\n";
    }

    return s;
}

void MoveBoxesBig(List<char[]> map, List<(int,int)> oldEdges, List<Edge> newEdges, char direction)
{
    foreach (var e in newEdges)
    {
        var row = e.newPos.Item1;
        var col = e.newPos.Item2;
        map[row][col] = '.';
    }
    foreach (var e in newEdges)
    {
        var row = e.newPos.Item1;
        var col = e.newPos.Item2;
        switch (direction)
        {
            case '^':
                map[row - 1][col] = e.c;
                break;
            case 'v':
                map[row + 1][col] = e.c;
                break;
            case '<':
                map[row][col-1] = e.c;
                break;
            case '>':
                map[row][col+1] = e.c;
                break;
        }
        
    }
    
}

foreach (var move in movements)
{
    var squareMovingTo = GetMove(robotPos, move);
    var c = newMap[squareMovingTo.Item1][squareMovingTo.Item2];
    if (c == ']' || c == '[')
    {
        var oldEdges = new List<(int,int)>();
        var newEdges = new List<Edge>();
        if (CanMoveBigBoxes(newMap,(squareMovingTo.Item1, squareMovingTo.Item2), move, oldEdges, newEdges))
        {
            MoveBoxesBig(newMap, oldEdges, newEdges.Distinct().ToList(), move);
            newMap[robotPos.Item1][robotPos.Item2] = '.';
            newMap[squareMovingTo.Item1][squareMovingTo.Item2] = '@';
            robotPos = squareMovingTo;
        }
    } 
    if (c == '.')
    {
        newMap[robotPos.Item1][robotPos.Item2] = '.';
        newMap[squareMovingTo.Item1][squareMovingTo.Item2] = '@';
        robotPos = squareMovingTo;
    }
    // Console.WriteLine(ShowString(move, newMap));
}
//
//
var total = 0;
for(int i = 0; i < newMap.Count; i++){
    for (int j = 0; j < newMap[0].Length; j++)
    {
        if (newMap[i][j] == '[')
        {
            var rightIndex = j+1;
            var d1 = (newMap[0].Length -1) - rightIndex; // From right hand side to right edge
            var k = Math.Min(j,d1);


            total = total + (i * 100) + j;



        }
    }
}

/*
 * ####################
   ##[]...[].....[][]##
   ##[]...........[].##
   ##..........[]..[]##
   ##.....@...[].....##
   ##..##[]...[].....##
   ##...[]...[]..[][]##
   ##.....[].[]....[]##
   ##[]...........[].##
   ####################
   
 */


// var testMapStr = """
//               ####################
//               ##[]...[].....[][]##
//               ##[]...........[].##
//               ##..........[]..[]##
//               ##......@..[].....##
//               ##..##[][]..[]....##
//               ##...[]...[]..[][]##
//               ##.....[].[]....[]##
//               ##[]...........[].##
//               ####################
//             """;
//
// foreach(var line in testMapStr){
//  Console.WriteLine(line);
// }
Console.WriteLine(total);

