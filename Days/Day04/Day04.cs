using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2019.Utils;
using JetBrains.Annotations;

namespace AdventOfCode2019.Days.Day04;

[UsedImplicitly]
public class Day04 : AdventOfCode<long, IReadOnlyList<long>>
{
    public override IReadOnlyList<long> Parse(string input) => input
        .Lines().Single().Split("-").Select(it => Convert.ToInt64(it)).ToList();

    [TestCase(Input.Raw, 1890, Raw = @"138241-674034")]
    public override long Part1(IReadOnlyList<long> input)
    {
        var count = 0;
        for(var i = input[0]; i <= input[1]; i++)
        {
            var foundDouble = false;
            var allAscending = true;
            var previous = 0;
            foreach(var c in $"{i}")
            {
                var x = c - '0';
                if (x == previous) foundDouble = true;
                if (x < previous) { allAscending = false; break; }
                previous = x;
            }
            if (foundDouble && allAscending) count += 1;
        }
        return count;
    }

    [TestCase(Input.Raw, 1277, Raw = @"138241-674034")] // 1075 too low
    public override long Part2(IReadOnlyList<long> input)
    {
        var count = 0;
        for(var i = input[0]; i <= input[1]; i++)
        {
            var runLength = 1;
            var foundDouble = false;
            var allAscending = true;
            var previous = 0;
            var stopLookingForDoubles = false;
            foreach(var c in $"{i}")
            {
                var x = c - '0';
                if (x == previous && !stopLookingForDoubles) {
                    runLength += 1;
                    foundDouble = runLength == 2;
                } else {
                    stopLookingForDoubles = foundDouble;
                    runLength = 1;
                }
                if (x < previous) { allAscending = false; break; }
                previous = x;
            }
            if (foundDouble && allAscending) count += 1;
        }
        return count;
    }
}

public record Instruction(Vector Vector, int Magnitude);