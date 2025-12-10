using System;

class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Dynamic Object Creation ===\n");

        object obj = Activator.CreateInstance(typeof(Product));

        Console.WriteLine($"Created: {obj.GetType().Name}");

        Product product = (Product)obj;
        Console.WriteLine($"Id: {product.Id}");
        Console.WriteLine($"Name: {product.Name}");
        Console.WriteLine($"Price: {product.Price}");
    }
}