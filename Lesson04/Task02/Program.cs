using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

class Program
{
    const int N = 5_000_000;
    const int RUNS = 5;

    static void Main()
    {
        Console.WriteLine("=== Boxing vs Generics Benchmark ===\n");
        Console.WriteLine($"N = {N:N0} elements, {RUNS} runs each\n");

        int[] data = GenerateData(N);

        Console.WriteLine("Warming up JIT...");
        RunArrayList(data);
        RunGenericList(data);
        Console.WriteLine("Warmup complete.\n");

        Console.WriteLine("--- Running Benchmarks ---\n");

        long[] arrayListTimes = new long[RUNS];
        long[] genericListTimes = new long[RUNS];

        for (int i = 0; i < RUNS; i++)
        {
            arrayListTimes[i] = RunArrayList(data);
            genericListTimes[i] = RunGenericList(data);
        }

        double avgArrayList = Average(arrayListTimes);
        double avgGenericList = Average(genericListTimes);

        Console.WriteLine("=== RESULTS ===\n");
        Console.WriteLine("| Scenario      | Run 1 | Run 2 | Run 3 | Run 4 | Run 5 | Average |");
        Console.WriteLine("|---------------|-------|-------|-------|-------|-------|---------|");
        Console.WriteLine($"| ArrayList     | {arrayListTimes[0],5} | {arrayListTimes[1],5} | {arrayListTimes[2],5} | {arrayListTimes[3],5} | {arrayListTimes[4],5} | {avgArrayList,7:F1} |");
        Console.WriteLine($"| List<int>     | {genericListTimes[0],5} | {genericListTimes[1],5} | {genericListTimes[2],5} | {genericListTimes[3],5} | {genericListTimes[4],5} | {avgGenericList,7:F1} |");
        Console.WriteLine();
        Console.WriteLine($"ArrayList is {avgArrayList / avgGenericList:F2}x slower than List<int>");

        Console.WriteLine("\n=== BOXING ANALYSIS ===\n");
        AnalyzeBoxing();

        Console.WriteLine("\n=== STRING FORMATTING BOXING ===\n");
        StringFormattingBoxing();

        Console.WriteLine("\n=== EXPLANATION ===\n");
        PrintExplanation();
    }

    static int[] GenerateData(int n)
    {
        Console.WriteLine($"Generating {n:N0} random integers...");
        Random rand = new Random(42);
        int[] data = new int[n];
        for (int i = 0; i < n; i++)
            data[i] = rand.Next(1, 1000);
        Console.WriteLine("Done.\n");
        return data;
    }

    static long RunArrayList(int[] data)
    {
        Stopwatch sw = Stopwatch.StartNew();

        ArrayList list = new ArrayList();
        for (int i = 0; i < data.Length; i++)
            list.Add(data[i]);

        long sum = 0;
        for (int i = 0; i < list.Count; i++)
            sum += (int)list[i];

        sw.Stop();
        return sw.ElapsedMilliseconds;
    }

    static long RunGenericList(int[] data)
    {
        Stopwatch sw = Stopwatch.StartNew();

        List<int> list = new List<int>();
        for (int i = 0; i < data.Length; i++)
            list.Add(data[i]);

        long sum = 0;
        for (int i = 0; i < list.Count; i++)
            sum += list[i];

        sw.Stop();
        return sw.ElapsedMilliseconds;
    }

    static double Average(long[] values)
    {
        long total = 0;
        for (int i = 0; i < values.Length; i++)
            total += values[i];
        return (double)total / values.Length;
    }

