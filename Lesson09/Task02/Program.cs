using System;
using System.Text.Json;
using System.Text.Json.Serialization;

class ProductDto
{
    [JsonPropertyName("product_id")]
    public int ProductId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("internal_notes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string InternalNotes { get; set; }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== JSON Contract ===\n");

        var options = new JsonSerializerOptions { WriteIndented = true };

        var product1 = new ProductDto
        {
            ProductId = 1,
            Name = "Laptop",
            Price = 999.99m,
            InternalNotes = "Check stock"
        };

        var product2 = new ProductDto
        {
            ProductId = 2,
            Name = "Mouse",
            Price = 29.99m,
            InternalNotes = null
        };

        Console.WriteLine("--- Serialize (with notes) ---");
        string json1 = JsonSerializer.Serialize(product1, options);
        Console.WriteLine(json1);

        Console.WriteLine("\n--- Serialize (null notes - ignored) ---");
        string json2 = JsonSerializer.Serialize(product2, options);
        Console.WriteLine(json2);

        Console.WriteLine("\n--- Deserialize ---");
        string inputJson = "{\"product_id\":3,\"name\":\"Keyboard\",\"price\":49.99}";
        Console.WriteLine($"Input: {inputJson}");
        var product3 = JsonSerializer.Deserialize<ProductDto>(inputJson);
        Console.WriteLine($"Parsed: Id={product3.ProductId}, Name={product3.Name}, Price={product3.Price}");
    }
}