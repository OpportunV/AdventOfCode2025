using System.Collections.Generic;
using System.Linq;
using Common.Extensions;

namespace AdventOfCode2025.Days;

public class Day11 : Day
{
    private readonly Dictionary<string, HashSet<string>> _graph;
    private const string Dac = "dac";
    private const string End = "out";
    private const string Fft = "fft";
    private const string StartPart1 = "you";
    private const string StartPart2 = "svr";

    public Day11()
    {
        _graph = GetInput()
            .Select(line => line.GetWords())
            .ToDictionary(
                words => words[0],
                words => words[1..].ToHashSet());
    }

    public override string Part1()
    {
        return CountPaths(StartPart1, End).ToString();
    }

    public override string Part2()
    {
        return CountPaths2(StartPart2, End, [], 0, 6).ToString();
    }

    private long CountPaths(string start, string end)
    {
        if (start == end)
        {
            return 1;
        }

        if (!_graph.TryGetValue(start, out var other))
        {
            return 0;
        }

        var res = other.Sum(next => CountPaths(next, end));

        return res;
    }

    private long CountPaths2(string start, string end, Dictionary<(string, int), long> cache, int flag, int targetFlag)
    {
        if (start == end)
        {
            return flag == targetFlag ? 1 : 0;
        }

        switch (start)
        {
            case Dac:
                flag |= 1 << 1;
                break;
            case Fft:
                flag |= 1 << 2;
                break;
        }

        if (!_graph.TryGetValue(start, out var other))
        {
            return 0;
        }

        if (cache.TryGetValue((start, flag), out var val))
        {
            return val;
        }

        var res = other.Sum(next => CountPaths2(next, end, cache, flag, targetFlag));
        cache[(start, flag)] = res;

        return res;
    }
}