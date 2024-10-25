using System.Collections;
using System.Text;

namespace DES;

public class Utilities
{
    public static byte[] ConvertToByteArray(string input)
    {
        return Encoding.UTF8.GetBytes(input);
    }

    public static IEnumerable<BitArray> CreateBlocks(string input)
    {
        int chunkSize = 8;
        var blocks = new List<BitArray>();
        var byteArray = ConvertToByteArray(input);
    
        for (int i = 0; i < byteArray.Length; i += chunkSize)
        {
            var chunk = byteArray.Skip(i).Take(chunkSize).ToList();

            if (chunk.Count < chunkSize)
            {
                chunk.AddRange(new byte[chunkSize - chunk.Count]);
            }
        
            blocks.Add(CreateSingleBlock(chunk));
        }

        return blocks;
    }

    private static BitArray CreateSingleBlock(IEnumerable<byte> input)
    {
        return new BitArray(input.ToArray());
    }

    public static List<BitArray> ConvertBytesToBits(IEnumerable<IEnumerable<byte>> blocks)
    {
        var bits = new List<BitArray>();
        blocks.ToList().ForEach(x => bits.Add(new BitArray(x.ToArray())));
        return bits;
    }

    public static BitArray PermutateKey(string key)
    {
        var keyAsBytes = ConvertToByteArray(key);
        var keyBits = new BitArray(keyAsBytes);
        var permutatedKey = new BitArray(56);

        for (var i = 0; i < Tables.PermutedChoiceOne.Length; i++)
        {
            permutatedKey[i] = keyBits[Tables.PermutedChoiceOne[i]];
        }

        return permutatedKey;
    }
    
    private static byte[] BitArrayToByteArray(BitArray bitArray)
    {
        int numBytes = (bitArray.Length + 7) / 8; 
        byte[] byteArray = new byte[numBytes];

        for (int i = 0; i < bitArray.Length; i++)
        {
            if (bitArray[i])
            {
                byteArray[i / 8] |= (byte)(1 << (i % 8)); 
            }
        }

        return byteArray;
    }

    public static string BitArrayToString(BitArray keyAsBits)
    {
        var bytes = BitArrayToByteArray(keyAsBits);
        return Encoding.UTF8.GetString(bytes);
    }

    public static string ConvertBlocksToString(IEnumerable<BitArray> blocks)
    {
        StringBuilder result = new StringBuilder();

        foreach (var block in blocks)
        {
            string s = Encoding.UTF8.GetString(BitArrayToByteArray(block));
            result.Append(s);
        }

        return result.ToString().TrimEnd('\0');
    }

    public static (BitArray left, BitArray right) Split(BitArray bits)
    {
        int halfLength = bits.Length / 2;
    
        var leftBytes = new bool[halfLength];
        var rightBytes = new bool[halfLength];

        for (var i = 0; i < halfLength; i++)
        {
            leftBytes[i] = bits[i];
        }

        for (var i = 0; i < halfLength; i++)
        {
            rightBytes[i] = bits[i + halfLength];
        }

        var left = new BitArray(leftBytes);
        var right = new BitArray(rightBytes);

        return (left, right);
    }
}