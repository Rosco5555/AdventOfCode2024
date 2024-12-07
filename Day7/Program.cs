using Day7;

var equations = new List<Equation>();

foreach (string line in File.ReadLines("../../../input.txt"))
{
    var equation = line.Split(':');
    var ans = long.Parse(equation[0]);
    var nums = equation[1].Split(' ').Where(x => x.Length > 0).Select(long.Parse).ToArray();
    equations.Add(new Equation(ans, nums));

}

long total = 0;
foreach (var eqn in equations)
{
    if (Solvable(1, eqn.nums[0], eqn))
    {
        total += eqn.ans;
    }
}

long Concat(long a, long b)
{
    return long.Parse(a.ToString() + b.ToString());
}

bool Solvable(int i, long total, Equation eqn)
{
    if (i == eqn.nums.Length)
    {
        return (total == eqn.ans);
    }

    
    var b = eqn.nums[i];

    var add = Solvable(i+1, total + b, eqn);
    var multiply = Solvable(i + 1, total * b, eqn);
    var concat = Solvable(i + 1, Concat(total, b), eqn);
    return add || multiply || concat;
}

Console.WriteLine(total);