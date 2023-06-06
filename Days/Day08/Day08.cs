using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2019.Utils;
using JetBrains.Annotations;

namespace AdventOfCode2019.Days.Day08;

[UsedImplicitly]
public class Day08 : AdventOfCode<long, IReadOnlyList<long>>
{
    public override IReadOnlyList<long> Parse(string input) => input
        .Lines().Single().Select(it => (long)(it - '0')).ToList();

    [TestCase(Input.File, 1703)]
    public override long Part1(IReadOnlyList<long> input)
    {
        var layerSize = 25 * 6;
        var layers = Enumerable.Range(0, input.Count / layerSize).Select(n => input.Skip(n * layerSize).Take(layerSize).ToList()).ToList();
        var needle = layers.MinBy(layer => layer.Count(it => it == 0)) ?? throw new ApplicationException();
        return needle.Count(it => it == 1) * needle.Count(it => it == 2);
    }


    [TestCase(Input.File, 0)]
    public override long Part2(IReadOnlyList<long> input)
    {
        var layerSize = 25 * 6;
        var layers = Enumerable.Range(0, input.Count / layerSize).Select(n => input.Skip(n * layerSize).Take(layerSize).ToList()).ToList();
        var image = layers.Aggregate((accumulator, current) => accumulator.Zip(current).Select(it => it.First == 2 ? it.Second : it.First).ToList());
        foreach (var y in Enumerable.Range(0, 6))
        {
            Console.WriteLine();
            foreach (var x in Enumerable.Range(0, 25))
            {
                if (image[y * 25 + x] == 0) // BLACK
                {
                    Console.Write(' ');
                } else Console.Write('█');
            }
        }
        return 0;
    }
}