using System.Collections.Generic;

namespace Wordel.Model;

public enum LetterUse
{
    Unknown = 0,
    Wrong = 1,
    Possible = 2,
    Correct = 3
}

public static class WordUtil
{
    public static LetterUse[] MatchInput(string target, string current)
    {
        var maxLength = current.Length;
        
        var result = new LetterUse[maxLength];
        for (var i = 0; i < maxLength; i++)
        {
            result[i] = LetterUse.Unknown;
        }
        if (target == "")
        {
            return result;
        }
        
        var remLet = new Dictionary<char, int>(target.Length);
        foreach (var letter in target.ToCharArray())
        {
            if (remLet.ContainsKey(letter))
            {
                remLet[letter] += 1;
            }
            else
            {
                remLet[letter] = 1;
            }
        }

        for (var i = 0; i < maxLength; i++)
        {
            var letter = current[i];
            
            if (target[i] == current[i])
            {
                // is used correctly
                remLet[letter] -= 1;
                result[i] = LetterUse.Correct;
                continue;
            }

            var remUses = remLet.GetValueOrDefault(letter);
            if (remUses > 0)
            {
                // could be used; check if used correctly later:
                for (var j = i + 1; j < maxLength; j++)
                {
                    if (letter == current[j] && letter == target[j])
                    {
                        remUses -= 1;
                    }
                }

                // if no correct uses:
                if (remUses > 0)
                {
                    remLet[letter] -= 1;
                    result[i] = LetterUse.Possible;
                    continue;
                }
            }

            result[i] = LetterUse.Wrong;
        }

        return result;
    }
}

