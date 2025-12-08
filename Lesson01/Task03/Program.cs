using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Text Analysis");
        Console.WriteLine("Enter text (press Enter twice to finish):");

        string text = "";
        string line;
        while ((line = Console.ReadLine()) != "")
        {
            if (text.Length > 0)
                text += "\n";
            text += line;
        }

        int totalChars = text.Length;
        int spaces = 0;
        int visibleChars = 0;
        int lines = text.Length > 0 ? 1 : 0;

        foreach (char c in text)
        {
            if (c == ' ')
                spaces++;
            if (c == '\n')
                lines++;
            if (!char.IsWhiteSpace(c))
                visibleChars++;
        }

        string[] words = text.Split(new char[] { ' ', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        int wordCount = words.Length;

        Console.WriteLine($"\nResults:");
        Console.WriteLine($"Words: {wordCount}");
        Console.WriteLine($"Spaces: {spaces}");
        Console.WriteLine($"Total characters: {totalChars}");
        Console.WriteLine($"Visible characters: {visibleChars}");
        Console.WriteLine($"Lines: {lines}");
    }
}