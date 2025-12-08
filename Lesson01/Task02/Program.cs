using System;

class Program
{
    const double USD_TO_UAH = 41.50;
    const double EUR_TO_UAH = 45.00;
    const double EUR_TO_USD = 1.08;

    static void Main()
    {
        Console.WriteLine("Currency Converter");
        Console.WriteLine("Available currencies: USD, EUR, UAH");

        Console.Write("Enter amount: ");
        double amount = Convert.ToDouble(Console.ReadLine());

        Console.Write("From currency (USD/EUR/UAH): ");
        string from = Console.ReadLine().ToUpper();

        Console.Write("To currency (USD/EUR/UAH): ");
        string to = Console.ReadLine().ToUpper();

        double result = ConvertCurrency(amount, from, to);

        if (result >= 0)
            Console.WriteLine($"{amount} {from} = {result:F2} {to}");
        else
            Console.WriteLine("Error: Invalid currency!");
    }

    static double ConvertCurrency(double amount, string from, string to)
    {
        if (from == to)
            return amount;

        if (from == "USD" && to == "UAH")
            return amount * USD_TO_UAH;
        if (from == "UAH" && to == "USD")
            return amount / USD_TO_UAH;
        if (from == "EUR" && to == "UAH")
            return amount * EUR_TO_UAH;
        if (from == "UAH" && to == "EUR")
            return amount / EUR_TO_UAH;
        if (from == "USD" && to == "EUR")
            return amount / EUR_TO_USD;
        if (from == "EUR" && to == "USD")
            return amount * EUR_TO_USD;

        return -1;
    }
}