using System;
using System.Collections.Generic;
using System.Linq;
using Common.Extensions;
using Common.Models;

namespace AdventOfCode2025.Days;

public class Day8 : Day
{
    private readonly HashSet<Vector3<long>> _coords;
    private const int Part1Count = 1000;

    public Day8()
    {
        _coords = GetInput()
            .Select(line => line.GetNumbers<long>())
            .Select(numbers => new Vector3<long>(numbers[0], numbers[1], numbers[2]))
            .ToHashSet();
    }

    public override string Part1()
    {
        var pairs = GetOrderedPairs()
            .Take(Part1Count * 2)
            .Where((_, index) => index % 2 == 0);

        var graph = CreateGraph(pairs);

        var toVisit = new HashSet<Vector3<long>>(graph.Keys);
        var circuits = new List<long>();
        while (toVisit.Count > 0)
        {
            var current = toVisit.First();
            var connected = GetConnected(current, graph);
            toVisit = toVisit.Except(connected).ToHashSet();
            circuits.Add(connected.Count);
        }

        return circuits.OrderDescending().Take(3).Product().ToString();
    }

    public override string Part2()
    {
        var pairs = GetOrderedPairs();
        var graph = new Dictionary<Vector3<long>, HashSet<Vector3<long>>>();
        var connected = new HashSet<Vector3<long>>();
        foreach (var (index, (left, right)) in pairs.Index())
        {
            AddToGraph(graph, left, right);
            if (index <= Part1Count)
            {
                continue;
            }

            if (connected.Contains(left) && connected.Contains(right))
            {
                continue;
            }

            connected = GetConnected(left, graph);
            if (connected.Count == _coords.Count)
            {
                return (left.X * right.X).ToString();
            }
        }

        throw new ArgumentOutOfRangeException(nameof(pairs), "Unable to find last pair that connects the circuit.");
    }

    private static HashSet<Vector3<long>> GetConnected(Vector3<long> start,
        IReadOnlyDictionary<Vector3<long>, HashSet<Vector3<long>>> graph)
    {
        var seen = new HashSet<Vector3<long>>();
        var toVisit = new Queue<Vector3<long>>();
        toVisit.Enqueue(start);
        while (toVisit.TryDequeue(out var next))
        {
            if (!seen.Add(next))
            {
                continue;
            }

            foreach (var connected in graph[next])
            {
                toVisit.Enqueue(connected);
            }
        }

        return seen;
    }

    private static Dictionary<Vector3<long>, HashSet<Vector3<long>>> CreateGraph(
        IEnumerable<(Vector3<long>, Vector3<long>)> pairs)
    {
        var graph = new Dictionary<Vector3<long>, HashSet<Vector3<long>>>();
        foreach (var (left, right) in pairs)
        {
            AddToGraph(graph, left, right);
        }

        return graph;
    }

    private static void AddToGraph(Dictionary<Vector3<long>, HashSet<Vector3<long>>> graph,
        Vector3<long> left, Vector3<long> right)
    {
        if (!graph.TryAdd(left, [right]))
        {
            graph[left].Add(right);
        }

        if (!graph.TryAdd(right, [left]))
        {
            graph[right].Add(left);
        }
    }

    private IEnumerable<(Vector3<long>, Vector3<long>)> GetOrderedPairs()
    {
        return _coords
            .DoubleIteration()
            .OrderBy(pair => (pair.Item1 - pair.Item2)
                .LengthSquared());
    }
}