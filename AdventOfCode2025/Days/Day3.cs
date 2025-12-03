using System;
using System.Linq;
using System.Text;
using Common.Extensions;

namespace AdventOfCode2025.Days;

public class Day3 : Day
{
    private readonly string[] _input;
    private const int Part1Amount = 2;
    private const int Part2Amount = 12;

    public Day3()
    {
        _input = GetInput();
    }

    public override string Part1()
    {
        return _input
            .Select(battery => GetJoltage(battery, Part1Amount))
            .Sum()
            .ToString();
    }

    public override string Part2()
    {
        return _input
            .Select(battery => GetJoltage(battery, Part2Amount))
            .Sum()
            .ToString();
    }

    private static long GetJoltage(ReadOnlySpan<char> battery, int amount)
    {
        var builder = new StringBuilder();
        var start = 0;
        var end = battery.Length - amount + 1;
        for (var i = 0; i < amount; i++)
        {
            var (max, index) = battery.MaxWithIndex(start, end);
            start = index + 1;
            end = battery.Length - amount + i + 2;
            builder.Append(max);
        }

        return long.Parse(builder.ToString());
    }
}