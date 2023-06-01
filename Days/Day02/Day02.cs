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
    [TestCase(Input.File, 9706670)] // 1 - wrong
    public override long Part1(IReadOnlyList<long> input)
    {
        var program = input.ToList();
        program[1] = 12;
        program[2] = 2;
        return RunOpcodeProgram(program);
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
                var result = RunOpcodeProgram(program);
                if (result == 19690720) return 100 * noun + verb;
            }
        }
        throw new ApplicationException();
    }

    private long RunOpcodeProgram(IReadOnlyList<long> input)
    {
        var program = input.ToList();
        var pc = 0;

        while (true)
        {
            switch (program[pc])
            {
                case 99: return program[0];
                case 1: program[(int)program[pc + 3]] = program[(int)program[pc + 1]] + program[(int)program[pc + 2]]; break;
                case 2: program[(int)program[pc + 3]] = program[(int)program[pc + 1]] * program[(int)program[pc + 2]]; break;
                default: throw new ApplicationException($"Encountered opcode {program[pc]} at position {pc}");
            }
            pc += 4;
        }
    }
}
