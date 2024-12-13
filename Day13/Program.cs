using System.Text.RegularExpressions;
using MathNet.Numerics.LinearAlgebra;

var map = new List<string>();

long ans = 0;
var bigString = File.ReadAllText("../../../input.txt");

var equations = bigString.Split("\n\n");
foreach (var line in equations)
{

    string pattern = @"(\d+)+";
    var matches = Regex.Matches(line, pattern);

    var x1 = long.Parse(matches[0].Value);
    var x2 = long.Parse(matches[2].Value);

    var y1 = long.Parse(matches[1].Value);
    var y2 = long.Parse(matches[3].Value);
    
    var x = long.Parse(matches[4].Value) + 10000000000000;
    var y = long.Parse(matches[5].Value)+10000000000000;
    
    
    var M = Matrix<double>.Build;
    double[,] a = {{ x1, x2},
        { y1, y2}};
    var A = M.DenseOfArray(a);

    double[,] b = {{x},{y}};

    var B = M.DenseOfArray(b);
    
    
    if (A != null && A.Determinant() != 0)
    {
        // invertible
        var aInverse = A.Inverse();
        var X = aInverse.Multiply(B);

        var n1 = X.At(0, 0);
        var n2 = X.At(1, 0);

        if (Tolerate(n1) && Tolerate(n2) )
        {
            ans += Convert.ToInt64(n2 + (n1 * 3));
        }
    }
    
    foreach (Match m in matches)
    {
        Console.WriteLine(m.Value);
    }
    
    map.Add(line);
}


bool Tolerate(double n)
{
    return Math.Abs(n - Math.Round(n)) <= 0.01;
}
Console.WriteLine($"ans {ans}");
// Console.WriteLine(Convert.ToInt64(3.9999d));