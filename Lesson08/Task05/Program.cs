using System;
using System.Collections.Generic;
using System.Linq;

record Item(int Id, string Name, int Value, bool Active);

class Program
{
    static void Main()
    {
        Console.WriteLine("=== LINQ Pipeline Optimization ===\n");

        List<Item> items = new List<Item>
        {
            new Item(1, "Alpha", 50, true),
            new Item(2, "Beta", 30, true),
            new Item(3, "Gamma", 80, false),
            new Item(4, "Delta", 60, true),
            new Item(5, "Epsilon", 40, true),
            new Item(6, "Zeta", 90, true)
        };

        Console.WriteLine("--- Naive Version (BAD) ---\n");

        var step1 = items.Where(x => x.Active).ToList();
        var step2 = step1.Where(x => x.Value > 35).ToList();
        var step3 = step2.OrderByDescending(x => x.Value).ToList();
        var step4 = step3.Select(x => new { x.Name, x.Value }).ToList();

        if (step4.Count() > 0)
        {
            foreach (var x in step4)
                Console.WriteLine($"  {x.Name}: {x.Value}");
        }

        Console.WriteLine("\n--- Optimized Version (GOOD) ---\n");

        var result = items
            .Where(x => x.Active)
            .Where(x => x.Value > 35)
            .OrderByDescending(x => x.Value)
            .Select(x => new { x.Name, x.Value })
            .ToList();

        if (result.Any())
        {
            foreach (var x in result)
                Console.WriteLine($"  {x.Name}: {x.Value}");
        }

        Console.WriteLine("\n--- Summary ---");
        Console.WriteLine("Naive: 4x ToList() in middle of pipeline");
        Console.WriteLine("Optimized: 1x ToList() at the end");
        Console.WriteLine("Replaced: Count() > 0 with Any()");
    }
}