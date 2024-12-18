namespace Day6;

public class Position
{
    public int row;
    public int col;
    public Direction facing;
    public int dist;
    
    public Position(int row, int col, Direction facing)
    {
        this.row = row;
        this.col = col;
        this.facing = facing;
        this.dist = dist;
    }

    public override bool Equals(object obj)
    {
        var item = obj as Position;

        if (item == null)
        {
            return false;
        }

        return this.row == item.row && this.col == item.col && this.facing == item.facing;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(row, col, facing);
    }
}