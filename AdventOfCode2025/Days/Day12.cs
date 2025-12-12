using System.Collections.Generic;
using System.Linq;
using Common.Extensions;
using Common.Models;

namespace AdventOfCode2025.Days;

public class Day12 : Day
{
    private readonly Grid<bool>[] _presents;
    private readonly (Grid<bool> grid, List<int> presents)[] _targets;

    public Day12()
    {
        var splits = GetInputRaw().GetSections();
        _presents = splits[..^1]
            .Select(split => split
                .Skip(1)
                .Select(line => line
                    .Select(chr => chr == '#')
                    .ToArray())
                .ToArray())
            .Select(present => new Grid<bool>(present))
            .ToArray();

        _targets = splits[^1]
            .Select(line => line.GetNumbers<int>())
            .Select(numbers => (grid: new Grid<bool>(numbers[0], numbers[1], false),
                presents: numbers[2..].ToList()))
            .ToArray();
    }

    public override string Part1()
    {
        var areas = _presents
            .Select(present => present
                .Flatten()
                .Count(item => item.Value))
            .ToList();

        return _targets
            .Count(target =>
                target.grid.Rows * target.grid.Cols
                > target.presents
                    .Select((amount, ind) => amount * areas[ind])
                    .Sum())
            .ToString();
    }

    public override string Part2()
    {
        return "*";
    }
}