    static void AnalyzeBoxing()
    {
        Console.WriteLine("WHERE BOXING OCCURS IN ARRAYLIST:");
        Console.WriteLine();
        Console.WriteLine("1. ArrayList.Add(object value)");
        Console.WriteLine("   - Method signature expects 'object'");
        Console.WriteLine("   - When we call list.Add(intValue), the int is BOXED");
        Console.WriteLine("   - Boxing creates a new object on the heap");
        Console.WriteLine();
        Console.WriteLine("2. ArrayList indexer returns 'object'");
        Console.WriteLine("   - When we read: (int)list[i]");
        Console.WriteLine("   - We get an 'object' that must be UNBOXED back to int");
        Console.WriteLine();
        Console.WriteLine($"With N = {N:N0}:");
        Console.WriteLine($"   - {N:N0} boxing operations during Add()");
        Console.WriteLine($"   - {N:N0} unboxing operations during read");
        Console.WriteLine($"   - Total: {N * 2:N0} boxing/unboxing operations");
        Console.WriteLine();
        Console.WriteLine("WHY LIST<int> IS FASTER:");
        Console.WriteLine();
        Console.WriteLine("1. List<int>.Add(int value)");
        Console.WriteLine("   - Method signature expects 'int' directly");
        Console.WriteLine("   - No boxing needed, value stored as-is");
        Console.WriteLine();
        Console.WriteLine("2. List<int> indexer returns 'int'");
        Console.WriteLine("   - No unboxing needed");
        Console.WriteLine("   - Direct memory access");
    }

    static void StringFormattingBoxing()
    {
        int x = 42;

        Console.WriteLine("Example 1: string.Format with boxing");
        Console.WriteLine("   Code: string.Format(\"X={0}\", x)");
        Console.WriteLine("   Boxing: YES - Format expects 'object', so int is boxed");
        string s1 = string.Format("X={0}", x);
        Console.WriteLine($"   Result: {s1}");
        Console.WriteLine();

        Console.WriteLine("Example 2: String interpolation");
        Console.WriteLine("   Code: $\"X={x}\"");
        Console.WriteLine("   Boxing: In older .NET - YES, In .NET 6+ - optimized, often NO");
        string s2 = $"X={x}";
        Console.WriteLine($"   Result: {s2}");
        Console.WriteLine();

        Console.WriteLine("Example 3: Concatenation with ToString()");
        Console.WriteLine("   Code: \"X=\" + x.ToString()");
        Console.WriteLine("   Boxing: NO - ToString() is called directly on int");
        string s3 = "X=" + x.ToString();
        Console.WriteLine($"   Result: {s3}");
        Console.WriteLine();

        Console.WriteLine("RECOMMENDATION:");
        Console.WriteLine("   - In hot paths, use .ToString() explicitly");
        Console.WriteLine("   - Modern .NET optimizes interpolation well");
        Console.WriteLine("   - Avoid string.Format in performance-critical code");
    }

    static void PrintExplanation()
    {
        Console.WriteLine("CONCLUSIONS:");
        Console.WriteLine();
        Console.WriteLine("1. Generics eliminate boxing/unboxing overhead:");
        Console.WriteLine("   - ArrayList stores 'object', requiring boxing for value types");
        Console.WriteLine("   - List<int> stores 'int' directly, no conversion needed");
        Console.WriteLine();
        Console.WriteLine("2. Performance difference comes from:");
        Console.WriteLine("   - Memory allocation: boxing creates heap objects");
        Console.WriteLine("   - GC pressure: boxed objects need garbage collection");
        Console.WriteLine("   - CPU overhead: boxing/unboxing instructions");
        Console.WriteLine();
        Console.WriteLine("3. Other places where hidden boxing can occur:");
        Console.WriteLine("   - Casting value types to interfaces (e.g., int to IComparable)");
        Console.WriteLine("   - Using value types with non-generic collections (Hashtable, etc.)");
        Console.WriteLine("   - String formatting with value type arguments");
        Console.WriteLine("   - Calling object methods (GetType()) on value types");
        Console.WriteLine("   - Using 'object' parameters or return types");
        Console.WriteLine();
        Console.WriteLine("4. Best practices:");
        Console.WriteLine("   - Always use generic collections for value types");
        Console.WriteLine("   - Use generic interfaces (IComparable<T>) instead of non-generic");
        Console.WriteLine("   - Be aware of boxing in string operations");
        Console.WriteLine("   - Profile code to find hidden boxing in hot paths");
    }
}