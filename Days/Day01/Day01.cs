using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2019.Utils;
using JetBrains.Annotations;
using TypeParser;

namespace AdventOfCode2019.Days.Day01;

[UsedImplicitly]
public class Day01 : AdventOfCode<long, IReadOnlyList<long>>
{
    public override IReadOnlyList<long> Parse(string input) => input
        .Lines().Select(line => Convert.ToInt64(line)).ToList();

    // [TestCase(Input.Example, 24000)]
    [TestCase(Input.File, 0)]
    public override long Part1(IReadOnlyList<long> input)
    {
        return input.Select(it => it / 3 - 2).Sum();
    }

    // [TestCase(Input.Example, 45000)]
    [TestCase(Input.File, 0)]
    public override long Part2(IReadOnlyList<long> input)
    {
        return input.Select(ComputeMass).Sum();
    }

    private long ComputeMass(long input)
    {
        var result = Math.Max(input / 3 - 2, 0);
        if (result > 0) return result + ComputeMass(result);
        return result;
    }
}