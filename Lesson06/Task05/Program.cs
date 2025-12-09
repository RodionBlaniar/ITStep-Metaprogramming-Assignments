using System;

class Counter
{
    private int _value;

    public event EventHandler<int> Changed;

    public void Increment()
    {
        _value++;
        Changed?.Invoke(this, _value);
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Counter Events ===\n");

        Counter counter = new Counter();

        counter.Changed += (sender, value) =>
            Console.WriteLine($"Handler 1: Value is now {value}");

        counter.Changed += (sender, value) =>
            Console.WriteLine($"Handler 2: Counter incremented!");

        Console.WriteLine("Incrementing 3 times:\n");
        counter.Increment();
        counter.Increment();
        counter.Increment();
    }
}