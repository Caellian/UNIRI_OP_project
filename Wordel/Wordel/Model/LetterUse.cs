using System.Collections.Generic;

namespace Wordel.Model;

public enum LetterUse
{
    Unknown = 0,
    Wrong = 1,
    Possible = 2,
    Currect = 3
}

public static class WordUtil
{
    public static LetterUse[] MatchInput(string target, string current, int maxLength)
    {
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
                remLet[letter] -= 1;
                result[i] = LetterUse.Currect;
                continue;
            }

            if (remLet.GetValueOrDefault(letter) > 0)
            {
                remLet[letter] -= 1;
                result[i] = LetterUse.Possible;
                continue;
            }

            result[i] = LetterUse.Wrong;
        }

        return result;
    }
}

