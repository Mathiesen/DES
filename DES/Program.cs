using System.Collections;
using System.Text;

namespace DES;

class Program
{
    static void Main(string[] args)
    {
        string input = "This a string that should be encrypted with DES. DES has been replaced " +
                       "by AES as DES is not very secure, as it only has a key lenght of 56 bits." +
                       "By comparison AES can use key lengths of 128, 192 and 256 bits.";
        var blocks = Utilities.CreateBlocks(input).ToList();

        foreach (var block in blocks)
        {
            Console.WriteLine(block.Count);
        }
        
        var s = Utilities.ConvertBlocksToString(blocks);
        Console.WriteLine(s);
        //
        // string secretKey = "TEST!23?";
        // var permutatedKey = Utilities.PermutateKey(secretKey);
        // var splitKey = Utilities.Split(permutatedKey);



    }
    
}