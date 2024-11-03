using System.Collections;
using System.Text;
using NUnit.Framework;
using DES;

namespace DESTests;

public class Tests
{
    private KeySchedule keySchedule;

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
        byte[] expected = Encoding.UTF8.GetBytes(input);
        
        // Act 
        byte[] result = Utilities.ConvertToByteArray(input);
        
        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void InputCanBeBrokenUpInto8ByteBlocks()
    {
        // Arrange
        byte[] input = Encoding.UTF8.GetBytes("Hello world");

        // Act 
        var blocks = Utilities.CreateBlocks(input);

        // Assert
        Assert.That(blocks.Count(), Is.EqualTo(2));
    }

    [Test]
    public void BlocksArePaddedWithZeroBits()
    {
        // Arrange 
        byte[] input = Encoding.UTF8.GetBytes("Hello World!");

        // Act
        var blocks = Utilities.CreateBlocks(input);

        // Assert
        Assert.That(blocks.Last().Length, Is.EqualTo(64));
    }
    
    [Test]
    public void CanRemoveParityBitsFromKey()
    {
        // Arrange
        string key = "TEST1234";

        // Act
        var keyAsBits = Utilities.Permutate(key, Tables.PermutedChoiceOne);

        // Assert
        Assert.That(keyAsBits.Length, Is.EqualTo(56));
    }

    [Test]
    public void RunKeySchedule()
    {
        // Act
        keySchedule.Run("TEST1234");

        // Assert
        Assert.That(keySchedule.Keys, Is.Not.Empty);
        Assert.That(keySchedule.Keys.Count, Is.EqualTo(16));  // Should generate 16 keys for DES
    }

    [Test]
    public void CanSplitBits()
    {
        // Arrange 
        bool[] bits = { true, false, true, false, false, false, true, false };
        bool[] expectedLeft = [true, false, true, false];
        bool[] expectedRight = [false, false, true, false];
        
        // Act
        (bool[] resultLeft, bool[] resultRight) = bits.Split();

        // Assert
        Assert.That(resultLeft, Is.EqualTo(expectedLeft));
        Assert.That(resultRight, Is.EqualTo(expectedRight));
    }
    
    [Test]
    public void CanConcatenateBitArrays()
    {
        // Arrange
        bool[] left = { false, false, true, false, false, false, true, false };
        bool[] right = { true, false, true, true, true, false, true, true };
        bool[] expected = { false, false, true, false, false, false, true, false, true, false, true, true, true, false, true, true };
        
        // Act
        var actual = left.Concatenate(right);
        
        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Expand_ShouldReturnCorrectExpandedBitArray()
    {
        // Arrange
        bool[] bits = { true, false, true, false, true };
        Tables.Expansion = new byte[] { 1, 3, 5, 2, 4, 1 };
        bool[] expectedResult = { true, true, true, false, false, true };

        // Act
        var result = bits.Expand();

        // Assert
        Assert.That(result.Length, Is.EqualTo(expectedResult.Length), "Resulting BitArray length is incorrect.");

        for (int i = 0; i < expectedResult.Length; i++)
        {
            Assert.That(result[i], Is.EqualTo(expectedResult[i]), $"Bit at position {i} is incorrect.");
        }
    }

    [Test]
    public void CanReduce6BitsTo4BitsBySBox()
    {
        // Arrange
        var expected = new bool[] { true, true, true, true };

        // Act
        var actual = Utilities.ReduceToFourBits(Tables.SBoxOne, new bool[] { true, false, false, false, false, true });
        
        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Split_CanSplit48BitsInto8Times6Bits()
    {
        // Arrange
        var bytes = Utilities.ConvertToByteArray("48bits");
        var bits = Utilities.ConvertBytesToBits(bytes);

        // Act
        var split = bits.Split(6);

        // Assert
        Assert.That(split.Count(), Is.EqualTo(8));
    }

    [TestCase(1, new[] { false, true, false, false, false, true, false, true })]
    [TestCase(2, new[] { true, false, false, false, true, false, true, false })]
    public void LeftShiftBits(int shift, bool[] expected)
    {
        // Arrange
        bool[] bits = { true, false, true, false, false, false, true, false };

        // Act
        bool[] actual = bits.LeftShift(shift);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }
}
