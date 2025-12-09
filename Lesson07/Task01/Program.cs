using System;
using System.IO;
using System.Text;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Text File Normalizer ===\n");

        string inputPath = "input.txt";
        string outputPath = "output.txt";

        File.WriteAllText(inputPath, "Line one   \r\nLine two  \r\nLine three   \n", Encoding.UTF8);
        Console.WriteLine($"Created test file: {inputPath}");

        int lineCount = 0;
        int spacesRemoved = 0;
        StringBuilder result = new StringBuilder();

        using (StreamReader reader = new StreamReader(inputPath, detectEncodingFromByteOrderMarks: true))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                int originalLength = line.Length;
                string trimmed = line.TrimEnd();
                spacesRemoved += originalLength - trimmed.Length;

                if (result.Length > 0)
                    result.Append('\n');
                result.Append(trimmed);

                lineCount++;
            }
        }

        using (StreamWriter writer = new StreamWriter(outputPath, false, new UTF8Encoding(false)))
        {
            writer.Write(result.ToString());
        }

        Console.WriteLine($"\nResults:");
        Console.WriteLine($"  Lines processed: {lineCount}");
        Console.WriteLine($"  Trailing spaces removed: {spacesRemoved}");
        Console.WriteLine($"  Output file: {outputPath} (UTF-8 without BOM, LF line endings)");

        Console.WriteLine($"\nOutput file contents:");
        Console.WriteLine(File.ReadAllText(outputPath).Replace("\n", "\\n\n"));
    }
}