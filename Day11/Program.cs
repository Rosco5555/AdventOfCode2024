// See https://aka.ms/new-console-template for more information

using Day11;

var stones =  File.ReadAllText("../../../input.txt").Split(" ").Select(x=>x.ToCharArray()).ToList();

char[] RemoveLeadingZeroes(char[] stone)
{
    var stoneNoZeroes = String.Join("",stone).TrimStart(['0']).ToCharArray();
    if (stoneNoZeroes.Length == 0)
    {
        return ['0'];
    }

    return stoneNoZeroes;
}

List<char[]> SplitStone(char[] stone)
{
    var n = stone.Length;

    var mid = n / 2;
    var leftStone = new char[mid];
    var rightStone = new char[mid];

    for (int i = 0; i < mid; i++)
    {
        leftStone[i] = stone[i];
        rightStone[i] = stone[mid+i];
    }

    rightStone = RemoveLeadingZeroes(rightStone);
    
    return [leftStone, rightStone];
}

var cache = new Dictionary<Args, long>();


long Blink(char[] stone, int n, int blinks)
{
    if (cache.ContainsKey(new Args(stone, n,blinks)))
    {
        return cache[new Args(stone, n,blinks)];
    }
    
    if (blinks == n)
    {
        return 1;
    }
    
    if (stone is ['0'])
    {
        var res = Blink(['1'], n, blinks + 1);
        cache[new Args(['1'], n, blinks+1)] = res;
        return res;
    } 
    if (stone.Length % 2 == 0)
    {
        var newStones = SplitStone(stone);
        var left = Blink(newStones[0], n, blinks + 1);
        cache[new Args(newStones[0], n ,blinks+1)] = left;
        var right = Blink(newStones[1], n, blinks + 1);
        cache[new Args(newStones[1], n ,blinks+1)] = right;
        return left + right;
    }
    else
    {
        var stoneStr = String.Join("", stone);
        var stoneInt = long.Parse(stoneStr);
        var res = (stoneInt * 2024);
        var strArr = res.ToString().ToCharArray();
        var ans =  Blink(strArr, n, blinks+1);
        cache[new Args(strArr, n, blinks+1)] = ans;
        return ans;
    }
}


long total = 0;

foreach (var stone in stones)
{
    total += Blink(stone, 75, 0);
}

Console.WriteLine();
Console.WriteLine(total);
