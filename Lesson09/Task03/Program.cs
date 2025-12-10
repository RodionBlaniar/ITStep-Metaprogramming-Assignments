using System;
using System.ComponentModel.DataAnnotations;

class Room
{
    public int Id { get; set; }

    [Required]
    [MaxLength(10)]
    public string Number { get; set; }

    [Range(1, 10)]
    public int Capacity { get; set; }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== EF Room Modeling ===\n");

        Console.WriteLine("Room entity attributes:");
        Console.WriteLine("  - Id: Primary key (by convention)");
        Console.WriteLine("  - Number: [Required, MaxLength(10)] -> NOT NULL, NVARCHAR(10)");
        Console.WriteLine("  - Capacity: [Range(1, 10)] -> Validated 1-10");
        Console.WriteLine("  - Would have [Index(Number, IsUnique=true)] for unique index");

        Console.WriteLine("\nExpected SQL schema:");
        Console.WriteLine("  CREATE TABLE Rooms (");
        Console.WriteLine("    Id INT PRIMARY KEY IDENTITY,");
        Console.WriteLine("    Number NVARCHAR(10) NOT NULL,");
        Console.WriteLine("    Capacity INT NOT NULL");
        Console.WriteLine("  );");
        Console.WriteLine("  CREATE UNIQUE INDEX IX_Rooms_Number ON Rooms(Number);");

        var room = new Room { Id = 1, Number = "A-101", Capacity = 4 };
        Console.WriteLine($"\nSample: Id={room.Id}, Number={room.Number}, Capacity={room.Capacity}");
    }
}