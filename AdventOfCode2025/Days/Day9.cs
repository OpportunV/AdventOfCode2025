using System.Linq;
using Common.Extensions;
using Common.Helpers;
using Common.Models;

namespace AdventOfCode2025.Days;

public class Day9 : Day
{
    private readonly Vector3<long>[] _coords;

    public Day9()
    {
        _coords = GetInput()
            .Select(line => line.GetNumbers<long>())
            .Select(nums => new Vector3<long>(nums[0], nums[1], 0))
            .ToArray();
    }

    public override string Part1()
    {
        return Combinations.GenerateAllPairs(_coords)
            .Select(pair => new Rect2d<long>(pair.first, pair.second))
            .Max(rect => rect.Area)
            .ToString();
    }

    public override string Part2()
    {
        var lines = _coords
            .Zip(_coords.Skip(1).Append(_coords[0]), (first, second) => new Line<long>(first, second))
            .ToHashSet();

        var valid = Combinations.GenerateAllPairs(_coords)
            .Select(pair => new Rect2d<long>(pair.first, pair.second))
            .OrderByDescending(rect => rect.Area)
            .First(rect => !lines.Any(rect.Intersects));

        return valid.Area.ToString();
    }
}