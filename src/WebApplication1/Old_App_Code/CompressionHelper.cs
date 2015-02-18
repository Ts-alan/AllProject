using System;
using System.Collections.Generic;
using System.Web;

using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System.IO;

public static class CompressionHelper
{
    public static byte[] Compress(byte[] bytes)
    {
        MemoryStream memory = new MemoryStream();
        Stream stream = new DeflaterOutputStream(memory,
            new ICSharpCode.SharpZipLib.Zip.Compression.Deflater(
            ICSharpCode.SharpZipLib.Zip.Compression.Deflater.BEST_COMPRESSION), 131072);
        stream.Write(bytes, 0, bytes.Length);
        stream.Close();
        return memory.ToArray();
    }

    public static byte[] Decompress(byte[] bytes)
    {
        Stream stream = new InflaterInputStream(new MemoryStream(bytes));
        MemoryStream memory = new MemoryStream();
        int totalLength = 0;
        byte[] writeData = new byte[4096];
        while (true)
        {
            int size = stream.Read(writeData, 0, writeData.Length);
            if (size > 0)
            {
                totalLength += size;
                memory.Write(writeData, 0, size);
            }
            else
                break;
        }
        stream.Close();
        return memory.ToArray();
    }
}