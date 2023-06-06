using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2019.Utils;
using JetBrains.Annotations;

namespace AdventOfCode2019.Days.Day03;

[UsedImplicitly]
public class Day03 : AdventOfCode<long, IReadOnlyList<IReadOnlyList<Instruction>>>
{
    public override IReadOnlyList<IReadOnlyList<Instruction>> Parse(string input) => input
        .Lines().Select(line => line.Split(",")
            .Select(item => new Instruction(
                item[0] switch {
                    'U' => Vector.North,
                    'R' => Vector.East,
                    'D' => Vector.South,
                    'L' => Vector.West,
                    _ => throw new ApplicationException($"Unexpected direction {item}")
                }
                , Convert.ToInt32(item.Substring(1))))
            .ToList())
        .ToList();

    [TestCase(Input.Raw, 159, Raw = @"R75,D30,R83,U83,L12,D49,R71,U7,L72
U62,R66,U55,R34,D71,R55,D58,R83")]
    [TestCase(Input.Raw, 135, Raw = @"R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51
U98,R91,D20,R16,D67,R40,U7,R15,U6,R7")]
    [TestCase(Input.File, 651)]
    public override long Part1(IReadOnlyList<IReadOnlyList<Instruction>> input)
    {
        return Draw(input[0]).Intersect(Draw(input[1])).Select(it => it.ManhattanDistance(Position.Zero)).Min();
    }

    [TestCase(Input.Raw, 610, Raw = @"R75,D30,R83,U83,L12,D49,R71,U7,L72
U62,R66,U55,R34,D71,R55,D58,R83")]
    [TestCase(Input.Raw, 410, Raw = @"R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51
U98,R91,D20,R16,D67,R40,U7,R15,U6,R7")]
    [TestCase(Input.File, 7534)]
    public override long Part2(IReadOnlyList<IReadOnlyList<Instruction>> input)
    {
        var path0 = Draw(input[0]).ToList();
        var path1 = Draw(input[1]).ToList();
        return path0.Intersect(path1).Select(it => path0.IndexOf(it) + path1.IndexOf(it)).Min() + 2;
    }

    private IEnumerable<Position> Draw(IEnumerable<Instruction> instructions)
    {
        var current = Position.Zero;
        foreach(var instruction in instructions)
        {
            foreach(var step in Enumerable.Range(0, instruction.Magnitude))
            {
                current += instruction.Vector;
                yield return current;
            }
        }
    }
}

public record Instruction(Vector Vector, int Magnitude);