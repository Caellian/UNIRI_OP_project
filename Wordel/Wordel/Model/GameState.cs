using System;
using System.Collections;
using System.Collections.Generic;

namespace Wordel.Model;

public class GameState
{
    public readonly Answer CorrectAnswer;
    public List<Answer> Answers;
    public Limits Limits;
    
    private readonly Random _random = new();
    
    public GameState(Limits limits)
    {
        this.Limits = limits;
        var wl = WordList.GetSized(limits.WordLength);
        CorrectAnswer = new Answer(wl[_random.Next() % wl.Length]);
        Answers = new List<Answer>(limits.MaxAnswers);
    }
}