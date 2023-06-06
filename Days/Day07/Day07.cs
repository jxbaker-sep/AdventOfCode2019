using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2019.Utils;
using JetBrains.Annotations;

namespace AdventOfCode2019.Days.Day07;

[UsedImplicitly]
public class Day07 : AdventOfCode<long, IReadOnlyList<long>>
{
    public override IReadOnlyList<long> Parse(string input) => input
        .Lines().Single().Split(",").Select(it => Convert.ToInt64(it)).ToList();

    [TestCase(Input.File, 368584)]
    public override long Part1(IReadOnlyList<long> input)
    {
        return new List<int>{0, 1, 2, 3, 4}.Permute().Max(permutation => CalculatePower(permutation, input));
    }


    [TestCase(Input.File, 35993240)]
    public override long Part2(IReadOnlyList<long> input)
    {
        return new List<int>{5, 6, 7, 8, 9}.Permute().Max(permutation => CalculatePower2(permutation, input));
    }

    private long CalculatePower(List<int> permutation, IReadOnlyList<long> input)
    {
        long output = 0;
        foreach(var n in permutation)
        {
            var c = new IntcodeComputer(input);
            c.ProvideInput(n);
            c.ProvideInput(output);
            c.Run();
            output = c.Output;
        }
        return output;
    }

    private long CalculatePower2(List<int> permutation, IReadOnlyList<long> input)
    {
        var computers = permutation.Select(n => {
            var c = new IntcodeComputer(input);
            c.ProvideInput(n);
            return c;
        }).ToList();

        long output = 0;
        IntcodeResult result = IntcodeResult.OUTPUT;
        while (result != IntcodeResult.HALT)
        {
            foreach(var c in computers)
            {
                c.ProvideInput(output);
                result = c.Run();
                output = c.Output;
            }
        }
        
        return output;
    }
}