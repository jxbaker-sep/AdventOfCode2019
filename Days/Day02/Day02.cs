using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2019.Utils;
using JetBrains.Annotations;
using TypeParser;

namespace AdventOfCode2019.Days.Day02;

[UsedImplicitly]
public class Day02 : AdventOfCode<long, IReadOnlyList<long>>
{
    public override IReadOnlyList<long> Parse(string input) => input
        .Lines().Single().Split(",").Select(it => Convert.ToInt64(it)).ToList();

    // [TestCase(Input.Example, 24000)]
    [TestCase(Input.File, 9706670)]
    public override long Part1(IReadOnlyList<long> input)
    {
        var program = input.ToList();
        program[1] = 12;
        program[2] = 2;
        var comp = new IntcodeComputer(program);
        comp.Run();
        return comp.ReadOnlyMemory[0];
    }

    // [TestCase(Input.Example, 45000)]
    [TestCase(Input.File, 2552)]
    public override long Part2(IReadOnlyList<long> input)
    {
        foreach(var noun in Enumerable.Range(0, 100))
        {
            foreach(var verb in Enumerable.Range(0, 100))
            {
                var program = input.ToList();
                program[1] = noun;
                program[2] = verb;
                var comp = new IntcodeComputer(program);
                comp.Run();
                var result = comp.ReadOnlyMemory[0];
                if (result == 19690720) return 100 * noun + verb;
            }
        }
        throw new ApplicationException();
    }
}
