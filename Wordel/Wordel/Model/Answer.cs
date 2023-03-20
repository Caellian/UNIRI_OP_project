using System.Collections.Generic;

namespace Wordel.Model;

public class Answer
{
    public string Value;
    private bool _locked;

    public bool Locked { get => _locked; set => _locked = value; }

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
        if (Locked)
        {
            return Letter(index)?.UseColor(state, index);
        }
        else
        {
            return LetterUse.Unknown;
        }
    }
}