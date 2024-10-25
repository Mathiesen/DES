using System.Collections;
using System.Xml.Xsl;
using DES;

namespace DESTests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void InputCanConvertToByteArray()
    {
        // Arrange
        string input = "Hello World!";
        byte[] actual = new[]
        {
            (byte)'H',
            (byte)'e',
            (byte)'l',
            (byte)'l',
            (byte)'o',
            (byte)' ',
            (byte)'W',
            (byte)'o',
            (byte)'r',
            (byte)'l',
            (byte)'d',
            (byte)'!',
        };
        
        // Act 
        byte[]? result = Utilities.ConvertToByteArray(input);
        
        // Assert
        Assert.That(actual, Is.EqualTo(result));
    }

    [Test]
    public void InputCanBeBrokenUpInto8ByteBlocks()
    {
        // Arrange
        
        // Act 
        var blocks = Utilities.CreateBlocks("Hello World!");

        // Assert
        Assert.That(blocks.Count, Is.EqualTo(2));
    }

    [Test]
    public void BlocksArePaddedWithZeroBits()
    {
        // Arrange 
        var blocks = Utilities.CreateBlocks("Hello World!");
        
        // Act
        
        
        // Assert
        Assert.That(blocks.ToList()[1].Length, Is.EqualTo(64));
    }
    
    [Test]
    public void CanRemoveParityBitsFromKey()
    {
        // Arrange
        string key = "TEST1234";

        // Act
        var keyAsBits = Utilities.PermutateKey(key);

        // Assert
        Assert.That(keyAsBits.Length, Is.EqualTo(56));
    }

    [Test]
    public void KeyCanBeConvertedToString()
    {
        // Arrange
        string key = "TEST1234";
        var keyAsBytes = Utilities.ConvertToByteArray(key);
        var keyAsBits = new BitArray(keyAsBytes);
        
        // Act
        string result = Utilities.BitArrayToString(keyAsBits);

        // Assert
        Assert.That(result, Is.EqualTo(key));
    }

    [Test]
    public void BitBlocksCanBeConvertedToString()
    {
        // Arrange
        string phrase = "This string should be converted to bits and back again";
        var blocks = Utilities.CreateBlocks(phrase);
        
        // Act
        var resultPhrase = Utilities.ConvertBlocksToString(blocks);

        // Assert 
        Assert.That(resultPhrase, Is.EqualTo(phrase));
    }

    [Test]
    public void CanSplitBits()
    {
        // Arrange 
        BitArray bitArray = new BitArray([true, false, true, false, false, false, true, false]);
        BitArray actualLeft = new BitArray([true, false, true, false]);
        BitArray actualRight = new BitArray([false, false, true, false]);
        
        // Act
        (BitArray resultLeft, BitArray resultRight) = Utilities.Split(bitArray);

        // Assert
        Assert.That(actualLeft, Is.EqualTo(resultLeft));
        Assert.That(actualRight, Is.EqualTo(resultRight));
    }
}