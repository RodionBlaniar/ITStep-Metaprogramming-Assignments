using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

record Product(int Id, string Name, string Category, decimal Price);

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Expression Trees Filter ===\n");

        decimal minPrice = 20m;

        ParameterExpression param = Expression.Parameter(typeof(Product), "p");

        Expression priceProperty = Expression.Property(param, "Price");
        Expression minValue = Expression.Constant(minPrice);
        Expression priceCheck = Expression.GreaterThanOrEqual(priceProperty, minValue);

        Expression categoryProperty = Expression.Property(param, "Category");
        Expression booksValue = Expression.Constant("Books");
        Expression categoryCheck = Expression.Equal(categoryProperty, booksValue);

        Expression combined = Expression.AndAlso(priceCheck, categoryCheck);

        Expression<Func<Product, bool>> filterExpr =
            Expression.Lambda<Func<Product, bool>>(combined, param);

        Console.WriteLine($"Expression: {filterExpr}");

        Func<Product, bool> filter = filterExpr.Compile();

        List<Product> products = new List<Product>
        {
            new Product(1, "C# Guide", "Books", 29.99m),
            new Product(2, "Laptop", "Electronics", 999.99m),
            new Product(3, "Cheap Book", "Books", 9.99m),
            new Product(4, "Design Patterns", "Books", 44.99m),
            new Product(5, "Mouse", "Electronics", 19.99m)
        };

        Console.WriteLine($"\nFilter: Price >= {minPrice} AND Category == \"Books\"\n");

        Console.WriteLine("Results:");
        foreach (var p in products.Where(filter))
        {
            Console.WriteLine($"  {p.Name} ({p.Category}) - {p.Price:C}");
        }
    }
}