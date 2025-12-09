using System;

class Program
{
    static string TrimToUpper(string s)
    {
        return s.Trim().ToUpper();
    }

    static string MaskDigits(string s)
    {
        char[] chars = s.ToCharArray();
        for (int i = 0; i < chars.Length; i++)
        {
            if (char.IsDigit(chars[i]))
                chars[i] = '*';
        }
        return new string(chars);
    }

    static string Transform(string s, Func<string, string> strategy)
    {
        return strategy(s);
    }

    static void Main()
    {
        Console.WriteLine("=== Delegate Strategies ===\n");

        string input = "  Hello World 123  ";
        Console.WriteLine($"Input: \"{input}\"\n");

        string result1 = Transform(input, TrimToUpper);
        Console.WriteLine($"TrimToUpper: \"{result1}\"");

        string result2 = Transform(input, MaskDigits);
        Console.WriteLine($"MaskDigits:  \"{result2}\"");
    }
}