using System.Collections;

namespace DES;

public class KeySchedule
{
    public Dictionary<int, BitArray> Keys = new Dictionary<int, BitArray>();
    
    
    


    public void Run(string key)
    {
        var permutatedKey = Utilities.Permutate(key, Tables.PermutedChoiceOne);
        var splitedKey = Utilities.Split(permutatedKey);
        var leftShifted = splitedKey.left;
        var rightShifted = splitedKey.right;
        
        for (var i = 0; i < Tables.Shifts.Length; i++)
        {
            leftShifted = Utilities.LeftShift(leftShifted, Tables.Shifts[i]);
            rightShifted = Utilities.LeftShift(rightShifted, Tables.Shifts[i]);
            
            var transformed = Transform(leftShifted, rightShifted);
            Keys.Add(i, transformed);
        }
    }

    private BitArray Transform(BitArray left, BitArray right)
    {
        var concatenatedKey = Utilities.Concatenate(left, right);
        var permutatedKey = Utilities.Permutate(concatenatedKey, Tables.PermutedChoiceTwo);

        return permutatedKey;
    }
}