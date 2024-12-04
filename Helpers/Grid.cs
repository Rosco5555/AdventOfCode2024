namespace Helpers;

public class Grid
{
    public readonly List<List<int>> _cells;

    public Grid(string filePath)
    {
        _cells = new List<List<int>>();
        int i = 0;
        foreach (string line in File.ReadLines(filePath))
        {
            var row = line.Split(" ").Select(int.Parse).ToList();
            _cells.Add(row);
        }
    }

    public int Get(int row, int col)
    {
        return _cells[row][col];
    }


    public void Print()
    {
        foreach (var row in _cells)
        {
            Console.WriteLine(String.Join(" ", row));
        }
    }
}