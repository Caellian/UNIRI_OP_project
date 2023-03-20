using System.Collections.Generic;

namespace Wordel.Model;

public class Answer
{
    public string Value { get; set; }

    public Answer(string value)
    {
        Value = value;
    }
    
    public AnswerLetter? Letter(int index)
    {
        if (index >= Value.Length) return null;
        return new AnswerLetter(Value[index]);
    }

    public LetterUse? GetLetterUse(GameState state, int index)
    {
        return Letter(index)?.UseColor(state, index);
    }
}