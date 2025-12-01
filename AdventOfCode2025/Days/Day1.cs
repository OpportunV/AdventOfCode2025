using System;
using Common.Extensions;

namespace AdventOfCode2025.Days;

public class Day1 : Day
{
    private readonly string[] _input;
    private const int Length = 100;
    private const int StartPos = 50;

    public Day1()
    {
        _input = GetInput();
    }

    public override string Part1()
    {
        var pos = StartPos;
        var counter = 0;
        foreach (var line in _input)
        {
            var displacement = GetDisplacement(line);
            pos += displacement;
            if (pos % Length == 0)
            {
                counter++;
            }
        }

        return counter.ToString();
    }

    public override string Part2()
    {
        var pos = StartPos;
        var counter = 0;
        foreach (var line in _input)
        {
            var displacement = GetDisplacement(line);
            var newPos = pos + displacement;
            var (quotient, reminder) = Math.DivRem(newPos, Length);
            if (reminder < 0)
            {
                quotient--;
                reminder = reminder.Mod(Length);
            }

            counter += Math.Abs(quotient);
            if (displacement < 0)
            {
                counter -= pos == 0 ? 1 : 0;
                counter += reminder == 0 ? 1 : 0;
            }

            pos = newPos.Mod(Length);
        }

        return counter.ToString();
    }

    private static int GetDisplacement(ReadOnlySpan<char> line)
    {
        var direction = line[0];
        var amount = int.Parse(line[1..]);
        return direction switch
        {
            'L' => -amount,
            'R' => amount,
            _ => throw new ArgumentOutOfRangeException(nameof(line))
        };
    }
}