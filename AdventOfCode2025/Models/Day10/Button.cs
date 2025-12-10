using System.Collections.Generic;
using System.Linq;
using Common.Extensions;

namespace AdventOfCode2025.Models.Day10;

public record Button(HashSet<int> Toggles, int TogglesBit)
{
    public static Button Parse(string input)
    {
        var numbers = input.GetNumbers<int>();
        var toggles = numbers.Sum(number => 1 << number);

        return new Button(numbers.ToHashSet(), toggles);
    }
}