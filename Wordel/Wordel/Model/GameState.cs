using System;

namespace Wordel.Model;

public class GameState
{
    public readonly string CorrectAnswer;
    public string[] Answers = new string[1];
    
    private readonly Random _random = new();
    
    public GameState(int wordLength)
    {
        var wl = WordList.GetSized(wordLength);
        CorrectAnswer = wl[_random.Next() % wl.Length];
    }
}