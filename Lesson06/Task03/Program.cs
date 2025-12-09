using System;
using System.IO;

class TempFileWriter : IDisposable
{
    private StreamWriter _writer;
    private bool _disposed;

    public string FilePath { get; }

    public TempFileWriter()
    {
        FilePath = Path.GetTempFileName();
        _writer = new StreamWriter(FilePath);
    }

    public void WriteLine(string text)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(TempFileWriter));
        _writer.WriteLine(text);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _writer?.Close();
            _disposed = true;
        }
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== TempFileWriter Demo ===\n");

        string path;
        using (TempFileWriter writer = new TempFileWriter())
        {
            path = writer.FilePath;
            writer.WriteLine("Hello");
            writer.WriteLine("World");
            Console.WriteLine($"Wrote to: {path}");
        }

        Console.WriteLine("File contents:");
        foreach (string line in File.ReadAllLines(path))
            Console.WriteLine($"  {line}");

        File.Delete(path);

        Console.WriteLine("\nTesting ObjectDisposedException:");
        TempFileWriter disposed = new TempFileWriter();
        string tempPath = disposed.FilePath;
        disposed.Dispose();

        try
        {
            disposed.WriteLine("Should fail");
        }
        catch (ObjectDisposedException)
        {
            Console.WriteLine("  ObjectDisposedException caught!");
        }

        File.Delete(tempPath);
    }
}