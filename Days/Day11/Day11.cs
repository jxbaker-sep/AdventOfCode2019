using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2019.Utils;
using JetBrains.Annotations;

namespace AdventOfCode2019.Days.Day11;

[UsedImplicitly]
public class Day11 : AdventOfCode<long, IReadOnlyList<long>>
{
    private const long Black = 0;
    private const long White = 1;
    public override IReadOnlyList<long> Parse(string input) => input
        .Lines().Single().Split(",").Select(it => Convert.ToInt64(it)).ToList();

    [TestCase(Input.File, 2511)]
    public override long Part1(IReadOnlyList<long> program)
    {
        var c = new IntcodeComputer(program);
        var d = new Dictionary<Position, long>();
        var p = Position.Zero;
        var v = Vector.North;
        while (true)
        {
            c.ProvideInput(d.GetValueOrDefault(p));
            var x = c.RunToOutputOrHalt();
            if (x == IntcodeResult.HALT) break;
            var newColor = c.Output;
            var direction = c.RunToOutput();
            d[p] = newColor;
            v = direction switch { 0 => v.RotateLeft(), 1 => v.RotateRight(), _ => throw new ApplicationException() };
            p += v;
        }

        return d.Count();
    }

    [TestCase(Input.File, 0)]
    public override long Part2(IReadOnlyList<long> program)
    {
        var c = new IntcodeComputer(program);
        var d = new Dictionary<Position, long>();
        d[Position.Zero] = White;
        var p = Position.Zero;
        var v = Vector.North;
        while (true)
        {
            c.ProvideInput(d.GetValueOrDefault(p));
            var x = c.RunToOutputOrHalt();
            if (x == IntcodeResult.HALT) break;
            var newColor = c.Output;
            var direction = c.RunToOutput();
            d[p] = newColor;
            v = direction switch { 0 => v.RotateLeft(), 1 => v.RotateRight(), _ => throw new ApplicationException() };
            p += v;
        }

        d.Print(c => c == White ? '█' : ' ', ' ');

        return 0;
    }
}

