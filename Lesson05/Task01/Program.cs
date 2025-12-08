using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Task 1: foreach vs Manual Iteration ===\n");

        List<int> numbers = new List<int> { 10, 20, 30, 40, 50 };

        Console.WriteLine("--- 1. Manual iteration with GetEnumerator/MoveNext/Current ---");
        ManualIteration(numbers);

        Console.WriteLine("\n--- 2. Iteration with foreach ---");
        ForeachIteration(numbers);

        Console.WriteLine("\n--- 3. Modifying collection during foreach (causes exception) ---");
        ModifyDuringForeach(numbers);

        Console.WriteLine("\n--- 4. Safe iteration methods ---");
        SafeIteration(numbers);

        Console.WriteLine("\n=== REPORT ===");
        Console.WriteLine("1. IEnumerable<T> interface provides GetEnumerator() method");
        Console.WriteLine("2. IEnumerator<T> interface provides MoveNext(), Current, Reset(), Dispose()");
        Console.WriteLine("3. foreach is syntactic sugar that compiles to manual enumerator code");
        Console.WriteLine("4. Modifying collection during enumeration throws InvalidOperationException");
        Console.WriteLine("5. Safe alternatives: iterate over snapshot (ToArray) or use index-based for loop");
    }

    static void ManualIteration(List<int> list)
    {
        IEnumerator<int> enumerator = list.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                int current = enumerator.Current;
                Console.WriteLine($"  Element: {current}");
            }
        }
        finally
        {
            enumerator.Dispose();
        }
        Console.WriteLine("  Manual iteration completed successfully.");
    }

    static void ForeachIteration(List<int> list)
    {
        foreach (int number in list)
        {
            Console.WriteLine($"  Element: {number}");
        }
        Console.WriteLine("  foreach iteration completed successfully.");
    }

    static void ModifyDuringForeach(List<int> list)
    {
        List<int> testList = new List<int> { 1, 2, 3, 4, 5 };

        try
        {
            foreach (int number in testList)
            {
                Console.WriteLine($"  Reading: {number}");
                if (number == 2)
                {
                    Console.WriteLine("  Attempting to add element during foreach...");
                    testList.Add(100);
                }
            }
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"  EXCEPTION CAUGHT: {ex.GetType().Name}");
            Console.WriteLine($"  Message: {ex.Message}");
        }
    }

    static void SafeIteration(List<int> list)
    {
        Console.WriteLine("\n  Method A: Iterate over snapshot (ToArray)");
        List<int> testList1 = new List<int> { 1, 2, 3 };
        int[] snapshot = testList1.ToArray();
        foreach (int number in snapshot)
        {
            Console.WriteLine($"    Reading: {number}");
            if (number == 2)
            {
                testList1.Add(100);
                Console.WriteLine("    Added 100 to original list (safe, iterating over snapshot)");
            }
        }
        Console.WriteLine($"    Final list count: {testList1.Count}");

        Console.WriteLine("\n  Method B: Index-based for loop");
        List<int> testList2 = new List<int> { 1, 2, 3 };
        for (int i = 0; i < testList2.Count; i++)
        {
            Console.WriteLine($"    Reading at index {i}: {testList2[i]}");
            if (testList2[i] == 2)
            {
                testList2.Add(200);
                Console.WriteLine("    Added 200 to list (safe with for loop, but be careful with indices)");
            }
        }
        Console.WriteLine($"    Final list count: {testList2.Count}");
    }
}