using System.Collections;

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
        var blocks = Utilities.CreateBlocks(text);
        List<BitArray> finalBlocks = new List<BitArray>();
        
        foreach (var block in blocks)
        {
            var permutatedBlock = Utilities.Permutate(block, Tables.InitialPermutation);
            var split = Utilities.Split(permutatedBlock);

            var rightBits = split.right;
            var leftBits = split.left;

            
            
           var (l, r) = RunRounds(rightBits, leftBits, 0);
           var concatenated = Utilities.Concatenate(leftBits, rightBits);
           var finalPermutation = Utilities.Permutate(concatenated, Tables.FinalPermutation);
           finalBlocks.Add(finalPermutation);
        }

        int totalLenght = 0;
        foreach (var bitArray in finalBlocks)
        {
            totalLenght += bitArray.Length;
        }
        
        var concatAll = new BitArray(totalLenght);

        int currentIndex = 0;
        foreach (var block in finalBlocks)
        {
            for (int i = 0; i < block.Length; i++)
            {
                concatAll[currentIndex++] = block[i];
            }
        }

        var bytes = Utilities.ConvertToByteArray(concatAll);
        return Convert.ToBase64String(bytes);
    }

    private (BitArray left, BitArray right) RunRounds(BitArray rightBits, BitArray leftBits, int index)
    {
        var function = RightBits(rightBits, keySchedule.Keys[index]);
        var leftRightXor = leftBits.Xor(function);
        var newLeft = rightBits;
        var newRight = leftRightXor;

        while (index < keySchedule.Keys.Count - 1)
        {
            index++;
            (newLeft, newRight) = RunRounds(newRight, newLeft, index);
        }

        return (leftRightXor, rightBits);
    }

    private BitArray RightBits(BitArray splitRight, BitArray keyScheduleKey)
    {
        var expanded = Utilities.Expand(splitRight);
        var xor = expanded.Xor(keyScheduleKey);
        
        var splitBits = Utilities.Split(xor, 6);

        var bools = splitBits.ToArray();
        var sBoxOneOutput = Utilities.ReduceToFourBits(Tables.SBoxOne, bools[0]);
        var sBoxTwoOutput = Utilities.ReduceToFourBits(Tables.SBoxTwo, bools[1]);
        var sBoxThreeOutput = Utilities.ReduceToFourBits(Tables.SBoxThree, bools[2]);
        var sBoxFourOutput = Utilities.ReduceToFourBits(Tables.SBoxFour, bools[3]);
        var sBoxFiveOutput = Utilities.ReduceToFourBits(Tables.SBoxFive, bools[4]);
        var sBoxSixOutput = Utilities.ReduceToFourBits(Tables.SBoxSix, bools[5]);
        var sBoxSevenOutput = Utilities.ReduceToFourBits(Tables.SBoxSeven, bools[6]);
        var sBoxEightOutput = Utilities.ReduceToFourBits(Tables.SBoxEight, bools[7]);

        var concatenated = sBoxOneOutput
            .Concat(sBoxTwoOutput)
            .Concat(sBoxThreeOutput)
            .Concat(sBoxFourOutput)
            .Concat(sBoxFiveOutput)
            .Concat(sBoxSixOutput)
            .Concat(sBoxSevenOutput)
            .Concat(sBoxEightOutput).ToArray();

        var permutatedP = Utilities.Permutate(new BitArray(concatenated), Tables.PermutationP);
        
        return permutatedP;
    }

    private void LeftBits(BitArray splitLeft)
    {
        throw new NotImplementedException();
    }
}