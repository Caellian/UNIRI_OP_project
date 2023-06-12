using Wordel.Model;

namespace Wordel.Tests;

public class TextMatchingTest
{
    [Fact]
    public void MatchInput_EmptyTarget_ReturnsArrayOfUnknowns()
    {
        string target = "";
        string current = "example";
        
        var result = WordUtil.MatchInput(target, current);
        
        foreach (var letterUse in result)
        {
            Assert.Equal(LetterUse.Unknown, letterUse);
        }
    }
    
    [Fact]
    public void MatchInput_MatchingLetters_ReturnsArrayOfCorrects()
    {
        string target = "example";
        string current = "example";
        
        var result = WordUtil.MatchInput(target, current);
        
        foreach (var letterUse in result)
        {
            Assert.Equal(LetterUse.Correct, letterUse);
        }
    }
    
    [Fact]
    public void MatchInput_PossibleLetters_ReturnsArrayOfPossibles()
    {
        string target = "example";
        string current = "elpmaxe";
        
        var result = WordUtil.MatchInput(target, current);
        
        Assert.Equal(LetterUse.Correct, result[0]);
        Assert.Equal(LetterUse.Possible, result[1]);
        Assert.Equal(LetterUse.Possible, result[2]);
        Assert.Equal(LetterUse.Correct, result[3]);
        Assert.Equal(LetterUse.Possible, result[4]);
        Assert.Equal(LetterUse.Possible, result[5]);
        Assert.Equal(LetterUse.Correct, result[6]);
    }
    
    [Fact]
    public void MatchInput_OnePossible_ReturnsWrongsIfCorrectlyUsedLater()
    {
        string target = "exampule";
        string current = "wronglly";
        
        var result = WordUtil.MatchInput(target, current);
        
        Assert.Equal(LetterUse.Wrong, result[0]);
        Assert.Equal(LetterUse.Wrong, result[1]);
        Assert.Equal(LetterUse.Wrong, result[2]);
        Assert.Equal(LetterUse.Wrong, result[3]);
        Assert.Equal(LetterUse.Wrong, result[4]);
        Assert.Equal(LetterUse.Wrong, result[5]);
        Assert.Equal(LetterUse.Correct, result[5]);
        Assert.Equal(LetterUse.Wrong, result[6]);
    }
}