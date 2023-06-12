using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2019.Utils;
using JetBrains.Annotations;

namespace AdventOfCode2019.Days.Day15;

[UsedImplicitly]
public class Day15 : AdventOfCode<long, IReadOnlyList<long>>
{
    private const long North = 1;
    private const long South = 2;
    private const long West = 3;
    private const long East = 4;
    private const long Wall = 0;
    private const long Open = 1;
    private const long OxygenSystem = 2;
    public override IReadOnlyList<long> Parse(string input) => input
        .Lines().Single().Split(",").Select(it => Convert.ToInt64(it)).ToList();

    [TestCase(Input.File, 380)]
    public override long Part1(IReadOnlyList<long> program)
    {
        var unexplored = new HashSet<Position>();

        var explored = new Dictionary<Position, long>();
        var c = new IntcodeComputer(program);

        var current = Position.Zero;
        var proposed = Position.Zero + Vector.North;
        var steps = new Queue<Position>();
        c.ProvideInput(DirectionTo(current, proposed));
        while (true)
        {
            var x = c.Run();
            if (x == IntcodeResult.HALT) throw new ApplicationException();
            if (x == IntcodeResult.OUTPUT)
            {
                explored[proposed] = c.Output;
                // Console.WriteLine("\n\n========================\n");
                
                if (c.Output == Wall)
                {
                    
                }
                else if (c.Output == OxygenSystem)
                {
                    Console.WriteLine($" Part 2 = {Floodfill(proposed, explored)}");
                    return DirectionsTo(proposed, Position.Zero, explored).Count;
                }
                else
                {
                    current = proposed;
                }
                // Draw(explored.Keys.Append(current).Distinct().ToDictionary(it => it, it => it == current ? 123L : explored[it]));
                continue;
            }
            // INPUT
            if (steps.Count == 0)
            {
                var unexploredNeighbors = current.OrthoganalNeighbors().Except(explored.Keys).ToList();
                foreach(var item in unexploredNeighbors) unexplored.Add(item);
                if (unexploredNeighbors.FirstOrDefault() is {} firstUnexploredNeighbor)
                {
                    unexplored.Remove(firstUnexploredNeighbor);
                    steps.Enqueue(firstUnexploredNeighbor);
                }
                else
                {
                    var destination = unexplored.FirstOrDefault() ?? throw new ApplicationException();
                    unexplored.Remove(destination);
                    var directions = DirectionsTo(current, destination, explored);
                    foreach(var direction in directions) steps.Enqueue(direction);
                }
            }
            var step = steps.Dequeue();
            c.ProvideInput(DirectionTo(current, step));
            proposed = step;
        }

        throw new ApplicationException();
    }

    private long Floodfill(Position proposed, IReadOnlyDictionary<Position, long> explored)
    {
        var count = 0;
        var temp = explored.ToDictionary(it => it.Key, it => it.Value);

        while (true)
        {
            var openAdjacent = explored.Where(it => it.Value == OxygenSystem)
                .SelectMany(it => it.Key.OrthoganalNeighbors().Where(nb => explored[nb] == Open))
                .ToList();
            if (!openAdjacent.Any()) return count;
            count += 1;
            foreach(var item in openAdjacent)
            {
                temp[item] = OxygenSystem;
            }
        }
        throw new ApplicationException();
    }

    private void Draw(Dictionary<Position, long> explored)
    {
        explored.Draw(it => it switch{ Wall => '#', Open => '.', 123 => '@', _ => throw new ApplicationException() }, '?');
    }

    [TestCase(Input.File, 0)]
    public override long Part2(IReadOnlyList<long> program)
    {
        return 0;
    }

    private long DirectionTo(Position current, Position next)
    {
        if (next == current + Vector.North) return North;
        if (next == current + Vector.East) return East;
        if (next == current + Vector.West) return West;
        if (next == current + Vector.South) return South;
        throw new ApplicationException();
    }

    private IReadOnlyList<Position> DirectionsTo(Position start, Position destination, IReadOnlyDictionary<Position, long> map)
    {
        var closed = new HashSet<Position>{start};
        var open = new PriorityQueue<(Position Current, IReadOnlyList<Position> Steps)>(it => it.Current.ManhattanDistance(destination) + it.Steps.Count);
        open.Enqueue((start, new List<Position>()));
        while (open.TryDequeue(out var current))
        {
            foreach(var neighbor in current.Current.OrthoganalNeighbors())
            {
                if (neighbor == destination) return current.Steps.Append(neighbor).ToList();
                if (!map.TryGetValue(neighbor, out var temp) || temp != Open) continue;
                if (!closed.Add(neighbor)) continue;
                open.Enqueue((neighbor, current.Steps.Append(neighbor).ToList()));
            }
        }
        throw new ApplicationException();
    }
}

