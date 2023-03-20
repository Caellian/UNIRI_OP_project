using System;
using System.Collections;
using System.Collections.Generic;

namespace Wordel.Model;

public class GameState
{
    public readonly Answer CorrectAnswer;
    public List<Answer> Answers;
    public int CurrentTry;
    
    public Settings Settings;
    
    private readonly Random _random = new();
    
    public GameState(Settings settings)
    {
        this.Settings = settings;
        var wl = WordList.GetSized(settings.WordLength);
        CorrectAnswer = new Answer(wl[_random.Next() % wl.Length]);
        Answers = new List<Answer>(settings.MaxAnswers);
        for (var i = 0; i < settings.MaxAnswers; i++)
        {
            Answers.Add(new Answer(""));
        }

        CurrentTry = 0;
    }
}