using System.Linq;

namespace Wordel.Model;

public record struct AnswerLetter(char Value)
{
    public LetterUse UseColor(GameState state, int position)
    {
        if (state.CorrectAnswer.Value[position] == Value)
        {
            return LetterUse.Currect;
        }
        var self = this;
        if (state.CorrectAnswer.Value.Any(c => c == self.Value))
        {
            return LetterUse.Possible;
        }

        return LetterUse.Wrong;
    }
}
