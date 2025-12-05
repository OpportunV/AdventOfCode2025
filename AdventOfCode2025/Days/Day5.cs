using System;
using System.Collections.Generic;
using System.Linq;
using Common.Extensions;
using Common.Models;

namespace AdventOfCode2025.Days;

public class Day5 : Day
{
    private readonly List<long> _ids;
    private readonly List<Range<long>> _ranges;

    public Day5()
    {
        var split = GetInputRaw().Split("\n\n");
        _ranges = split[0]
            .GetNumbers<long>()
            .Chunk(2)
            .Select(chunk => new Range<long>(chunk[0], Math.Abs(chunk[1]) + 1))
            .OrderBy(range => range.Start)
            .ToList();

        _ids = split[1].GetNumbers<long>();
    }

    public override string Part1()
    {
        return _ids.Count(id => _ranges.Any(range => range.Contains(id))).ToString();
    }

    public override string Part2()
    {
        return MergeRanges().Sum(range => range.Length).ToString();
    }

    private List<Range<long>> MergeRanges()
    {
        var combined = new List<Range<long>>(_ranges.Count) { _ranges[0] };
        foreach (var range in _ranges.Skip(1))
        {
            var current = combined[^1];
            if (current.Intersects(range))
            {
                combined[^1] = current.Union(range);
            }
            else
            {
                combined.Add(range);
            }
        }

        return combined;
    }
}