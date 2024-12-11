namespace Day11;

public class Args
{
    public int n;
    public char[] stone;
    public int blinks;

    public Args(char[] stone, int n, int blinks)
    {
        this.stone = stone;
        this.n = n;
        this.blinks = blinks;
    }

    
    public override bool Equals(Object? other)
    {
        if (other is null) return false;
        if (other.GetType() != typeof(Args)) return false;
        return n == ((Args)other).n && stone.SequenceEqual(((Args)other).stone) && blinks == ((Args)other).blinks;
    }

    public override int GetHashCode()
    {
        var sum = stone.ToList().Sum(x=>x);
        return HashCode.Combine(n, sum, blinks);
    }
}