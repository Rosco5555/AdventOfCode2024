namespace Day6;

public class Cheat
{
    public (int, int) start;
    public (int, int) end;

    public Cheat((int, int) start, (int, int) end)
    {
        this.start = start;
        this.end = end;
    }
    
    public override bool Equals(object obj)
    {
        var item = obj as Cheat;

        if (item == null)
        {
            return false;
        }

        return this.start == item.start && this.end == item.end;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(start, end);
    }
}