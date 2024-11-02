using System.Collections;
using System.Text;

namespace DES;

public class Utilities
{
    public static byte[] ConvertToByteArray(string input)
    {
        return Encoding.UTF8.GetBytes(input);
    }

    public static byte[] ConvertToByteArray(BitArray input)
    {
        byte[] bytes = new byte[input.Length];
        input.CopyTo(bytes, 0);
        return bytes;
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

    public static BitArray ConvertBytesToBits(IEnumerable<byte> bytes)
    {
        return new BitArray(bytes.ToArray());
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
    
    public static BitArray Permutate(string key, byte[] permutationTable)
    {
        var keyAsBytes = ConvertToByteArray(key);
        var keyBits = new BitArray(keyAsBytes);
        return Permutate(keyBits, permutationTable);
    }

    public static BitArray Permutate(BitArray key, byte[] permutationTable)
    {
        var permutatedKey = new BitArray(permutationTable.Length);

        for (var i = 0; i < permutationTable.Length; i++)
        {
            permutatedKey[i] = key[permutationTable[i]];
        }
        
        return permutatedKey;
    }

    public static IEnumerable<bool[]> Split(BitArray bits, int numberOfBits)
    {
        var result = new List<bool[]>();
        var bitsAsArray = new bool[bits.Length];
        bits.CopyTo(bitsAsArray, 0);

        for (int i = 0; i < bits.Length; i += numberOfBits)
        {
            var block = bitsAsArray.ToList().Skip(i).Take(numberOfBits);
            result.Add(block.ToArray());
        }

        return result;
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

    public static IEnumerable<BitArray> Partition(BitArray splitRight)
    {
        return null;
    }

    public static BitArray Expand(BitArray bits)
    {
        var newArray = new BitArray(Tables.Expansion.Length);

        for (int i = 0; i < Tables.Expansion.Length; i++)
        {
            newArray[i] = bits[Tables.Expansion[i]];
        }
        
        return newArray;
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

        outputBits[0] = (b & 0b0001) != 0;
        outputBits[1] = (b & 0b0010) != 0;
        outputBits[2] = (b & 0b0100) != 0;
        outputBits[3] = (b & 0b1000) != 0;
        
        return outputBits;
    }

    public static BitArray ConcatBitArray(BitArray first, BitArray second)
    {
        var concatenated = new BitArray(first.Length + second.Length);

        for (var i = 0; i < first.Length; i++)
        {
            concatenated[i] = first[i];
        }

        for (var j = 0; j < second.Length; j++)
        {
            concatenated[first.Length + j] = second[j];
        }

        return concatenated;
    }
}