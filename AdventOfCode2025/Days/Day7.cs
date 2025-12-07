using System.Collections.Generic;
using System.Linq;
using Common.Models;

namespace AdventOfCode2025.Days;

public class Day7 : Day
{
    private readonly Grid<char> _map;
    private readonly GridItem<char> _start;
    private const char SplitSymbol = '^';
    private const char StartSymbol = 'S';
    private string _part1Answer = string.Empty;
    private string _part2Answer = string.Empty;

    public Day7()
    {
        var data = GetInput().Select(line => line.ToCharArray()).ToArray();
        _map = new Grid<char>(data);
        _start = _map.Flatten().First(item => item.Value == StartSymbol);
    }

    public override string Part1()
    {
        if (string.IsNullOrEmpty(_part1Answer))
        {
            SimulateBeam();
        }

        return _part1Answer;
    }

    public override string Part2()
    {
        if (string.IsNullOrEmpty(_part2Answer))
        {
            SimulateBeam();
        }

        return _part2Answer;
    }

    private void SimulateBeam()
    {
        var splitCounter = 0;
        var timelineCounter = 0L;
        var currentBeams = new Dictionary<GridPos2d, long> { { _start.Pos, 1 } };

        while (currentBeams.Count > 0)
        {
            var newBeams = new Dictionary<GridPos2d, long>();
            foreach (var (beamPos, depth) in currentBeams)
            {
                var nextPos = beamPos + GridPos2d.Down;
                if (!_map.Contains(nextPos))
                {
                    timelineCounter += depth;
                    continue;
                }

                List<GridPos2d> newPoses;
                switch (_map[nextPos])
                {
                    case SplitSymbol:
                        splitCounter++;
                        newPoses = [nextPos + GridPos2d.Left, nextPos + GridPos2d.Right];
                        break;
                    default:
                        newPoses = [nextPos];
                        break;
                }

                foreach (var newPos in newPoses)
                {
                    if (!newBeams.TryAdd(newPos, depth))
                    {
                        newBeams[newPos] += depth;
                    }
                }
            }

            currentBeams = newBeams;
        }

        _part1Answer = splitCounter.ToString();
        _part2Answer = timelineCounter.ToString();
    }
}