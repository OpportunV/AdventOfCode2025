using System.Linq;
using AdventOfCode2025.Models.Day10;

namespace AdventOfCode2025.Days;

public class Day10 : Day
{
    private readonly Machine[] _machines;

    public Day10()
    {
        _machines = GetInput().Select(Machine.Parse).ToArray();
    }

    public override string Part1()
    {
        return _machines
            .Select(machine => machine.TurnOn())
            .Sum()
            .ToString();
    }

    public override string Part2()
    {
        return _machines
            .Select(machine => machine.AdjustJoltage())
            .Sum()
            .ToString();
    }
}