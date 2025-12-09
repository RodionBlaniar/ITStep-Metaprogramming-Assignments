using System;
using System.Collections.Generic;
using System.Linq;

public static class StringExtensions
{
    public static string OrEmpty(this string s)
    {
        return s ?? "";
    }

    public static string NormalizeSpaces(this string s)
    {
        if (string.IsNullOrEmpty(s))
            return s;

        var words = s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        return string.Join(" ", words);
    }

    public static IEnumerable<string> Words(this string s)
    {
        if (string.IsNullOrEmpty(s))
            return Enumerable.Empty<string>();

        return s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(w => w.ToLower());
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Mini-DSL for Text ===\n");

        string input = "  Hello   World   hello   Test   world  TEST  ";
        Console.WriteLine($"Input: \"{input}\"\n");

        string normalized = input.OrEmpty().NormalizeSpaces();
        Console.WriteLine($"Normalized: \"{normalized}\"\n");

        var words = input
            .OrEmpty()
            .NormalizeSpaces()
            .Words()
            .Distinct()
            .OrderBy(w => w)
            .ToList();

        Console.WriteLine("Words (distinct, sorted):");
        foreach (var word in words)
        {
            Console.WriteLine($"  {word}");
        }

        Console.WriteLine("\nTesting null:");
        string nullStr = null;
        Console.WriteLine($"  null.OrEmpty() = \"{nullStr.OrEmpty()}\"");
    }
}