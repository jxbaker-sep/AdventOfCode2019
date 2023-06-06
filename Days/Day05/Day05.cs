using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2019.Utils;
using JetBrains.Annotations;

namespace AdventOfCode2019.Days.Day05;

[UsedImplicitly]
public class Day05 : AdventOfCode<long, IReadOnlyList<long>>
{
    public override IReadOnlyList<long> Parse(string input) => input
        .Lines().SelectMany(line => line.Split(",")).Select(it => Convert.ToInt64(it)).ToList();

    [TestCase(Input.File, 16225258)]
    public override long Part1(IReadOnlyList<long> input)
    {
        var computer = new IntcodeComputer(input);
        computer.ProvideInput(1);
        computer.RunToHalt();
        return computer.Output;
    }

    [TestCase(Input.File, 2808771)]
    public override long Part2(IReadOnlyList<long> input)
    {
        var computer = new IntcodeComputer(input);
        computer.ProvideInput(5);
        computer.RunToHalt();
        return computer.Output;
    }
}

public record Instruction(Vector Vector, int Magnitude);