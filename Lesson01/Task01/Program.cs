using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter first number: ");
        double a = Convert.ToDouble(Console.ReadLine());

        Console.Write("Enter second number: ");
        double b = Convert.ToDouble(Console.ReadLine());

        Console.Write("Enter operation (+, -, *, /): ");
        string op = Console.ReadLine();

        double result = 0;
        bool valid = true;

        switch (op)
        {
            case "+":
                result = a + b;
                break;
            case "-":
                result = a - b;
                break;
            case "*":
                result = a * b;
                break;
            case "/":
                if (b != 0)
                    result = a / b;
                else
                {
                    Console.WriteLine("Error: Division by zero!");
                    valid = false;
                }
                break;
            default:
                Console.WriteLine("Error: Unknown operation!");
                valid = false;
                break;
        }

        if (valid)
            Console.WriteLine($"Result: {a} {op} {b} = {result}");
    }
}