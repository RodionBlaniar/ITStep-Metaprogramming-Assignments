using System;
using System.IO;
using System.Threading;

class Program
{
    static string inboxPath = "inbox";
    static string processedPath = "processed";

    static void Main()
    {
        Console.WriteLine("=== Mini CSV Importer ===\n");

        Directory.CreateDirectory(inboxPath);
        Directory.CreateDirectory(processedPath);

        using (FileSystemWatcher watcher = new FileSystemWatcher())
        {
            watcher.Path = inboxPath;
            watcher.Filter = "*.csv";
            watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;

            watcher.Created += OnFileDetected;
            watcher.Changed += OnFileDetected;

            watcher.EnableRaisingEvents = true;

            Console.WriteLine($"Watching folder: {Path.GetFullPath(inboxPath)}");
            Console.WriteLine("Drop .csv files into inbox/ folder");
            Console.WriteLine("Press Enter to create test file, or Q to quit.\n");

            while (true)
            {
                string input = Console.ReadLine();
                if (input.ToUpper() == "Q")
                    break;

                string testFile = Path.Combine(inboxPath, "report.csv");
                File.WriteAllText(testFile, "id,name,value\n1,Test,100");
                Console.WriteLine($"Created test file: {testFile}\n");
            }
        }

        Console.WriteLine("Watcher stopped.");
    }

    static void OnFileDetected(object sender, FileSystemEventArgs e)
    {
        Console.WriteLine($"Detected: {e.Name}");

        Thread.Sleep(500);

        try
        {
            string date = DateTime.Now.ToString("yyyyMMdd");
            string newName = Path.GetFileNameWithoutExtension(e.Name) + "-" + date + ".csv";
            string destPath = Path.Combine(processedPath, newName);

            if (File.Exists(destPath))
                File.Delete(destPath);

            File.Move(e.FullPath, destPath);
            Console.WriteLine($"Processed: {e.Name} -> {newName}\n");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Error processing {e.Name}: {ex.Message}\n");
        }
    }
}