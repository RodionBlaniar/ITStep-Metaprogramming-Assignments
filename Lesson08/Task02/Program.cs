using System;

public delegate void Notifier(string message);

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Events and Delegates ===\n");

        Notifier notifier = null;

        notifier += NamedHandler;

        notifier += delegate (string msg)
        {
            Console.WriteLine($"[Anonymous] {msg}");
        };

        notifier += (msg) => Console.WriteLine($"[Lambda] {msg}");

        Console.WriteLine("Invoking multicast delegate:\n");
        notifier("Hello from all handlers!");

        Console.WriteLine("\nSending another message:\n");
        notifier("Second notification");
    }

    static void NamedHandler(string message)
    {
        Console.WriteLine($"[Named] {message}");
    }
}