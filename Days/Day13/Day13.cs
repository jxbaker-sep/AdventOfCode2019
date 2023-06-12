using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2019.Utils;
using JetBrains.Annotations;

namespace AdventOfCode2019.Days.Day13;

[UsedImplicitly]
public class Day13 : AdventOfCode<long, IReadOnlyList<long>>
{
    private const long Empty = 0;
    private const long Wall = 1;
    private const long Block = 2;
    private const long HorizontalPaddle = 3;
    private const long Ball = 4;
    public override IReadOnlyList<long> Parse(string input) => input
        .Lines().Single().Split(",").Select(it => Convert.ToInt64(it)).ToList();

    [TestCase(Input.File, 344)]
    public override long Part1(IReadOnlyList<long> program)
    {
        var d = new Dictionary<Position, long>();
        var c = new IntcodeComputer(program);
        while (c.TryRunToOutputOrHalt(out var x))
        {
            var y = c.RunToOutput();
            var id = c.RunToOutput();
            d.Add(new(y, x), id);
        }

        return d.Count(it => it.Value == Block);
    }

    [TestCase(Input.File, 17336)]
    public override long Part2(IReadOnlyList<long> program)
    {
        var tempprogram = program.ToList();
        tempprogram[0] = 2;
        var d = new Dictionary<Position, long>();
        var c = new IntcodeComputer(tempprogram);
        var paddle = Position.Zero;
        var ball = Position.Zero;
        var score = 0L;
        c.ProvideInput(0);
        while (true)
        {
            var temp = c.Run();
            if (temp == IntcodeResult.HALT) return score;
            if (temp == IntcodeResult.OUTPUT)
            {
                var x = c.Output;
                var y = c.RunToOutput();
                var id = c.RunToOutput();
                if (x == -1 && y == 0)
                {
                    score = id;
                    continue;
                }
                d[new(y, x)] = id;
                if (id == HorizontalPaddle) paddle = new(y,x);
                else if (id == Ball) ball = new(y,x);
                continue;
            }
            c.ProvideInput(LMath.Sign(ball.X - paddle.X));
        }
    }

    private void Draw(Dictionary<Position, long> d)
    {
        d.Draw(x => x switch {
            Empty => ' ',
            Ball => '*',
            Wall => '#',
            HorizontalPaddle => '_',
            Block => '%',
            _ => throw new ApplicationException()
        }, ' ');
    }
}

