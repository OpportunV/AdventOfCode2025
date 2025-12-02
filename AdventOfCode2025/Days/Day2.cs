using System;
using System.Collections.Generic;
using System.Linq;
using Common.Extensions;

namespace AdventOfCode2025.Days;

public class Day2 : Day
{
    private readonly IEnumerable<long[]> _ranges;

    public Day2()
    {
        _ranges = GetInputRaw()
            .GetNumbers<long>()
            .Select(Math.Abs)
            .Chunk(2);
    }

    public override string Part1()
    {
        var totalInvalid = _ranges
            .Select(longs => GetInvalidIdsPart1(longs).Sum())
            .Sum();

        return totalInvalid.ToString();
    }

    public override string Part2()
    {
        var totalInvalid = _ranges
            .Select(longs => GetInvalidIdsPart2(longs).Sum())
            .Sum();

        return totalInvalid.ToString();
    }

    private static IEnumerable<long> GetInvalidIdsPart1(long[] range)
    {
        var id = range[0];
        var digitAmount = id.ToString().Length;

        if (digitAmount % 2 != 0)
        {
            id = (int)Math.Pow(10, digitAmount);
        }

        while (id <= range[1])
        {
            var strId = id.ToString();
            digitAmount = strId.Length;
            if (digitAmount % 2 != 0)
            {
                id = (int)Math.Pow(10, digitAmount);
                continue;
            }

            if (CheckSpan(strId, digitAmount / 2))
            {
                yield return id;
            }

            id++;
        }
    }

    private static IEnumerable<long> GetInvalidIdsPart2(long[] range)
    {
        var id = range[0];
        var digitAmount = 0;
        IReadOnlyCollection<int> steps = [];

        while (id <= range[1])
        {
            var strId = id.ToString();
            if (digitAmount != strId.Length)
            {
                steps = Enumerable.Range(1, strId.Length / 2).ToList();
                digitAmount = strId.Length;
            }

            if (CheckSpan(strId, steps))
            {
                yield return id;
            }

            id++;
        }
    }

    private static bool CheckSpan(ReadOnlySpan<char> span, params IReadOnlyCollection<int> steps)
    {
        foreach (var step in steps)
        {
            if (span.Length % step != 0)
            {
                continue;
            }

            if (CheckChunks(span, step))
            {
                return true;
            }
        }

        return false;
    }

    private static bool CheckChunks(ReadOnlySpan<char> span, int step)
    {
        for (var i = 0; i < step; i++)
        {
            for (var j = step + i; j < span.Length; j += step)
            {
                if (span[i] != span[j])
                {
                    return false;
                }
            }
        }

        return true;
    }
}