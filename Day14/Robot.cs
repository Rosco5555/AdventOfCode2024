namespace Day14;


public class Robot
{
    public static int maxX = 101;
    public static int maxY = 103;
    public static int seconds =1000000000;
    public (long, long) pos;
    public (long, long) velocity;

    public Robot((long, long) pos, (long, long) velocity)
    {
        this.pos = pos;
        this.velocity = velocity;
    }

    public void Move()
    {
        
        pos = (mod(pos.Item1+velocity.Item1,maxX), mod(pos.Item2+velocity.Item2,maxY));
    
    }
    
    private long mod(long a, long n)
    {
        long result = a % n;
        if ((result<0 && n>0) || (result>0 && n<0)) {
            result += n;
        }
        return result;
    }
}