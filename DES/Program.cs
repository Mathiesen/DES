using System.Collections;
using System.Text;

namespace DES;

class Program
{
    static void Main(string[] args)
    {
        string input = "This a string that should be encrypted with DES. DES has been replaced " +
                       "by AES as DES is not very secure, as it only has a key lenght of 56 bits. " +
                       "By comparison AES can use key lengths of 128, 192 and 256 bits.";
        string key = "12345678";
        
        DES des = new DES();
        var encrypted = des.Encrypt(input, key);
        Console.WriteLine(encrypted);
        
    }
    
}