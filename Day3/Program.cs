using System.Text.RegularExpressions;

var total = 0;
var enabled = true;
var input = File.ReadAllText("../../../input.txt");

string pattern = @"mul\(([0-9]{1,3}),([0-9]{1,3})\)|do\(\)|don't\(\)";

var matches = Regex.Matches(input, pattern);

foreach (Match match in matches)
{
    switch (match.Value)
    {
        case "don't()":
            enabled = false;
            break;
        case "do()":
            enabled = true;
            break;
        default:
            if (enabled)
            {
                total += int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);
            }
            break; 
            
    }
    
    
    
}

Console.WriteLine(total);