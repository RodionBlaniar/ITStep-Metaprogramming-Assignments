using System;
using System.Runtime.CompilerServices;

class Program
{
    static (string key, string value) ParseSetting(
        string line,
        [CallerArgumentExpression("line")] string paramName = null)
    {
        if (string.IsNullOrEmpty(line))
            throw new ArgumentNullException(nameof(line));

        int index = line.IndexOf('=');
        if (index < 0)
            throw new FormatException($"Parameter '{paramName}' must contain '=' character");

        string key = line.Substring(0, index).Trim();
        string value = line.Substring(index + 1).Trim();
        return (key, value);
    }

    static void Main()
    {
        Console.WriteLine("=== Config Parser ===\n");

        var result = ParseSetting("name=John");
        Console.WriteLine($"Parsed: key=\"{result.key}\", value=\"{result.value}\"");

        try
        {
            ParseSetting(null);
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine($"Null input: {ex.GetType().Name}");
        }

        try
        {
            ParseSetting("no-equals-here");
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"No equals: {ex.Message}");
        }
    }
}