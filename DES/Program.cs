using System.Collections;
using System.Text;

namespace DES;

class Program
{
    static void Main(string[] args)
    {
        string input = "This a string that should be encrypted with DES";
        List<IEnumerable<byte>> blocks = Utilities.CreateBlocks(input).ToList();
        var s = Utilities.ConvertBlocksToString(blocks);
        Console.WriteLine(s);
        string secretKey = "TEST!23?";
        
        
    
    }
    
}