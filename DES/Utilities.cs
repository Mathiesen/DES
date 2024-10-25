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
        int chunckSize = 8;
        var blocks = new List<BitArray>();
        var byteArray = ConvertToByteArray(input);
        
        for (int i = 0; i <= byteArray.Length; i += chunckSize)
        {
            IEnumerable<byte> chunck = byteArray.Skip(i).Take(chunckSize);

            while (chunck.Count() < 8)
            {
                chunck = chunck.Append<byte>(00);
            }
            
            blocks.Add(CreateSingleBlock(chunck));
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
        var bits = new BitArray(keyAsBytes);
        var permutatedKey = new bool[56];

        for (int i = 0; i < Tables.PermutedChoiceOne.Length; i++)
        {
            permutatedKey[i] = bits.Get(Tables.PermutedChoiceOne[i]);
        }

        return new BitArray(permutatedKey);
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
        for (int i = 0; i < blocks.Count(); i++)
        {
            var block = blocks.ElementAt(i);
            var s = Encoding.UTF8.GetString(BitArrayToByteArray(block));
            result.Append(s);
        }

        return result.ToString().TrimEnd('\0');
    }

    public static (BitArray left, BitArray right) Split(BitArray bits)
    {
        var leftBytes = new bool[bits.Length / 2];
        var rightBytes = new bool[bits.Length / 2];
        
        for (var i = 0; i < bits.Length / 2; i++)
        {
            leftBytes[i] = bits[i];
            var idx = 0;
            for (var j = bits.Length / 2; j < bits.Length; j++)
            {
                rightBytes[idx] = bits[j];
                idx++;
            }
        }
        
        var left = new BitArray(leftBytes);
        var right = new BitArray(rightBytes);
        
        return new ValueTuple<BitArray, BitArray>(left, right);
    }
}