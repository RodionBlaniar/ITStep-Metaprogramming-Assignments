using System;
using System.Collections.Generic;

public class TimeInterval
{
    private int _startMinutes;
    private int _endMinutes;

    public int StartMinutes => _startMinutes;
    public int EndMinutes => _endMinutes;

    public TimeInterval(int startMinutes, int endMinutes)
    {
        if (startMinutes < 0 || startMinutes >= 1440)
            throw new ArgumentException("Start must be between 0 and 1439");
        if (endMinutes < 0 || endMinutes >= 1440)
            throw new ArgumentException("End must be between 0 and 1439");
        if (startMinutes > endMinutes)
            throw new ArgumentException("Start cannot be after end");

        _startMinutes = startMinutes;
        _endMinutes = endMinutes;
    }

    public TimeInterval(string interval)
    {
        if (string.IsNullOrWhiteSpace(interval))
            throw new ArgumentException("Interval string cannot be empty");

        string[] parts = interval.Split('-');
        if (parts.Length != 2)
            throw new ArgumentException("Invalid format. Use HH:MM-HH:MM");

        _startMinutes = ParseTime(parts[0].Trim());
        _endMinutes = ParseTime(parts[1].Trim());

        if (_startMinutes > _endMinutes)
            throw new ArgumentException("Start cannot be after end");
    }

    private static int ParseTime(string time)
    {
        string[] parts = time.Split(':');
        if (parts.Length != 2)
            throw new ArgumentException("Invalid time format. Use HH:MM");

        int hours = int.Parse(parts[0]);
        int minutes = int.Parse(parts[1]);

        if (hours < 0 || hours > 23 || minutes < 0 || minutes > 59)
            throw new ArgumentException("Invalid time values");

        return hours * 60 + minutes;
    }

    private static string FormatTime(int minutes)
    {
        int hours = minutes / 60;
        int mins = minutes % 60;
        return $"{hours:D2}:{mins:D2}";
    }

    public override string ToString()
    {
        return $"[{FormatTime(_startMinutes)}-{FormatTime(_endMinutes)}]";
    }

