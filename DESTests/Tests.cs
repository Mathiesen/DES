using System.Collections;
using System.Xml.Xsl;
using DES;

namespace DESTests;

public class Tests
{
    KeySchedule keySchedule;
    
    [SetUp]
    public void Setup()
    {
        keySchedule = new KeySchedule();
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
        
        // Act
        var blocks = Utilities.CreateBlocks("Hello World!");
        
        // Assert
        Assert.That(blocks.ToList()[1].Length, Is.EqualTo(64));
    }
    
    [Test]
    public void CanRemoveParityBitsFromKey()
    {
        // Arrange
        string key = "TEST1234";

        // Act
        var keyAsBits = keySchedule.PermutateKey(key, Tables.PermutedChoiceOne);

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
    public void RunTransform()
    {
        keySchedule.Transform(
            new BitArray([true, false, true, false]), 
            new BitArray([false, false, true, false]));
    }

    [Test]
    public void CanSplitBits()
    {
        // Arrange 
        BitArray bitArray = new BitArray([true, false, true, false, false, false, true, false]);
        BitArray actualLeft = new BitArray([true, false, true, false]);
        BitArray actualRight = new BitArray([false, false, true, false]);
        
        // Act
        (BitArray resultLeft, BitArray resultRight) = keySchedule.Split(bitArray);

        // Assert
        Assert.That(actualLeft, Is.EqualTo(resultLeft));
        Assert.That(actualRight, Is.EqualTo(resultRight));
    }
    
    [Test]
    public void CanConcatenateBitArrays()
    {
        // Arrange
        var left = new BitArray([false, false, true, false, false, false, true, false]);
        var right = new BitArray([true, false, true, true, true, false, true, true]);
        
        var expected = new BitArray([false, false, true, false, false, false, true, 
            false, true, false, true, true, true, false, true, true]);
        
        // Act
        var actual = Utilities.Concatenate(left, right);
        
        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [TestCase(1, new[] { false, true, false, false, false, true, false, true })]
    [TestCase(2, new[] { true, false, false, false, true, false, true, false })]
    public void LeftShiftBits(int shift, bool[] expected)
    {
        // Arrange
        var bits = new BitArray([true, false, true, false, false, false, true, false]);
        
        // Act
        BitArray actual = Utilities.LeftShift(bits, shift);

        // Assert
        Assert.That(actual, Is.EqualTo(new BitArray(expected)));
    }
}