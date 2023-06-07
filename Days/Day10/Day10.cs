using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2019.Utils;
using JetBrains.Annotations;

namespace AdventOfCode2019.Days.Day10;

[UsedImplicitly]
public class Day10 : AdventOfCode<long, IReadOnlySet<Position>>
{
    public override IReadOnlySet<Position> Parse(string input) => input
        .Lines().SelectMany((line, y) => line.WithIndices().Where(c => c.Value == '#').Select(c => new Position(y, c.Index))).ToHashSet();

    [TestCase(Input.Raw, 8, Raw=@".#..#
.....
#####
....#
...##")]
[TestCase(Input.Raw, 33, Raw=@"......#.#.
#..#.#....
..#######.
.#.#.###..
.#..#.....
..#....#.#
#..#....#.
.##.#..###
##...#..#.
.#....####")]
[TestCase(Input.Raw, 210, Raw=@".#..##.###...#######
##.############..##.
.#.######.########.#
.###.#######.####.#.
#####.##.#.##.###.##
..#####..#.#########
####################
#.####....###.#.#.##
##.#################
#####.##.###..####..
..######..##.#######
####.##.####...##..#
.#####..#.######.###
##...#.##########...
#.##########.#######
.####.#.###.###.#.##
....##.##.###..#####
.#.#.###########.###
#.#.#.#####.####.###
###.##.####.##.#..##")]
    [TestCase(Input.File, 344)]
    public override long Part1(IReadOnlySet<Position> asteroids)
    {
        return asteroids.Select(asteroid => asteroids.Where(otherAsteroid => CanSee(asteroid, otherAsteroid, asteroids)).Count()).Max();
    }

    private bool CanSee(Position asteroid, Position otherAsteroid, IReadOnlySet<Position> asteroids)
    {
        if (otherAsteroid == asteroid) return false; // Doesn't count itself
        var delta = NormalizedDelta(asteroid, otherAsteroid);
        for(var current = asteroid + delta; current != otherAsteroid; current += delta)
        {
            if (asteroids.Contains(current)) return false;
        }
        return true;
    }

    // [TestCase(Input.File 0)]
    [TestCase(Input.Raw, 802, Raw = @".#..##.###...#######
##.############..##.
.#.######.########.#
.###.#######.####.#.
#####.##.#.##.###.##
..#####..#.#########
####################
#.####....###.#.#.##
##.#################
#####.##.###..####..
..######..##.#######
####.##.####...##..#
.#####..#.######.###
##...#.##########...
#.##########.#######
.####.#.###.###.#.##
....##.##.###..#####
.#.#.###########.###
#.#.#.#####.####.###
###.##.####.##.#..##")]
    [TestCase(Input.File, 2732)]
    public override long Part2(IReadOnlySet<Position> asteroids)
    {
        var pivot = asteroids.MaxBy(asteroid => asteroids.Where(otherAsteroid => CanSee(asteroid, otherAsteroid, asteroids)).Count())!;
        var others = asteroids.Where(it => it != pivot);
        var quadrants = others.Select(otherAsteroid => {
            var d = otherAsteroid - pivot;
            var delta = new Vector(d.Y, d.X);
            var q = (LMath.Sign(delta.dY) , LMath.Sign(delta.dX)) switch
            {
                (-1, -1) => 3,
                (-1, 0) => 0,
                (-1, 1) => 0,
                (0, -1) => 3,
                (0, 0) => throw new ApplicationException(),
                (0, 1) => 1,
                (1, 1) => 1,
                (1, 0) => 2,
                (1, -1) => 2,
                _ => throw new ApplicationException()
            };
            foreach(var _ in Enumerable.Range(0, q)) delta = delta.RotateLeft();
            return (Asteroid: otherAsteroid, Q: q, Angle: -1.0 * delta.dY / delta.dX);
        })
        .ToDictionaryOfLists(it => it.Q, it => it);
        foreach(var key in quadrants.Keys)
        {
            quadrants[key] = quadrants[key].OrderByDescending(it => it.Angle).ThenBy(it => it.Asteroid.ManhattanDistance(pivot)).ToList();
        }
        
        var n = 0;
        var q = 0;
        var first = true;
        var lastAngle = double.MaxValue;
        Position? needle = null;
        while (n < 200)
        {
            var items = first ? quadrants[q].Take(1).ToList() : quadrants[q].Where(item => item.Angle < lastAngle).ToList();
            if (items.Count == 0)
            {
                q = (q + 1) % 4;
                first = true;
                continue;
            }
            first = false;
            var item = items[0];
            // Console.WriteLine($"{item}");
            quadrants[q].Remove(item);
            lastAngle = item.Angle;
            needle = item.Asteroid;
            n += 1;
        }

        
        return needle!.X * 100 + needle.Y;
    }

    private Vector NormalizedDelta(Position p1, Position p2)
    {
        var x = p2.X - p1.X;
        var y = p2.Y - p1.Y;
        var signX = LMath.Sign(x);
        var signY = LMath.Sign(y);
        x = LMath.Abs(x);
        y = LMath.Abs(y);

        if (x == 0) return new Vector(signY, 0);
        if (y == 0) return new Vector(0, signX);

        for(var divisor = 2; divisor <= Math.Min(x, y); )
        {
            if ((x % divisor == 0) && (y % divisor == 0))
            {
                x /= divisor;
                y /= divisor;
            }
            else divisor += 1;
        }
        return new Vector(signY * y, signX * x);
    }
}