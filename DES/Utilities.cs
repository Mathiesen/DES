using System.Collections;
using System.Text;

namespace DES;

public class Utilities
{
    public static byte[] ConvertToByteArray(string input)
    {
        return Encoding.UTF8.GetBytes(input);
    }

    public static IEnumerable<bool[]> CreateBlocks(byte[] byteArray)
    {
        int chunkSize = 8;
        var blocks = new List<bool[]>();
        
        int paddedLength = ((byteArray.Length + chunkSize - 1) / chunkSize) * chunkSize;
        Array.Resize(ref byteArray, paddedLength);

        for (int i = 0; i < byteArray.Length; i += chunkSize)
        {
            var chunk = byteArray.Skip(i).Take(chunkSize).ToArray();
            blocks.Add(ByteArrayToBoolArray(chunk));
        }

        return blocks;
    }

    public static bool[] ConvertBytesToBits(IEnumerable<byte> bytes)
    {
        return ByteArrayToBoolArray(bytes.ToArray());
    }
    
    private static byte[] BitArrayToByteArray(BitArray bits)
    {
        int numBytes = (bits.Length + 7) / 8;
        byte[] bytes = new byte[numBytes];

        for (int i = 0; i < bits.Length; i++)
        {
            int byteIndex = i / 8;
            int bitIndex = 7 - (i % 8); 
            if (bits[i])
            {
                bytes[byteIndex] |= (byte)(1 << bitIndex);
            }
        }

        return bytes;
    }

    public static bool[] ByteArrayToBoolArray(byte[] bytes)
    {
        int totalBits = bytes.Length * 8;
        bool[] boolArray = new bool[totalBits];

        for (int i = 0; i < bytes.Length; i++)
        {
            for (int bit = 0; bit < 8; bit++)
            {
                boolArray[i * 8 + bit] = ((bytes[i] >> bit) & 1) == 1;
            }
        }

        return boolArray;
    }
        

    public static string BitArrayToString(BitArray keyAsBits)
    {
        var bytes = BitArrayToByteArray(keyAsBits);
        return Encoding.UTF8.GetString(bytes);
    }

    public static string ConvertBlocksToString(IEnumerable<BitArray> blocks)
    {
        var byteList = new List<byte>();

        foreach (var block in blocks)
        {
            byteList.AddRange(BitArrayToByteArray(block));
        }
        
        return Encoding.UTF8.GetString(byteList.ToArray()).TrimEnd('\0');
    }
    
    public static bool[] Permutate(string key, byte[] permutationTable)
    {
        var keyAsBytes = ConvertToByteArray(key);
        var keyBits = ByteArrayToBoolArray(keyAsBytes);
        return Permutate(keyBits, permutationTable);
    }

    public static bool[] Permutate(bool[] key, byte[] permutationTable)
    {
        var permutatedKey = new bool[permutationTable.Length];

        for (var i = 0; i < permutationTable.Length; i++)
        {
            permutatedKey[i] = key[permutationTable[i] - 1];
        }
        
        return permutatedKey;
    }



    public static bool[] ReduceToFourBits(byte[,] sbox, bool[] bits)
    {
        int row = (bits[0] ? 2 : 0) + (bits[5] ? 1 : 0);
        int column = 0;
        column |= (bits[1] ? 8 : 0);
        column |= (bits[2] ? 4 : 0);
        column |= (bits[3] ? 2 : 0);
        column |= (bits[4] ? 1 : 0);
        
        byte b = sbox[row, column];
        var outputBits = new bool[4];

        outputBits[0] = (b & 0b1000) != 0;
        outputBits[1] = (b & 0b0100) != 0;
        outputBits[2] = (b & 0b0010) != 0;
        outputBits[3] = (b & 0b0001) != 0;
        
        return outputBits;
    }
    
    public static bool[] StringToBitArray(string input)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(input);
        return ByteArrayToBoolArray(bytes);
    }
}