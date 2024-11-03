namespace DES;

public static class BoolArrayExtensions
{
    public static bool[] Xor(this bool[] left, bool[] right)
    {
        if (left.Length != right.Length)
        {
            throw new ArgumentException("Arrays must be of the same length.");
        }
        bool[] resultArray = new bool[left.Length];
        
        for (int i = 0; i < left.Length; i++)
        {
            resultArray[i] = left[i] ^ right[i];
        }
        
        return resultArray;
    }
    
    public static bool[] LeftShift(this bool[] bits, int numberOfShifts)
    {
        int length = bits.Length;
        bool[] result = new bool[length];

        for (int i = 0; i < length; i++)
        {
            result[i] = bits[(i + numberOfShifts) % length];
        }

        return result;
    }
    
    public static bool[] Concatenate(this bool[] leftBits, bool[] rightBits)
    {
        var bools = new bool[leftBits.Length + rightBits.Length];
        leftBits.CopyTo(bools, 0);
        rightBits.CopyTo(bools, leftBits.Length);

        return bools;
    }
    
    public static byte[] ConvertToByteArray(this bool[] input)
    {
        int byteLength = (input.Length + 7) / 8; 
        byte[] bytes = new byte[byteLength];

        for (int i = 0; i < input.Length; i++)
        {
            if (input[i])
            {
                bytes[i / 8] |= (byte)(1 << (i % 8));
            }
        }

        return bytes;
    }
    
    public static bool[] Expand(this bool[] bits)
    {
        var newArray = new bool[Tables.Expansion.Length];

        for (int i = 0; i < Tables.Expansion.Length; i++)
        {
            newArray[i] = bits[Tables.Expansion[i] - 1];
        }
        
        return newArray;
    }
    
    public static IEnumerable<bool[]> Split(this bool[] bits, int numberOfBits)
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
    
    public static (bool[] left, bool[] right) Split(this bool[] bits)
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

        var left = leftBytes;
        var right = rightBytes;

        return (left, right);
    }
}