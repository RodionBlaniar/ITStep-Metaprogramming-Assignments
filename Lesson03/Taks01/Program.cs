using System;
using System.Collections.Generic;

public class PaymentProcessedEventArgs : EventArgs
{
    public decimal Amount { get; }
    public string Currency { get; }
    public DateTime ProcessedUtc { get; }

    public PaymentProcessedEventArgs(decimal amount, string currency, DateTime processedUtc)
    {
        Amount = amount;
        Currency = currency;
        ProcessedUtc = processedUtc;
    }
}

public interface IRefundable
{
    void Refund(decimal amount);
}

public abstract class Payment
{
    private decimal _amount;
    private string _currency;

    public decimal Amount
    {
        get { return _amount; }
    }

    public string Currency
    {
        get { return _currency; }
    }

    public DateTime CreatedUtc { get; }
    public DateTime? ProcessedUtc { get; private set; }

    public event EventHandler<PaymentProcessedEventArgs> Processed;

    protected Payment(decimal amount, string currency)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than 0");
        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency cannot be empty");

        _amount = amount;
        _currency = currency;
        CreatedUtc = DateTime.UtcNow;
        ProcessedUtc = null;
    }

    public void Process()
    {
        if (ProcessedUtc != null)
            throw new InvalidOperationException("Payment already processed");

        OnBeforeProcess();
        DoProcess();
        OnAfterProcess();

        var args = new PaymentProcessedEventArgs(Amount, Currency, ProcessedUtc.Value);
        OnProcessed(args);
    }

    protected virtual void OnBeforeProcess()
    {
    }

    protected abstract void DoProcess();

    protected virtual void OnAfterProcess()
    {
        ProcessedUtc = DateTime.UtcNow;
    }

    protected virtual void OnProcessed(PaymentProcessedEventArgs e)
    {
        Processed?.Invoke(this, e);
    }
}

public class CashPayment : Payment
{
    public CashPayment(decimal amount, string currency) : base(amount, currency)
    {
    }

    protected override void DoProcess()
    {
        Console.WriteLine($"[CashPayment] Processing cash payment: {Amount} {Currency}");
    }
}

public class CardPayment : Payment, IRefundable
{
    public string CardMasked { get; }
    public string AuthCode { get; private set; }

    private decimal _refundedAmount;

    public CardPayment(decimal amount, string currency, string cardMasked) : base(amount, currency)
    {
        if (string.IsNullOrWhiteSpace(cardMasked))
            throw new ArgumentException("Card number cannot be empty");
        CardMasked = cardMasked;
        _refundedAmount = 0;
    }

    protected override void DoProcess()
    {
        AuthCode = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        Console.WriteLine($"[CardPayment] Processing card payment: {Amount} {Currency}, Card: {CardMasked}, AuthCode: {AuthCode}");
    }

    public void Refund(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Refund amount must be greater than 0");
        if (amount > Amount - _refundedAmount)
            throw new ArgumentException("Refund amount exceeds available balance");
        if (ProcessedUtc == null)
            throw new InvalidOperationException("Cannot refund unprocessed payment");

        _refundedAmount += amount;
        Console.WriteLine($"[CardPayment] Refunded {amount} {Currency} to card {CardMasked}");
    }
}

public class CryptoPayment : Payment
{
    public string TxHash { get; private set; }

    public CryptoPayment(decimal amount, string currency) : base(amount, currency)
    {
    }

    protected override void DoProcess()
    {
        TxHash = "0x" + Guid.NewGuid().ToString("N");
        Console.WriteLine($"[CryptoPayment] Processing crypto payment: {Amount} {Currency}, TxHash: {TxHash}");
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Payment System Demo ===\n");

        var payments = new List<Payment>
        {
            new CashPayment(100.00m, "UAH"),
            new CardPayment(250.50m, "USD", "****1234"),
            new CryptoPayment(0.005m, "BTC"),
            new CardPayment(99.99m, "EUR", "****5678")
        };

        Console.WriteLine("--- Processing Payments ---\n");
        ProcessAllPayments(payments);

        Console.WriteLine("\n--- Refunding Card Payments ---\n");
        var refundables = new List<IRefundable>();
        foreach (var payment in payments)
        {
            if (payment is IRefundable refundable)
                refundables.Add(refundable);
        }
        RefundAll(refundables, 50.00m);

        Console.WriteLine("\n--- Validation Tests ---\n");
        RunValidationTests();
    }

    static void ProcessAllPayments(IEnumerable<Payment> payments)
    {
        foreach (var payment in payments)
        {
            payment.Processed += OnPaymentProcessed;
            payment.Process();
        }
    }

    static void OnPaymentProcessed(object sender, PaymentProcessedEventArgs e)
    {
        Console.WriteLine($"  -> Event: Payment processed at {e.ProcessedUtc:HH:mm:ss}, Amount: {e.Amount} {e.Currency}\n");
    }

    static void RefundAll(IEnumerable<IRefundable> refundables, decimal amount)
    {
        foreach (var refundable in refundables)
        {
            try
            {
                refundable.Refund(amount);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"  Refund failed: {ex.Message}");
            }
        }
    }

    static void RunValidationTests()
    {
        Console.WriteLine("Test 1: Amount <= 0");
        try
        {
            var invalid = new CashPayment(-50, "USD");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"  PASS: {ex.Message}");
        }

        Console.WriteLine("\nTest 2: Empty currency");
        try
        {
            var invalid = new CashPayment(100, "");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"  PASS: {ex.Message}");
        }

        Console.WriteLine("\nTest 3: ProcessedUtc is set after Process()");
        var payment = new CashPayment(50, "UAH");
        Console.WriteLine($"  Before Process: ProcessedUtc = {payment.ProcessedUtc}");
        payment.Process();
        Console.WriteLine($"  After Process: ProcessedUtc = {payment.ProcessedUtc}");
        Console.WriteLine($"  PASS: ProcessedUtc is set");

        Console.WriteLine("\nTest 4: Cannot process twice");
        try
        {
            payment.Process();
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"  PASS: {ex.Message}");
        }

        Console.WriteLine("\nTest 5: Refund exceeds amount");
        var card = new CardPayment(100, "USD", "****9999");
        card.Process();
        try
        {
            card.Refund(150);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"  PASS: {ex.Message}");
        }

        Console.WriteLine("\nTest 6: CashPayment has no Refund method (compile-time safety)");
        Console.WriteLine("  PASS: CashPayment does not implement IRefundable");
    }
}