using System;

[AttributeUsage(AttributeTargets.Property)]
class RequiredAttribute : Attribute
{
}

class User
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    public string Phone { get; set; }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Custom Attributes ===\n");

        Type type = typeof(User);

        Console.WriteLine($"Class: {type.Name}\n");
        Console.WriteLine("Properties:");

        foreach (var prop in type.GetProperties())
        {
            bool isRequired = Attribute.IsDefined(prop, typeof(RequiredAttribute));
            string marker = isRequired ? "[Required]" : "";
            Console.WriteLine($"  {prop.Name} {marker}");
        }
    }
}