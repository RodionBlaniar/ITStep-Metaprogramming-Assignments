using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

class UserProfile
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("full_name")]
    public string FullName { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("registered_utc")]
    public DateTimeOffset RegisteredUtc { get; set; }

    [JsonIgnore]
    public bool IsInternal { get; set; }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== User Profiles JSON ===\n");

        string filePath = "profile.json";

        UserProfile profile = new UserProfile
        {
            Id = 1,
            FullName = "John Doe",
            Email = "john@example.com",
            RegisteredUtc = DateTimeOffset.UtcNow,
            IsInternal = true
        };

        Console.WriteLine("--- Save ---");
        Console.WriteLine($"Id: {profile.Id}");
        Console.WriteLine($"FullName: {profile.FullName}");
        Console.WriteLine($"Email: {profile.Email}");
        Console.WriteLine($"RegisteredUtc: {profile.RegisteredUtc:O}");
        Console.WriteLine($"IsInternal: {profile.IsInternal}");

        JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(profile, options);
        File.WriteAllText(filePath, json);

        Console.WriteLine($"\nSaved to {filePath}:");
        Console.WriteLine(json);
        Console.WriteLine("\nNote: IsInternal is not in JSON (JsonIgnore)");

        Console.WriteLine("\n--- Load ---");
        string loadedJson = File.ReadAllText(filePath);
        UserProfile loaded = JsonSerializer.Deserialize<UserProfile>(loadedJson);

        Console.WriteLine($"Id: {loaded.Id}");
        Console.WriteLine($"FullName: {loaded.FullName}");
        Console.WriteLine($"Email: {loaded.Email}");
        Console.WriteLine($"RegisteredUtc (ISO-8601): {loaded.RegisteredUtc:O}");
        Console.WriteLine($"IsInternal: {loaded.IsInternal} (default, not in JSON)");

        File.Delete(filePath);
    }
}