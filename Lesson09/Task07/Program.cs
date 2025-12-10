using System;
using System.Text.Json;

class FlexibleContainer<T>
{
    public T Value { get; set; }

    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }

    public static FlexibleContainer<T> FromJson(string json)
    {
        return JsonSerializer.Deserialize<FlexibleContainer<T>>(json);
    }
}

class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Flexible Container JSON ===\n");

        var container1 = new FlexibleContainer<int> { Value = 42 };
        string json1 = container1.ToJson();
        Console.WriteLine($"Int container: {json1}");

        var restored1 = FlexibleContainer<int>.FromJson(json1);
        Console.WriteLine($"Restored value: {restored1.Value}");

        Console.WriteLine();

        var container2 = new FlexibleContainer<Person>
        {
            Value = new Person { Name = "John", Age = 30 }
        };
        string json2 = container2.ToJson();
        Console.WriteLine($"Person container: {json2}");

        var restored2 = FlexibleContainer<Person>.FromJson(json2);
        Console.WriteLine($"Restored: Name={restored2.Value.Name}, Age={restored2.Value.Age}");

        Console.WriteLine("\nNote: In Level 2, enable PublishTrimmed to see potential issues");
    }
}