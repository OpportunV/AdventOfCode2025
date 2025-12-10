using System;
using System.Collections.Generic;
using System.Linq;
using Common.Extensions;
using Microsoft.Z3;

namespace AdventOfCode2025.Models.Day10;

public record Machine(int Lights, Button[] Buttons, int[] Joltages)
{
    private const char LightOn = '#';

    public static Machine Parse(string input)
    {
        var split = input.Split(" ");
        var lights = 0;
        foreach (var (i, chr) in split[0][1..^1].Index())
        {
            if (chr == LightOn)
            {
                lights += 1 << i;
            }
        }

        var buttons = split[1..^1]
            .Select(Button.Parse)
            .ToArray();

        var joltages = split[^1].GetNumbers<int>().ToArray();

        return new Machine(lights, buttons, joltages);
    }

    public int TurnOn()
    {
        return PressButtons();
    }

    public int AdjustJoltage()
    {
        return PressButtonsJoltage();
    }

    private int PressButtonsJoltage()
    {
        using var context = new Context();
        using var optimizer = context.MkOptimize();

        var buttonPresses = Buttons
            .Select(ArithExpr (_, i) => context.MkIntConst(i.ToString()))
            .ToArray();

        foreach (var buttonPress in buttonPresses)
        {
            optimizer.Add(buttonPress >= 0);
        }

        for (var i = 0; i < Joltages.Length; i++)
        {
            var switchingButtons = buttonPresses
                .Where((_, j) => Buttons[j].Toggles.Contains(i))
                .ToList();
            optimizer.Add(context.MkEq(context.MkAdd(switchingButtons), context.MkInt(Joltages[i])));
        }

        optimizer.MkMinimize(context.MkAdd(buttonPresses));
        optimizer.Check();

        return buttonPresses.Sum(p => ((IntNum)optimizer.Model.Eval(p)).Int);
    }

    private int PressButtons()
    {
        var toVisit = new Queue<int>();
        toVisit.Enqueue(0);
        var seen = new HashSet<int>();
        var counter = 0;
        while (toVisit.Count > 0)
        {
            var len = toVisit.Count;
            for (var i = 0; i < len; i++)
            {
                var cur = toVisit.Dequeue();
                foreach (var button in Buttons)
                {
                    var next = cur ^ button.TogglesBit;

                    if (next == Lights)
                    {
                        return ++counter;
                    }

                    if (seen.Add(next))
                    {
                        toVisit.Enqueue(next);
                    }
                }
            }

            counter++;
        }

        throw new Exception("Unable to enable all required lights.");
    }
}