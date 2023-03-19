namespace Wordel.Model.Game;

public record struct Answer(string Value)
{
    
    
    public AnswerLetter? Letter(int index)
    {
        if (index >= Value.Length) return null;
        return new AnswerLetter(Value[index]);
    }

    public LetterUse GetLetterUse(int index)
    {
        Letter(index)?.UseColor()
    }
}