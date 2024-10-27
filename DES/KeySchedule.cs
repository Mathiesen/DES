using System.Collections;

namespace DES;

public class KeySchedule
{
    private Dictionary<int, BitArray> keys = new Dictionary<int, BitArray>();
    
    public BitArray PermutateKey(string key, byte[] permutationTable)
    {
        var keyAsBytes = Utilities.ConvertToByteArray(key);
        var keyBits = new BitArray(keyAsBytes);
        var permutatedKey = new BitArray(56);

        for (var i = 0; i < permutationTable.Length; i++)
        {
            permutatedKey[i] = keyBits[Tables.PermutedChoiceOne[i]];
        }

        return permutatedKey;
    }
    
    public (BitArray left, BitArray right) Split(BitArray bits)
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

    public void RunKeySchedule(string key)
    {
        var permutatedKey = PermutateKey(key, Tables.PermutedChoiceOne);
        var splitedKey = Split(permutatedKey);
        Transform(splitedKey.left, splitedKey.right);
    }

    public void Transform(BitArray left, BitArray right)
    {
        var leftShifted = left;
        var rightShifted = right;
        for (int i = 0; i < Tables.Shifts.Length; i++)
        {
            leftShifted = Utilities.LeftShift(leftShifted, Tables.Shifts[i]);
            rightShifted = Utilities.LeftShift(rightShifted, Tables.Shifts[i]);
            
            keys.Add(i, Utilities.Concatenate(leftShifted, rightShifted));
        }
    }
}