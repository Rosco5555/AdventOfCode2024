using System.Security.Cryptography;
using Day17;

long GetComboOperand(long operand, State s)
{
    switch (operand)
    {
        case 0:
        case 1:
        case 2:
        case 3:
            return operand;
        case 4:
            return s.A;
        case 5:
            return s.B;
        case 6:
            return s.C;
        default:
            throw new Exception("Invalid operand");
    }
}

State PerformInstruction(long opcode, long operand, State s, List<long> output)
{
    // Console.WriteLine($"Instruction polonger: {s.Ip} Performing opcode: {opcode} on operand {operand}");
    switch (opcode)
    {
        case 0: // Av
            s.A= (long)(s.A/ Math.Pow(2, GetComboOperand(operand,s)));
            s.Ip+=2;
            return s;
        case 1:
            s.B= operand ^ s.B;
            s.Ip+=2;
            return s;
        case 2:
            s.B= GetComboOperand(operand,s) % 8;
            s.Ip+=2;
            return s;
        case 3:
            if (s.A!= 0)
            {
                s.Ip = operand;
            }
            else
            {
                s.Ip += 2;
            }
            return s;
        case 4:
            s.B= s.B^ s.C;
            s.Ip+=2;
            return s;
        case 5:
            output.Add(GetComboOperand(operand,s) % 8);
            s.Ip+=2;
            return s;
        case 6:
            s.B=(long)(s.A/ Math.Pow(2, GetComboOperand(operand, s)));
            s.Ip+=2;
            return s;
        case 7:
            s.C=(long)(s.A/ Math.Pow(2, GetComboOperand(operand,s)));
            s.Ip+=2;
            return s;
    }

    return s;
}

var input = new List<long>();

bool SameFirstN(List<long> one, List<long> two, int n)
{
    bool same = false;
    for (var i = n; i < two.Count(); i++)
    {
        if (one[two.Count()-i-1] != two[i])
        {
            return false;
        }
    }

    if (n == 0)
    {
        return (one[0] == two[0]);
    }
    
    return true;
}

long ReadProgram()
{
    var lines = File.ReadLines("../../../input.txt").ToList();
    long A= long.Parse(lines[0].Split(": ")[1]);
    long B= long.Parse(lines[1].Split(": ")[1]);
    long C= long.Parse(lines[2].Split(": ")[1]);

    input = lines[4].Split(": ")[1].Split(",").Select(x => long.Parse(x)).ToList();

    

    var output = ExecuteProgram(new State(182976, B, C, 0), input);
    
    Console.WriteLine(String.Join(", ",output));
    // First digit repeats every 8
    // so when a = 2 you get first digit is 2 0o1
    // i 0, 9 = 11
    // output len = log base 8 of a
    // looks like first digit in output repeats every 8..
    // second more like every 64
  
    // 137

    
    var tarIndex = 5;

    long lower = 0;
    
    for (int i = input.Count()-1; i >= 0; i--)
    {
        // Console.WriteLine(lower);
        for (var a = lower; a <= lower + (long)(Math.Pow(8, input.Count) - i);a++) // upper bound can't be larger than this because otherwise we get too many digits in output
        {
            var newOutput =  ExecuteProgram(new State(a, B, C, 0), input);
            // Console.WriteLine(a);
            if (Enumerable.SequenceEqual(newOutput, input.Skip(i).Take(input.Count()-i)))
            {
                if (newOutput.Count() == input.Count())
                {
                    Console.WriteLine(a);
                    return a;
                }
                
             
                var inBase8 = Convert.ToString(a, 8);
                // Console.WriteLine(aOctel);
                lower = (long)(Convert.ToInt64(inBase8 + '0', 8));
                break;
            }
        }
    }

    return 0;
}

List<long> ExecuteProgram(State state, List<long> input)
{
    long progLength = input.Count;
    var output = new List<long>();
    while (state.Ip < progLength)
    {
        long opcode = input[(Index)state.Ip];
        long operand = input[(Index)(state.Ip + 1)];
        PerformInstruction(opcode, operand, state, output);
    }
    
    // Console.WriteLine($"Final a value {state.A}");
    return output;
}


var res = ReadProgram();
Console.WriteLine(res);