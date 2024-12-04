



var lines = File.ReadLines("../../../input.txt");

var rows = lines.Count();
var cols = lines.First().Length;
var grid = new char[rows,cols];
var i = 0;
foreach (string line in File.ReadLines("../../../input.txt"))
{
    for (int j = 0; j < line.Length; j++)
    {
        grid[i,j] = line[j];
    }

    i++;
}

var total = 0;


int CheckRight(int row, int col)
{
    if (col + 3 < cols)
    {
        var word =  grid[row, col].ToString() + grid[row, col + 1].ToString() + grid[row, col + 2].ToString() + grid[row, col + 3].ToString();
        return (word == "XMAS") ? 1 : 0;
    }
    
    return 0;
}

int CheckLeft(int row, int col)
{
    if (col - 3 >= 0)
    {
        var word =  grid[row, col].ToString() + grid[row, col - 1].ToString() + grid[row, col - 2].ToString() + grid[row, col - 3].ToString();
        Console.WriteLine(word);
        return (word == "XMAS") ? 1 : 0;
    }
    return 0;
}


int CheckDown(int row, int col)
{
    if (row + 3 < rows)
    {
        var word =  grid[row, col].ToString() + grid[row+1, col].ToString() + grid[row+2, col].ToString() + grid[row+3, col].ToString();
        Console.WriteLine(word);
        return (word == "XMAS") ? 1 : 0;
    }
    return 0;
}

int CheckUp(int row, int col)
{
    if (row - 3 >= 0)
    {
        var word = grid[row, col].ToString() + grid[row-1, col].ToString() + grid[row-2, col].ToString() + grid[row-3, col].ToString();
        Console.WriteLine(word);
        return (word == "XMAS") ? 1 : 0;
    }
    return 0;
}

int CheckUpLeft(int row, int col)
{
    if (row - 3 >= 0 && col - 3 >= 0)
    {
        var word = grid[row, col].ToString() + grid[row-1, col-1].ToString() + grid[row-2, col-2].ToString() + grid[row-3, col-3].ToString();
        Console.WriteLine(word);
        return (word == "XMAS") ? 1 : 0;
    }
    return 0;
}

int CheckDownLeft(int row, int col)
{

    var word = grid[row, col].ToString() + grid[row + 1, col - 1].ToString() + grid[row + 2, col - 2].ToString();
    Console.WriteLine(word);
    return (word == "MAS") || (word == "SAM") ? 1 : 0;
}

int CheckUpRight(int row, int col)
{
    if (row + 3 < rows && col + 3 < cols)
    {
        var word = grid[row, col].ToString() + grid[row+1, col+1].ToString() + grid[row+2, col+2].ToString() + grid[row+3, col+3].ToString();
        Console.WriteLine(word);
        return (word == "XMAS") ? 1 : 0;
    }
    return 0;
}

int CheckDownRight(int row, int col)
{
    var word = grid[row, col].ToString() + grid[row + 1, col + 1].ToString() + grid[row + 2, col + 2].ToString();
    Console.WriteLine(word);
    return (word == "MAS") || (word == "SAM") ? 1 : 0;
}

for (int row = 0; row < rows; row++)
{
    for (int col = 0; col < cols; col++)
    {
        // total += CheckLeft(row, col);
        // total += CheckRight(row, col);
        // total += CheckUp(row, col);
        // total += CheckDown(row, col);
        // total += CheckUpLeft(row, col);
        // total += CheckUpRight(row, col);
        // total += CheckDownLeft(row, col);
        // total += CheckDownRight(row, col);
        total += CheckForX(row, col);
    }
    Console.WriteLine();
}

Console.WriteLine(total);


int CheckForX(int row, int col)
{
    int total = 0;
    if (row - 1 >= 0 && row + 1 < rows && col - 1 >= 0 && col + 1 < cols)
    {
        total += CheckDownRight(row - 1, col - 1);
        total += CheckDownLeft(row - 1, col + 1);
    }
    return (total == 2) ? 1 : 0;
}


/*
M.S
.A.
M.S
*/