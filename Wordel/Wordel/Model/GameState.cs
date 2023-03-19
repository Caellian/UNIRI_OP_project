using System;

namespace Wordel.Model.Game;

public class GameState
{
    public static int WordLength = 5;
    public static int MaxAnswers = 5;
    
    public readonly string CorrectAnswer;
    public string?[] Answers = new string[MaxAnswers]; 
    
    private readonly Random _random = new();
    
    public GameState()
    {
        var wl = WordList.GetSized(WordLength);
        CorrectAnswer = wl[_random.Next() % wl.Length];
    }
}