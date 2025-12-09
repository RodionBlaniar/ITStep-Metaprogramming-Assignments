using System;
using System.Collections.Generic;
using System.Linq;

record Student(int Id, string Name, string Group, int Avg, bool IsActive, string Email);

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Student Analytics ===\n");

        List<Student> students = new List<Student>
        {
            new Student(1, "John Smith", "CS-101", 85, true, "john@mail.com"),
            new Student(2, "Anna Brown", "CS-101", 92, true, "anna@mail.com"),
            new Student(3, "Mike Wilson", "CS-102", 78, true, "mike@mail.com"),
            new Student(4, "Sara Davis", "CS-102", 88, true, "sara@mail.com"),
            new Student(5, "Tom Harris", "CS-101", 65, false, "tom@mail.com"),
            new Student(6, "Lisa Clark", "CS-103", 95, true, "lisa@mail.com"),
            new Student(7, "James Lee", "CS-103", 81, true, "james@mail.com"),
            new Student(8, "Emma White", "CS-102", 90, false, "emma@mail.com"),
            new Student(9, "David King", "CS-101", 83, true, "david@mail.com"),
            new Student(10, "Olivia Scott", "CS-103", 77, true, "olivia@mail.com")
        };

        var result = students
            .Where(s => s.IsActive && s.Avg >= 80)
            .OrderByDescending(s => s.Avg)
            .ThenBy(s => s.Name)
            .Select(s => new { s.Name, s.Avg })
            .ToList();

        Console.WriteLine("Active students with Avg >= 80:");
        Console.WriteLine("(Sorted by Avg desc, then Name asc)\n");

        foreach (var s in result)
        {
            Console.WriteLine($"  {s.Name}: {s.Avg}");
        }
    }
}