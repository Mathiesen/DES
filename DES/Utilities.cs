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

    public static BitArray LeftShift(BitArray bits, int numberOfShifts)
    {
        int length = bits.Length;
        BitArray result = new BitArray(length);

        for (int i = 0; i < length; i++)
        {
            int newIndex = (i - numberOfShifts + length) % length;
            result[newIndex] = bits[i];
        }

        return result;
    }

    public static BitArray Concatenate(BitArray leftBits, BitArray rightBits)
    {
        var bools = new bool[leftBits.Length + rightBits.Length];
        leftBits.CopyTo(bools, 0);
        rightBits.CopyTo(bools, leftBits.Length);
        
        return new BitArray(bools);
    }
}