using System;
using System.IO;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Binary Product Storage ===\n");

        string filePath = "products.dat";

        WriteProducts(filePath);
        ReadProducts(filePath);

        File.Delete(filePath);
    }

    static void WriteProducts(string path)
    {
        Console.WriteLine("Writing products (v1)...\n");

        using (FileStream fs = new FileStream(path, FileMode.Create))
        using (BinaryWriter writer = new BinaryWriter(fs))
        {
            writer.Write(new char[] { 'M', 'A', 'G', 'C' });
            writer.Write((int)1);

            writer.Write((int)1);
            writer.Write((double)19.99);
            writer.Write("Keyboard");

            writer.Write((int)2);
            writer.Write((double)29.99);
            writer.Write("Mouse");

            writer.Write((int)3);
            writer.Write((double)199.99);
            writer.Write("Monitor");
        }

        Console.WriteLine($"Written to: {path}\n");
    }

    static void ReadProducts(string path)
    {
        Console.WriteLine("Reading products...\n");

        using (FileStream fs = new FileStream(path, FileMode.Open))
        using (BinaryReader reader = new BinaryReader(fs))
        {
            char[] magic = reader.ReadChars(4);
            Console.WriteLine($"Magic: {new string(magic)}");

            int version = reader.ReadInt32();
            Console.WriteLine($"Version: {version}\n");

            Console.WriteLine("Products:");
            while (fs.Position < fs.Length)
            {
                int id = reader.ReadInt32();
                double price = reader.ReadDouble();
                string name = reader.ReadString();

                Console.WriteLine($"  #{id} {name} {price:F2}");
            }
        }
    }
}