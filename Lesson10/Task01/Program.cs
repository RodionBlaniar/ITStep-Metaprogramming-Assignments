using System;
using System.Reflection;

class Student
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Group { get; set; }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Type Structure Analysis ===\n");

        Type type = typeof(Student);

        Console.WriteLine($"Class: {type.Name}\n");
        Console.WriteLine("Properties:");

        foreach (PropertyInfo prop in type.GetProperties())
        {
            Console.WriteLine($"  {prop.Name}: {prop.PropertyType.Name}");
        }
    }
}