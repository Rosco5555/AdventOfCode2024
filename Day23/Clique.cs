namespace Day23;

public class Clique
{
    public List<string> nodes;


    public Clique(List<string> nodes)
    {
        this.nodes = nodes;
    }
    
    public override bool Equals(object obj)
    {
        var item = obj as Clique;

        if (item == null)
        {
            return false;
        }

        return  nodes.OrderBy(t => t).SequenceEqual(item.nodes.OrderBy(t => t));
    }

    public override int GetHashCode()
    {
        var ns = nodes.ToArray();
        Array.Sort(ns);
        
        return HashCode.Combine(String.Join("",ns));
    }
    
    public override string ToString()
    {
        return String.Join(", ", nodes);
    }
}