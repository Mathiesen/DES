using System.Collections;

namespace DES;

public class KeySchedule
{
    public List<bool[]> Keys = new List<bool[]>();

    public void Run(string key)
    {
        Keys.Clear();
        var keyBits = Utilities.StringToBitArray(key);
        var permutatedKey = Utilities.Permutate(keyBits, Tables.PermutedChoiceOne);

        var splitedKey = permutatedKey.Split();
        var leftShifted = splitedKey.left;
        var rightShifted = splitedKey.right;
        
        for (var i = 0; i < Tables.Shifts.Length; i++)
        {
            leftShifted = leftShifted.LeftShift(Tables.Shifts[i]);
            rightShifted = rightShifted.LeftShift(Tables.Shifts[i]);
            
            var transformed = Transform(leftShifted, rightShifted);
            Keys.Add(transformed);
        }
    }

    private bool[] Transform(bool[] left, bool[] right)
    {
        var concatenatedKey = left.Concatenate(right);
        var permutatedKey = Utilities.Permutate(concatenatedKey, Tables.PermutedChoiceTwo);

        return permutatedKey;
    }
}