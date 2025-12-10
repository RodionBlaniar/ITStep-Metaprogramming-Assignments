using System;
using System.Reflection;

class MathOperations
{
    public int Add(int a, int b) { return a + b; }
    public int Sub(int a, int b) { return a - b; }
    public int Mul(int a, int b) { return a * b; }
    public int Div(int a, int b) { return a / b; }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Dynamic Method Invocation ===\n");

        MathOperations math = new MathOperations();
        Type type = typeof(MathOperations);

        MethodInfo addMethod = type.GetMethod("Add");

        object[] parameters = { 5, 3 };
        object result = addMethod.Invoke(math, parameters);

        Console.WriteLine($"Calling Add(5, 3) via reflection");
        Console.WriteLine($"Result: {result}");
    }
}