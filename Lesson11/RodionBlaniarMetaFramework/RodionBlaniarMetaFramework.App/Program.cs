using System;
using System.Collections.Generic;
using System.IO;
using RodionBlaniarMetaFramework.Core.Mapping;
using RodionBlaniarMetaFramework.Core.Models;
using RodionBlaniarMetaFramework.Core.Validation;

namespace RodionBlaniarMetaFramework.App
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== Mini Import & Validation Framework ===\n");

            string filePath = "users.csv";

            if (!File.Exists(filePath))
            {
                CreateDemoFile(filePath);
                Console.WriteLine($"Created demo file: {filePath}\n");
            }

            Console.WriteLine("CSV file contents:");
            Console.WriteLine(File.ReadAllText(filePath));
            Console.WriteLine();

            CsvMapper<User> mapper = new CsvMapper<User>();
            List<User> users = mapper.Map(filePath);

            Console.WriteLine($"Imported {users.Count} records\n");

            Validator validator = new Validator();

            int validCount = 0;
            int invalidCount = 0;

            for (int i = 0; i < users.Count; i++)
            {
                User user = users[i];
                List<string> errors = validator.Validate(user);

                Console.WriteLine($"--- Record {i + 1} ---");
                Console.WriteLine($"Username: {user.Username}");
                Console.WriteLine($"Email: {user.Email}");
                Console.WriteLine($"Age: {user.Age}");
                Console.WriteLine($"IsAdult: {user.IsAdult}");

                if (errors.Count == 0)
                {
                    Console.WriteLine("Status: VALID");
                    validCount++;
                }
                else
                {
                    Console.WriteLine("Status: INVALID");
                    foreach (string error in errors)
                    {
                        Console.WriteLine($"  - {error}");
                    }
                    invalidCount++;
                }
                Console.WriteLine();
            }

            Console.WriteLine("=== Summary ===");
            Console.WriteLine($"Valid records: {validCount}");
            Console.WriteLine($"Invalid records: {invalidCount}");
        }

        static void CreateDemoFile(string path)
        {
            string[] lines = new string[]
            {
                "username,email,age",
                "john_doe,john@example.com,25",
                "ab,invalid,15",
                ",missing@email.com,30",
                "valid_user,valid@test.com,200",
                "alice,alice@mail.com,28"
            };
            File.WriteAllLines(path, lines);
        }
    }
}