namespace Day22;

public class Sequence
{
    public int one;
    public int two;
    public int three;
    public int four;


    public Sequence(int one, int two, int three, int four)
    {
        this.one = one;
        this.two = two;
        this.three = three;
        this.four = four;
    }
        
    public override bool Equals(object obj)
    {
        var item = obj as Sequence;

        if (item == null)
        {
            return false;
        }

        return this.one == item.one && this.two == item.two && this.three == item.three && this.four == item.four;
    }

    public override int GetHashCode()
    {
        return (one * 1) + (two * 2) + (three * 3) + (four * 4);
    }
}