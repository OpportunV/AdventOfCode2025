using System.Collections.Generic;
using System.Linq;
using Common.Models;

namespace AdventOfCode2025.Days;

public class Day4 : Day
{
    private readonly Grid<char> _grid;
    private const int Amount = 4;
    private const char Empty = '.';
    private const char PaperRoll = '@';

    public Day4()
    {
        _grid = new Grid<char>(GetInput().Select(line => line.ToCharArray()).ToArray());
    }

    public override string Part1()
    {
        return GetRemovableRolls(_grid.Flatten().Where(RollsFilter))
            .Count()
            .ToString();
    }

    public override string Part2()
    {
        var counter = 0;
        var removableRolls = GetRemovableRolls(_grid.Flatten().Where(RollsFilter)).ToList();
        do
        {
            counter += removableRolls.Count;
            removableRolls.ForEach(removableRoll => _grid[removableRoll.Pos] = Empty);
            var candidates = removableRolls
                .SelectMany(removableRoll => _grid
                    .AdjacentAll(removableRoll.Pos)
                    .Where(RollsFilter)).Distinct();
            removableRolls = GetRemovableRolls(candidates).ToList();
        } while (removableRolls.Count > 0);

        return counter.ToString();
    }

    private static bool RollsFilter(GridItem<char> item)
    {
        return item.Value == PaperRoll;
    }

    private IEnumerable<GridItem<char>> GetRemovableRolls(IEnumerable<GridItem<char>> candidates)
    {
        return candidates
            .Where(item => _grid.AdjacentAll(item)
                .Count(adjacent => adjacent.Value == PaperRoll) < Amount);
    }
}