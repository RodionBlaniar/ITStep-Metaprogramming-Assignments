using System;
using System.Collections.Generic;
using System.Linq;

record Product(int Id, string Name, string Category, decimal Price, bool IsActive);

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Product Filter ===\n");

        List<Product> products = new List<Product>
        {
            new Product(1, "C# Guide", "Books", 29.99m, true),
            new Product(2, "Python Basics", "Books", 24.99m, true),
            new Product(3, "Laptop", "Electronics", 999.99m, true),
            new Product(4, "Java Manual", "Books", 34.99m, false),
            new Product(5, "Clean Code", "Books", 39.99m, true),
            new Product(6, "Mouse", "Electronics", 19.99m, true),
            new Product(7, "Design Patterns", "Books", 44.99m, true),
            new Product(8, "Algorithms", "Books", 54.99m, true)
        };

        decimal minPrice = 25.00m;
        decimal maxPrice = 50.00m;

        Func<Product, bool> filter = p =>
            p.IsActive &&
            p.Category == "Books" &&
            p.Price >= minPrice &&
            p.Price <= maxPrice;

        var result = products
            .Where(filter)
            .OrderBy(p => p.Price)
            .ThenBy(p => p.Name)
            .Select(p => new { p.Id, p.Name, p.Price })
            .ToList();

        Console.WriteLine($"Active Books, price {minPrice}-{maxPrice}:");
        Console.WriteLine("(Sorted by Price, then Name)\n");

        foreach (var p in result)
        {
            Console.WriteLine($"  #{p.Id} {p.Name}: {p.Price:C}");
        }
    }
}