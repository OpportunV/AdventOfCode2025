using System.Collections.Generic;
using System.Linq;
using Common.Models;

namespace AdventOfCode2025.Days;

public class Day4 : Day
{
    private readonly Grid<char> _grid;
    private const int Amount = 4;
    private const char PaperRoll = '@';
    private const char Empty = '.';

    public Day4()
    {
        _grid = new Grid<char>(GetInput().Select(line => line.ToCharArray()).ToArray());
    }

    public override string Part1()
    {
        return GetRemovableRolls()
            .Count()
            .ToString();
    }

    public override string Part2()
    {
        var counter = 0;
        List<GridItem<char>> removableRolls;
        do
        {
            removableRolls = GetRemovableRolls().ToList();
            counter += removableRolls.Count;
            removableRolls.ForEach(removableRoll => _grid[removableRoll.Pos] = Empty);
        } while (removableRolls.Count > 0);

        return counter.ToString();
    }

    private IEnumerable<GridItem<char>> GetRemovableRolls()
    {
        return _grid
            .Flatten()
            .Where(item => item.Value == PaperRoll
                           && _grid.AdjacentAll(item)
                               .Count(adjacent => adjacent.Value == PaperRoll) < Amount);
    }
}