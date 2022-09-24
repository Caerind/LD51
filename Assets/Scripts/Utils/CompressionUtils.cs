using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;

public static class CompressionUtils
{
    public static byte[] ObjectToByteArray(object obj)
    {
        MemoryStream memoryStream = new MemoryStream();
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(memoryStream, obj);
        return memoryStream.ToArray();
    }

    public static object ByteArrayToObject(byte[] byteArray)
    {
        MemoryStream memoryStream = new MemoryStream(byteArray);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        memoryStream.Position = 0;
        return binaryFormatter.Deserialize(memoryStream);
    }

    public static byte[] Compress(byte[] data)
    {
        MemoryStream uncompressed = new MemoryStream(data);
        MemoryStream compressed = new MemoryStream();
        DeflateStream deflateStream = new DeflateStream(compressed, CompressionMode.Compress);
        uncompressed.CopyTo(deflateStream);
        deflateStream.Close();
        return compressed.ToArray();
    }

    public static byte[] Decompress(byte[] data)
    {
        MemoryStream compressed = new MemoryStream(data);
        MemoryStream decompressed = new MemoryStream();
        DeflateStream deflateStream = new DeflateStream(compressed, CompressionMode.Decompress);
        deflateStream.CopyTo(decompressed);
        byte[] result = decompressed.ToArray();
        return result;
    }

    public static byte[] CompressObject(object obj)
    {
        return Compress(ObjectToByteArray(obj));
    }

    public static object DecompressObject(byte[] data)
    {
        return ByteArrayToObject(Decompress(data));
    }
}