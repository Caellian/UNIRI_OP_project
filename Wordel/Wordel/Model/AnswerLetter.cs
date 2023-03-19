using System.Linq;

namespace Wordel.Model.Game;

public record struct AnswerLetter(char Value)
{
    public LetterUse UseColor(GameState state, int position)
    {
        if (state.CorrectAnswer[position] == Value)
        {
            return LetterUse.Currect;
        }
        var self = this;
        if (state.CorrectAnswer.Any(c => c == self.Value))
        {
            return LetterUse.Possible;
        }

        return LetterUse.Wrong;
    }
}
