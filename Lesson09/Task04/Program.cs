using System;
using System.Text.RegularExpressions;

class LoginValidator
{
    public static bool IsValidLogin(string s)
    {
        if (string.IsNullOrEmpty(s))
            return false;

        if (s.Length < 4 || s.Length > 20)
            return false;

        return Regex.IsMatch(s, @"^[a-zA-Z0-9_]+$");
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Login Validation Tests ===\n");
        Console.WriteLine("[Trait(\"category\", \"validation\")]\n");

        TestCase("user_123", true);
        TestCase("ab", false);
        TestCase("valid_user_name", true);
        TestCase("has space", false);

        Console.WriteLine("\n--- Summary ---");
        Console.WriteLine("Rules: length 4-20, only letters/digits/underscore");
    }

    static void TestCase(string input, bool expected)
    {
        bool result = LoginValidator.IsValidLogin(input);
        string status = result == expected ? "PASS" : "FAIL";
        Console.WriteLine($"[{status}] \"{input}\" -> {result} (expected {expected})");
    }
}

/*
xUnit test version:

[Trait("category", "validation")]
public class LoginValidatorTests
{
    [Theory]
    [InlineData("user_123", true)]
    [InlineData("ab", false)]
    [InlineData("valid_user_name", true)]
    [InlineData("has space", false)]
    public void IsValidLogin_ReturnsExpected(string input, bool expected)
    {
        Assert.Equal(expected, LoginValidator.IsValidLogin(input));
    }
}
*/