namespace Day6;

public class Position
{
    public int row;
    public int col;
    public int dist;
    
    public Position(int row, int col)
    {
        this.row = row;
        this.col = col;
        this.dist = dist;
    }

    public override bool Equals(object obj)
    {
        var item = obj as Position;

        if (item == null)
        {
            return false;
        }

        return this.row == item.row && this.col == item.col;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(row, col);
    }
}