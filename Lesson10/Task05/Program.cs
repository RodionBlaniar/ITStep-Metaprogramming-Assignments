using System;

interface IPlugin
{
    void Execute();
}

class HelloPlugin : IPlugin
{
    public void Execute()
    {
        Console.WriteLine("Hello from HelloPlugin!");
    }
}

class TimePlugin : IPlugin
{
    public void Execute()
    {
        Console.WriteLine($"Current time: {DateTime.Now:HH:mm:ss}");
    }
}

class GreetPlugin : IPlugin
{
    public void Execute()
    {
        Console.WriteLine("Greetings, user!");
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Plugin System ===\n");

        IPlugin[] plugins = new IPlugin[]
        {
            new HelloPlugin(),
            new TimePlugin(),
            new GreetPlugin()
        };

        foreach (IPlugin plugin in plugins)
        {
            Console.Write($"{plugin.GetType().Name}: ");
            plugin.Execute();
        }
    }
}