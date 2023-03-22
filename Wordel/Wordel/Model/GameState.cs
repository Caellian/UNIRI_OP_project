using System;
using System.Collections;
using System.Collections.Generic;

namespace Wordel.Model;

public class GameState
{
    public string CorrectAnswer;
    public List<string> Answers;
    public int CurrentTry;

    public int WordLength => CorrectAnswer.Length; 
    public int MaxAnswers => Answers.Capacity; 
    
    public GameState(Settings settings)
    {
        CorrectAnswer = WordList.GetRandomSized(settings.WordLength);
        Answers = new List<string>(settings.MaxAnswers);
        for (var i = 0; i < Answers.Capacity; i++)
        {
            Answers.Add(new string(""));
        }

        CurrentTry = 0;
    }

    public void Reset()
    {
        CorrectAnswer = WordList.GetRandomSized(CorrectAnswer.Length);
        Answers = new List<string>(Answers.Capacity);
        for (var i = 0; i < Answers.Capacity; i++)
        {
            Answers.Add(new string(""));
        }

        CurrentTry = 0;
    }
}