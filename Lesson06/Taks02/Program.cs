using System;

class Program
{
    static int AddChecked(int a, int b)
    {
        checked { return a + b; }
    }

    static int AddWrapped(int a, int b)
    {
        unchecked { return a + b; }
    }

    static void Main()
    {
        Console.WriteLine("=== Overflow Checked/Unchecked ===\n");

        Console.WriteLine("Test 1: Normal (5 + 3)");
        Console.WriteLine($"  Checked: {AddChecked(5, 3)}");
        Console.WriteLine($"  Wrapped: {AddWrapped(5, 3)}");

        Console.WriteLine("\nTest 2: int.MaxValue + 1");
        try
        {
            Console.WriteLine($"  Checked: {AddChecked(int.MaxValue, 1)}");
        }
        catch (OverflowException)
        {
            Console.WriteLine("  Checked: OverflowException");
        }
        Console.WriteLine($"  Wrapped: {AddWrapped(int.MaxValue, 1)}");

        Console.WriteLine("\nTest 3: int.MinValue + (-1)");
        try
        {
            Console.WriteLine($"  Checked: {AddChecked(int.MinValue, -1)}");
        }
        catch (OverflowException)
        {
            Console.WriteLine("  Checked: OverflowException");
        }
        Console.WriteLine($"  Wrapped: {AddWrapped(int.MinValue, -1)}");

        Console.WriteLine("\nTest 4: int.MaxValue + int.MaxValue");
        try
        {
            Console.WriteLine($"  Checked: {AddChecked(int.MaxValue, int.MaxValue)}");
        }
        catch (OverflowException)
        {
            Console.WriteLine("  Checked: OverflowException");
        }
        Console.WriteLine($"  Wrapped: {AddWrapped(int.MaxValue, int.MaxValue)}");
    }
}