    public override bool Equals(object obj)
    {
        if (obj is TimeInterval other)
            return _startMinutes == other._startMinutes && _endMinutes == other._endMinutes;
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_startMinutes, _endMinutes);
    }

    public int Length()
    {
        return _endMinutes - _startMinutes;
    }

    public bool Overlaps(TimeInterval other)
    {
        return _startMinutes <= other._endMinutes && _endMinutes >= other._startMinutes;
    }

    public bool Overlaps(int minute)
    {
        return minute >= _startMinutes && minute <= _endMinutes;
    }

    public int this[int i]
    {
        get
        {
            if (i == 0) return _startMinutes;
            if (i == 1) return _endMinutes;
            throw new IndexOutOfRangeException("Index must be 0 (start) or 1 (end)");
        }
    }

    public int this[string name]
    {
        get
        {
            if (name.Equals("start", StringComparison.OrdinalIgnoreCase))
                return _startMinutes;
            if (name.Equals("end", StringComparison.OrdinalIgnoreCase))
                return _endMinutes;
            throw new ArgumentException("Name must be 'start' or 'end'");
        }
    }

    public static bool operator ==(TimeInterval a, TimeInterval b)
    {
        if (ReferenceEquals(a, b)) return true;
        if (a is null || b is null) return false;
        return a.Equals(b);
    }

    public static bool operator !=(TimeInterval a, TimeInterval b)
    {
        return !(a == b);
    }

    public static TimeInterval operator +(TimeInterval a, TimeInterval b)
    {
        int newStart = Math.Min(a._startMinutes, b._startMinutes);
        int newEnd = Math.Max(a._endMinutes, b._endMinutes);
        return new TimeInterval(newStart, newEnd);
    }

    public static TimeInterval operator *(TimeInterval a, TimeInterval b)
    {
        int newStart = Math.Max(a._startMinutes, b._startMinutes);
        int newEnd = Math.Min(a._endMinutes, b._endMinutes);

        if (newStart > newEnd)
            throw new InvalidOperationException("Intervals do not overlap");

        return new TimeInterval(newStart, newEnd);
    }

    public static explicit operator int(TimeInterval interval)
    {
        return interval.Length();
    }

    public static TimeInterval FromString(string interval)
    {
        return new TimeInterval(interval);
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== TimeInterval Demo ===\n");

        Console.WriteLine("--- Creating Intervals ---");
        var interval1 = new TimeInterval(9 * 60 + 30, 11 * 60 + 15);
        var interval2 = new TimeInterval("10:00-12:00");
        var interval3 = new TimeInterval("14:00-16:00");

        Console.WriteLine($"Interval 1: {interval1}");
        Console.WriteLine($"Interval 2: {interval2}");
        Console.WriteLine($"Interval 3: {interval3}");

        Console.WriteLine("\n--- ToString() ---");
        Console.WriteLine($"interval1.ToString() = {interval1}");

        Console.WriteLine("\n--- Length() ---");
        Console.WriteLine($"interval1.Length() = {interval1.Length()} minutes");
        Console.WriteLine($"interval2.Length() = {interval2.Length()} minutes");

        Console.WriteLine("\n--- Overlaps() ---");
        Console.WriteLine($"interval1.Overlaps(interval2) = {interval1.Overlaps(interval2)}");
        Console.WriteLine($"interval1.Overlaps(interval3) = {interval1.Overlaps(interval3)}");
        Console.WriteLine($"interval1.Overlaps(600) [10:00] = {interval1.Overlaps(600)}");
        Console.WriteLine($"interval1.Overlaps(480) [08:00] = {interval1.Overlaps(480)}");

        Console.WriteLine("\n--- Indexers ---");
        Console.WriteLine($"interval1[0] = {interval1[0]} minutes ({FormatMinutes(interval1[0])})");
        Console.WriteLine($"interval1[1] = {interval1[1]} minutes ({FormatMinutes(interval1[1])})");
        Console.WriteLine($"interval1[\"start\"] = {interval1["start"]}");
        Console.WriteLine($"interval1[\"END\"] = {interval1["END"]}");

        Console.WriteLine("\n--- Equals and == ---");
        var intervalCopy = new TimeInterval("09:30-11:15");
        Console.WriteLine($"interval1 == intervalCopy: {interval1 == intervalCopy}");
        Console.WriteLine($"interval1.Equals(intervalCopy): {interval1.Equals(intervalCopy)}");
        Console.WriteLine($"interval1 == interval2: {interval1 == interval2}");
        Console.WriteLine($"interval1 != interval2: {interval1 != interval2}");

        Console.WriteLine("\n--- HashSet Test ---");
        var set = new HashSet<TimeInterval>();
        set.Add(interval1);
        set.Add(intervalCopy);
        set.Add(interval2);
        Console.WriteLine($"Added interval1, intervalCopy, interval2 to HashSet");
        Console.WriteLine($"HashSet count: {set.Count} (should be 2, since interval1 == intervalCopy)");

        Console.WriteLine("\n--- Operator + (Union) ---");
        var union = interval1 + interval2;
        Console.WriteLine($"{interval1} + {interval2} = {union}");

        Console.WriteLine("\n--- Operator * (Intersection) ---");
        var intersection = interval1 * interval2;
        Console.WriteLine($"{interval1} * {interval2} = {intersection}");

        Console.WriteLine("\n--- Operator * (No Overlap - Exception) ---");
        try
        {
            var noOverlap = interval1 * interval3;
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"interval1 * interval3 threw: {ex.Message}");
        }

        Console.WriteLine("\n--- Explicit Conversion to int ---");
        int duration = (int)interval1;
        Console.WriteLine($"(int)interval1 = {duration} minutes");
        Console.WriteLine("Note: This is explicit because we lose the boundaries, keeping only duration.");

        Console.WriteLine("\n--- Why No implicit from string? ---");
        Console.WriteLine("We use constructor or FromString() instead of implicit conversion because:");
        Console.WriteLine("1. Parsing can fail with invalid format");
        Console.WriteLine("2. Implicit should be safe and lossless");
        Console.WriteLine("3. Constructor makes intent clear");
        var fromFactory = TimeInterval.FromString("08:00-09:00");
        Console.WriteLine($"TimeInterval.FromString(\"08:00-09:00\") = {fromFactory}");

        Console.WriteLine("\n--- Validation Tests ---");

        Console.Write("Invalid index [2]: ");
        try { var x = interval1[2]; }
        catch (IndexOutOfRangeException ex) { Console.WriteLine(ex.Message); }

        Console.Write("Invalid name [\"middle\"]: ");
        try { var x = interval1["middle"]; }
        catch (ArgumentException ex) { Console.WriteLine(ex.Message); }

        Console.Write("Invalid format: ");
        try { var x = new TimeInterval("invalid"); }
        catch (ArgumentException ex) { Console.WriteLine(ex.Message); }

        Console.Write("Start > End: ");
        try { var x = new TimeInterval("12:00-10:00"); }
        catch (ArgumentException ex) { Console.WriteLine(ex.Message); }
    }

    static string FormatMinutes(int minutes)
    {
        return $"{minutes / 60:D2}:{minutes % 60:D2}";
    }
}