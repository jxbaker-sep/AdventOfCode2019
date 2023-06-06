using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2019.Utils;
using JetBrains.Annotations;

namespace AdventOfCode2019.Days.Day06;

[UsedImplicitly]
public class Day06 : AdventOfCode<long, IReadOnlyList<Node>>
{
    public override IReadOnlyList<Node> Parse(string input) => input
        .Lines(line => new Node(line.Split(")")[0], line.Split(")")[1]));

    [TestCase(Input.Example, 42)]
    [TestCase(Input.File, 300598)]
    public override long Part1(IReadOnlyList<Node> input)
    {
        var d = input.ToDictionary(it => it.Orbiter, it => it.Center);
        var allOrbiters = input.Select(it => it.Orbiter).ToList();
        return allOrbiters.Sum(orbiter => Count(orbiter, d));
    }

    [TestCase(Input.Raw, 4, Raw = @"COM)B
B)C
C)D
D)E
E)F
B)G
G)H
D)I
E)J
J)K
K)L
K)YOU
I)SAN")]
    [TestCase(Input.File, 520)]
    public override long Part2(IReadOnlyList<Node> input)
    {
        var orbiterToCenter = input.ToDictionary(it => it.Orbiter, it => it.Center);
        var centerToOrbiters = input.ToDictionaryOfLists(it => it.Center, it => it.Orbiter);
        var start = orbiterToCenter["YOU"];
        var destination = orbiterToCenter["SAN"];
        var open = new Queue<Move>();
        var closed = new HashSet<string>{start, "YOU"};
        open.Enqueue(new Move(start, new List<string>()));
        while (open.TryDequeue(out var current))
        {
            var orbiters = centerToOrbiters.GetValueOrDefault(current.Center) ?? new List<string>();
            var orbiting = orbiterToCenter.GetValueOrDefault(current.Center) is {} sat ? new List<string>{sat} : new List<string>();
            foreach(var neighbor in orbiters.Concat(orbiting))
            {
                if (!closed.Add(neighbor)) continue;
                if (neighbor == destination) return current.Path.Count + 1;
                open.Enqueue(new (neighbor, current.Path.Append(current.Center).ToList()));
            }
        }
        throw new ApplicationException();
    }

    private long Count(string v, Dictionary<string, string> d)
    {
        if (!d.ContainsKey(v)) return 0;
        return 1 + Count(d[v], d);
    }

}

public record Node(string Center, string Orbiter);
public record Move(string Center, IReadOnlyList<string> Path);