

using Helpers;

var grid = new Grid("../../../input.txt");

grid.Print();

bool Safe(List<int> row)
{
    bool increasing = false;
    bool decreasing = false;
    
    for (int i = 1; i < row.Count; i++)
    {
        
        var diff = Math.Abs(row[i - 1] - row[i]);
        
        if (diff is < 1 or > 3)
        {
            return false;
        }
        
      
        if (row[i - 1] > row[i])
        {
            if (decreasing)
            {
                return false;
            }

            increasing = true;
        } else if (row[i - 1] < row[i])
        {
            if (increasing)
            {
                return false;
            }

            decreasing = true;
        }
        else
        {
            return false;
        }
        
    }

    return true;
}

bool helper(List<int> row)
{
    for(var i = 0; i < row.Count(); i++)
    {
        var newList = new List<int>(row);
        newList.RemoveAt(i);
        if (Safe(newList))
        {
            return true;
        }
    }

    return false;
}

int safe = 0;
foreach (var row in grid._cells)
{
    if (helper(row))
    {
        safe++;
    }
}

Console.WriteLine(safe);