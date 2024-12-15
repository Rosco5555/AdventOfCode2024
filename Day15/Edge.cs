namespace Day15;

public class Edge
{
    public (int, int) newPos;
    public char c;

    public Edge((int, int) newPos, char c)
    {
        this.newPos = newPos;
        this.c = c;
    }
}