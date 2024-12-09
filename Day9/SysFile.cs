namespace Day9;

public class SysFile
{
    public int Id { get; set; }
    public List<int> Indices { get; set; }

    public SysFile(int id, List<int> indices)
    {
        Id = id;
        Indices = indices;
    }
}

