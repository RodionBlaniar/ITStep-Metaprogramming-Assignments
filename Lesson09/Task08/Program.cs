using System;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
struct Header
{
    public uint Magic;
    public ushort Version;
    public ushort Flags;
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Struct Layout Binary Header ===\n");

        Console.WriteLine($"Header size: {Marshal.SizeOf<Header>()} bytes");
        Console.WriteLine("  Magic:   4 bytes (uint)");
        Console.WriteLine("  Version: 2 bytes (ushort)");
        Console.WriteLine("  Flags:   2 bytes (ushort)");
        Console.WriteLine("  Total:   8 bytes (Pack=1, no padding)\n");

        Header[] headers = new Header[]
        {
            new Header { Magic = 0x4D414743, Version = 1, Flags = 0 },
            new Header { Magic = 0x4D414743, Version = 2, Flags = 1 },
            new Header { Magic = 0x4D414743, Version = 3, Flags = 3 }
        };

        byte[] buffer = new byte[headers.Length * Marshal.SizeOf<Header>()];
        Span<byte> span = buffer;

        for (int i = 0; i < headers.Length; i++)
        {
            MemoryMarshal.Write(span.Slice(i * 8), ref headers[i]);
        }

        Console.WriteLine("Written 3 headers to byte[]:");
        Console.WriteLine($"  Bytes: {BitConverter.ToString(buffer)}\n");

        Console.WriteLine("Reading back:");
        for (int i = 0; i < headers.Length; i++)
        {
            var h = MemoryMarshal.Read<Header>(span.Slice(i * 8));
            Console.WriteLine($"  [{i}] Magic=0x{h.Magic:X8}, Version={h.Version}, Flags={h.Flags}");
        }
    }
}