using System;

class Counter
{
    private int _value;

    public int Value
    {
        get { return this._value; }
    }

    public Counter() : this(0)
    {
    }

    public Counter(int start)
    {
        if (start < 0)
            throw new ArgumentException("Start value cannot be negative");
        this._value = start;
    }

    public void Increment()
    {
        this._value++;
    }

    public void Decrement()
    {
        if (this._value == 0)
            throw new InvalidOperationException("Counter cannot go below zero");
        this._value--;
    }

    public bool TryDecrement()
    {
        if (this._value == 0)
            return false;
        this._value--;
        return true;
    }

    public void Reset()
    {
        this._value = 0;
    }
}

class Program
{
    static void Main()
    {
        Counter counter = new Counter();
        Console.WriteLine($"Initial value: {counter.Value}");

        counter.Increment();
        counter.Increment();
        counter.Increment();
        Console.WriteLine($"After 3 increments: {counter.Value}");

        counter.Decrement();
        Console.WriteLine($"After decrement: {counter.Value}");

        bool success = counter.TryDecrement();
        Console.WriteLine($"TryDecrement success: {success}, Value: {counter.Value}");

        counter.Reset();
        Console.WriteLine($"After reset: {counter.Value}");

        success = counter.TryDecrement();
        Console.WriteLine($"TryDecrement at 0: {success}, Value: {counter.Value}");

        Counter counter2 = new Counter(10);
        Console.WriteLine($"Counter with start 10: {counter2.Value}");

        try
        {
            Counter invalid = new Counter(-5);
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Exception caught: {e.Message}");
        }

        try
        {
            counter.Decrement();
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine($"Exception caught: {e.Message}");
        }
    }
}