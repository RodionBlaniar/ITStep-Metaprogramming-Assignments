using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

class UserProfileDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(30, MinimumLength = 3)]
    public string Username { get; set; }

    [Required]
    public DateTime? Birthdate { get; set; }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== DataAnnotations Validation ===\n");

        Validate(new UserProfileDto
        {
            Email = "john@example.com",
            Username = "john_doe",
            Birthdate = new DateTime(1990, 5, 15)
        }, "Valid profile");

        Validate(new UserProfileDto
        {
            Email = "invalid-email",
            Username = "jo",
            Birthdate = null
        }, "Invalid profile");

        Validate(new UserProfileDto
        {
            Email = "",
            Username = "",
            Birthdate = null
        }, "Empty profile");
    }

    static void Validate(UserProfileDto dto, string testName)
    {
        Console.WriteLine($"--- {testName} ---");

        var context = new ValidationContext(dto);
        var results = new List<ValidationResult>();

        bool isValid = Validator.TryValidateObject(dto, context, results, true);

        if (isValid)
        {
            Console.WriteLine("  VALID\n");
        }
        else
        {
            Console.WriteLine("  INVALID:");
            foreach (var error in results)
                Console.WriteLine($"    - {error.ErrorMessage}");
            Console.WriteLine();
        }
    }
}