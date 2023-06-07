using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2019.Utils;
using JetBrains.Annotations;

namespace AdventOfCode2019.Days.Day09;

[UsedImplicitly]
public class Day09 : AdventOfCode<long, IReadOnlyList<long>>
{
    public override IReadOnlyList<long> Parse(string input) => input
        .Lines().Single().Split(",").Select(it => Convert.ToInt64(it)).ToList();

    [TestCase(Input.File, 2518058886L)]
    public override long Part1(IReadOnlyList<long> input)
    {
        var c = new IntcodeComputer(input);
        c.ProvideInput(1);
        c.Run();
        return c.Output;
    }


    [TestCase(Input.File, 44292)]
    public override long Part2(IReadOnlyList<long> input)
    {
        var c = new IntcodeComputer(input);
        c.ProvideInput(2);
        c.Run();
        return c.Output;
    }
}