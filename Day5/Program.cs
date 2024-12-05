var rules = new Dictionary<string, HashSet<string>>();
var incorrectingOrderings = new List<string[]>();


var total = 0;
foreach (string line in File.ReadLines("../../../input.txt"))
{
    if (line.Contains('|'))
    {
        var parts = line.Split('|');
        if (rules.ContainsKey(parts[0]))
        {
            rules[parts[0]].Add(parts[1]);
        }
        else
        {
            rules[parts[0]] = [parts[1]];
        }
    }
    else if (line.Contains(','))
    {
        var pages = line.Split(',');
        if (ChecklineFowards(pages))
        {
            total += Int32.Parse(pages[GetMiddle(pages.Length)]);
        }
        else
        {
            incorrectingOrderings.Add(pages);
        }
    }
}

bool LessThan(string curr, string next)
{
    if (rules.ContainsKey(next))
    {
        if (rules[next].Contains(curr))
        {
            return true;
        }
    }

    return false;
}

var newTotal = 0;

foreach (var ordering in incorrectingOrderings)
{
    var n = ordering.Length;
    int i, j;
    string temp;
    for (i = 0; i < n - 1; i++) {
        for (j = 0; j < n - i - 1; j++) {
            if (LessThan(ordering[j],ordering[j + 1])) {
            
                // Swap ordering[j] and ordering[j+1]
                temp = ordering[j];
                ordering[j] = ordering[j + 1];
                ordering[j + 1] = temp;
            }
        }
    }

    newTotal += int.Parse(ordering[GetMiddle(ordering.Length)]);

}




bool ChecklineFowards(string[] pages)
{
    for(int i = 0; i < pages.Length-1; i++)
    {
        if (rules.ContainsKey(pages[i]))
        {
            var validPages = rules[pages[i]];
            if (!CorrectPages(validPages, pages, i+1)){ ;
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    return true;
}

Console.WriteLine($"NEW TOTAL: {newTotal}");
int GetMiddle(int n)
{
    return n / 2;
}
bool CorrectPages(HashSet<string> validPages, string[] pages, int index)
{
    for (int j = index; j < pages.Length; j++)
    {
        if (!validPages.Contains(pages[j]))
        {
            return false;
        }
    }

    return true;
}