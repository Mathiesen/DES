using System.Collections;
using System.Text;

namespace DES;

public class DES
{
    private KeySchedule keySchedule;
    
    public DES()
    {
        keySchedule = new KeySchedule();
    }
    
    public string Encrypt(string text, string key)
    {
        keySchedule.Run(key);
        var b = Encoding.UTF8.GetBytes(text);
        var blocks = Utilities.CreateBlocks(b);
        List<bool[]> finalBlocks = new List<bool[]>();
        
        foreach (var block in blocks)
        { 
            var permutatedBlock = Utilities.Permutate(block, Tables.InitialPermutation);
            var (leftBits, rightBits) = permutatedBlock.Split();
            
            (leftBits, rightBits) = RunRounds(leftBits, rightBits, 0, keySchedule.Keys);
            var concatenated = rightBits.Concatenate(leftBits);
            var finalPermutation = Utilities.Permutate(concatenated, Tables.FinalPermutation);
            finalBlocks.Add(finalPermutation);
        }

        return ConvertToBase64(finalBlocks);
    }

    public string Decrypt(string text, string key)
    {
        keySchedule.Keys.Clear();
        keySchedule.Run(key);
        
        var keys = keySchedule.Keys;
        keys.Reverse();
        var b = Convert.FromBase64String(text);
        var blocks = Utilities.CreateBlocks(b);
        List<bool[]> finalBlocks = new List<bool[]>();
        
        foreach (var block in blocks)
        {
            var permutatedBlock = Utilities.Permutate(block, Tables.InitialPermutation);
            var (leftBits, rightBits) = permutatedBlock.Split();
            
            (leftBits, rightBits) = RunRounds(leftBits, rightBits, 0, keys);

            var concatenated = rightBits.Concatenate(leftBits);
            var finalPermutation = Utilities.Permutate(concatenated, Tables.FinalPermutation);
            finalBlocks.Add(finalPermutation);
        }

        return ConvertToText(finalBlocks);
    }

    private (bool[] left, bool[] right) RunRounds(bool[] leftBits, bool[] rightBits, int index, List<bool[]> keys)
    {
        for (int i = 0; i < 16; i++)
        {
            var rightExpanded = FeistelFunction(rightBits, keys[i]);
            var tempLeft = leftBits.Xor(rightExpanded);
            
            leftBits = rightBits;
            rightBits = tempLeft;
        }

        return (leftBits, rightBits);
    }

    private bool[] FeistelFunction(bool[] rightBits, bool[] keyScheduleKey)
    {
        var expanded = rightBits.Expand();
        var xor = expanded.Xor(keyScheduleKey);

        var splitBits = xor.Split(6).ToArray();

        var concatenated = Utilities.ReduceToFourBits(Tables.SBoxOne, splitBits[0])
            .Concat(Utilities.ReduceToFourBits(Tables.SBoxTwo, splitBits[1]))
            .Concat(Utilities.ReduceToFourBits(Tables.SBoxThree, splitBits[2]))
            .Concat(Utilities.ReduceToFourBits(Tables.SBoxFour, splitBits[3]))
            .Concat(Utilities.ReduceToFourBits(Tables.SBoxFive, splitBits[4]))
            .Concat(Utilities.ReduceToFourBits(Tables.SBoxSix, splitBits[5]))
            .Concat(Utilities.ReduceToFourBits(Tables.SBoxSeven, splitBits[6]))
            .Concat(Utilities.ReduceToFourBits(Tables.SBoxEight, splitBits[7])).ToArray();
        
        var pBox = Utilities.Permutate(concatenated, Tables.PermutationP);
        return pBox;
    }

    private string ConvertToBase64(List<bool[]> bitBlocks)
    {
        var totalLength = bitBlocks.Sum(b => b.Length);
        var concatAll = new bool[totalLength];

        int currentIndex = 0;
        foreach (var block in bitBlocks)
        {
            for (int i = 0; i < block.Length; i++)
            {
                concatAll[currentIndex++] = block[i];
            }
        }

        var bytes = concatAll.ConvertToByteArray();
        return Convert.ToBase64String(bytes);
    }

    private string ConvertToText(List<bool[]> bitBlocks)
    {
        var totalLength = bitBlocks.Sum(b => b.Length);
        var concatAll = new bool[totalLength];

        int currentIndex = 0;
        foreach (var block in bitBlocks)
        {
            for (int i = 0; i < block.Length; i++)
            {
                concatAll[currentIndex++] = block[i];
            }
        }

        var bytes = concatAll.ConvertToByteArray();
        var unpaddedBytes = RemovePadding(bytes);
        return System.Text.Encoding.ASCII.GetString(unpaddedBytes);
    }
    
    private byte[] RemovePadding(byte[] bytes)
    {
        int i = bytes.Length - 1;
        while (i >= 0 && bytes[i] == 0)
        {
            i--;
        }

        var result = new byte[i + 1];
        Array.Copy(bytes, result, i + 1);

        return result;
    }
}