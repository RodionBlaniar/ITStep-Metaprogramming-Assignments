#define TRACE

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

class DiagTimer
{
    private static Stopwatch _stopwatch;

    [Conditional("TRACE")]
    public static void Start()
    {
        _stopwatch = Stopwatch.StartNew();
    }

    [Conditional("TRACE")]
    public static void Stop(
        [CallerMemberName] string member = "",
        [CallerLineNumber] int line = 0)
    {
        if (_stopwatch != null)
        {
            _stopwatch.Stop();
            Console.WriteLine($"[TRACE] {member}() line {line}: {_stopwatch.ElapsedMilliseconds} ms");
            _stopwatch = null;
        }
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== DiagTimer with Conditional ===\n");

        Console.WriteLine("Running with TRACE defined:\n");

        DoWork();
        DoMoreWork();

        Console.WriteLine("\nNote: Remove #define TRACE to disable all tracing");
    }

    static void DoWork()
    {
        DiagTimer.Start();

        int sum = 0;
        for (int i = 0; i < 1000000; i++)
            sum += i;

        DiagTimer.Stop();
    }

    static void DoMoreWork()
    {
        DiagTimer.Start();

        System.Threading.Thread.Sleep(100);

        DiagTimer.Stop();
    }
}