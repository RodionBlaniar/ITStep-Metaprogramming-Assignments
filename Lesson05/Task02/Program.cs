using System;
using System.Collections.Generic;

class Book : IComparable<Book>
{
    public string Title { get; }
    public string Author { get; }
    public int Year { get; }

    public Book(string title, string author, int year)
    {
        Title = title;
        Author = author;
        Year = year;
    }

    public int CompareTo(Book other)
    {
        if (other == null)
            return 1;

        int authorCompare = string.Compare(Author, other.Author, StringComparison.Ordinal);
        if (authorCompare != 0)
            return authorCompare;

        int titleCompare = string.Compare(Title, other.Title, StringComparison.Ordinal);
        if (titleCompare != 0)
            return titleCompare;

        return Year.CompareTo(other.Year);
    }

    public override bool Equals(object obj)
    {
        if (obj is Book other)
        {
            return string.Equals(Title, other.Title, StringComparison.Ordinal) &&
                   string.Equals(Author, other.Author, StringComparison.Ordinal);
        }
        return false;
    }

    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 31 + (Title != null ? Title.GetHashCode() : 0);
        hash = hash * 31 + (Author != null ? Author.GetHashCode() : 0);
        return hash;
    }

    public override string ToString()
    {
        return $"\"{Title}\" by {Author} ({Year})";
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Task 2: Book with IComparable<T> and Equals/GetHashCode ===\n");

        List<Book> books = new List<Book>
        {
            new Book("1984", "George Orwell", 1949),
            new Book("Animal Farm", "George Orwell", 1945),
            new Book("Brave New World", "Aldous Huxley", 1932),
            new Book("Fahrenheit 451", "Ray Bradbury", 1953),
            new Book("The Martian Chronicles", "Ray Bradbury", 1950),
            new Book("Dune", "Frank Herbert", 1965)
        };

        Console.WriteLine("--- Original list ---");
        PrintBooks(books);

        Console.WriteLine("\n--- After Sort() using IComparable<Book> ---");
        Console.WriteLine("Order: Author (Ordinal) -> Title (Ordinal) -> Year\n");
        books.Sort();
        PrintBooks(books);

        Console.WriteLine("\n--- BinarySearch for existing book ---");
        Book searchExisting = new Book("Dune", "Frank Herbert", 1965);
        int indexExisting = books.BinarySearch(searchExisting);
        Console.WriteLine($"Searching for: {searchExisting}");
        Console.WriteLine($"BinarySearch result: {indexExisting}");
        if (indexExisting >= 0)
            Console.WriteLine($"Found at index {indexExisting}: {books[indexExisting]}");

        Console.WriteLine("\n--- BinarySearch for non-existing book ---");
        Book searchMissing = new Book("Foundation", "Isaac Asimov", 1951);
        int indexMissing = books.BinarySearch(searchMissing);
        Console.WriteLine($"Searching for: {searchMissing}");
        Console.WriteLine($"BinarySearch result: {indexMissing}");
        Console.WriteLine($"Negative result means not found. Bitwise complement (~{indexMissing} = {~indexMissing}) is insertion point.");

        Console.WriteLine("\n--- Equals/GetHashCode demonstration ---");
        Book book1 = new Book("1984", "George Orwell", 1949);
        Book book2 = new Book("1984", "George Orwell", 2020);
        Book book3 = new Book("Animal Farm", "George Orwell", 1945);

        Console.WriteLine($"book1: {book1}");
        Console.WriteLine($"book2: {book2}");
        Console.WriteLine($"book3: {book3}");
        Console.WriteLine();
        Console.WriteLine($"book1.Equals(book2) = {book1.Equals(book2)} (same Title+Author, different Year)");
        Console.WriteLine($"book1.Equals(book3) = {book1.Equals(book3)} (different Title)");
        Console.WriteLine();
        Console.WriteLine($"book1.GetHashCode() = {book1.GetHashCode()}");
        Console.WriteLine($"book2.GetHashCode() = {book2.GetHashCode()}");
        Console.WriteLine($"book3.GetHashCode() = {book3.GetHashCode()}");
        Console.WriteLine();
        Console.WriteLine("Note: book1 and book2 have same hash (same Title+Author)");

        Console.WriteLine("\n--- HashSet demonstration ---");
        HashSet<Book> bookSet = new HashSet<Book>();
        bookSet.Add(book1);
        bool added = bookSet.Add(book2);
        Console.WriteLine($"Added book1 to HashSet: True");
        Console.WriteLine($"Added book2 to HashSet: {added} (same Title+Author as book1)");
        Console.WriteLine($"HashSet count: {bookSet.Count}");

        Console.WriteLine("\n=== REPORT ===");
        Console.WriteLine("1. IComparable<Book> interface enables Sort() and BinarySearch() methods");
        Console.WriteLine("2. CompareTo() defines natural ordering: Author -> Title -> Year");
        Console.WriteLine("3. Equals/GetHashCode define logical equality based on Title+Author only");
        Console.WriteLine("4. This means two books with same Title+Author but different Year are equal");
        Console.WriteLine("5. HashSet uses GetHashCode() and Equals() - it sees book1 and book2 as duplicates");
        Console.WriteLine("6. BinarySearch returns negative value (bitwise complement of insertion point) when not found");
    }

    static void PrintBooks(List<Book> books)
    {
        for (int i = 0; i < books.Count; i++)
        {
            Console.WriteLine($"  [{i}] {books[i]}");
        }
    }
}