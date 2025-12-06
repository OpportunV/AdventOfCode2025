using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Extensions;
using Common.Models;

namespace AdventOfCode2025.Days;

public class Day6 : Day
{
    private readonly string[] _input;
    private const string Addition = "+";
    private const string Multiplication = "*";

    public Day6()
    {
        _input = GetInput();
    }

    public override string Part1()
    {
        var parsed = _input
            .Select(line => line.Split(" ", StringSplitOptions.RemoveEmptyEntries))
            .ToArray();
        var data = new Grid<string>(parsed);
        return data
            .VerticalFlatten()
            .Select(item => item.Value)
            .Chunk(data.Rows)
            .Sum(CalculateCol)
            .ToString();
    }

    public override string Part2()
    {
        return ProcessFullSheet(_input).Sum().ToString();
    }

    private long CalculateCol(IList<string> col)
    {
        var numbers = col.SkipLast(1);
        return col[^1] switch
        {
            Multiplication => numbers.Select(long.Parse).Product(),
            Addition => numbers.Select(long.Parse).Sum(),
            _ => throw new ArgumentOutOfRangeException(nameof(col), col[0], null)
        };
    }

    private IEnumerable<long> ProcessFullSheet(string[] sheet)
    {
        var rows = sheet[0].Length;
        var cols = sheet.Length;
        var currentData = new List<string>();
        var sb = new StringBuilder();
        for (var row = rows - 1; row >= 0; row--)
        {
            sb.Clear();
            for (var col = 0; col < cols; col++)
            {
                var current = sheet[col][row];
                if (char.IsDigit(current))
                {
                    sb.Append(current);
                }
                else if (!char.IsWhiteSpace(current))
                {
                    currentData.Add(sb.ToString());
                    currentData.Add(current.ToString());
                    yield return CalculateCol(currentData);
                    currentData.Clear();
                    sb.Clear();
                }
            }

            if (sb.Length > 0)
            {
                currentData.Add(sb.ToString());
            }
        }
    }
}