using System;
using System.IO;
using System.Text;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Config File Update ===\n");

        string configPath = "appsettings.txt";

        string[] configLines = new string[]
        {
            "SERVER=localhost",
            "PORT=8080",
            "DEBUG=true"
        };

        Console.WriteLine("Configuration to write:");
        foreach (string line in configLines)
            Console.WriteLine($"  {line}");

        using (FileStream fs = new FileStream(configPath, FileMode.Create, FileAccess.Write, FileShare.None))
        using (StreamWriter writer = new StreamWriter(fs, new UTF8Encoding(false)))
        {
            foreach (string line in configLines)
                writer.WriteLine(line);

            writer.Flush();
        }

        Console.WriteLine($"\nWritten to: {configPath}");
        Console.WriteLine("File saved with exclusive access, UTF-8 without BOM");

        Console.WriteLine($"\nFile contents:");
        foreach (string line in File.ReadAllLines(configPath))
            Console.WriteLine($"  {line}");

        File.Delete(configPath);
        Console.WriteLine("\nCleaned up test file.");
    }
}