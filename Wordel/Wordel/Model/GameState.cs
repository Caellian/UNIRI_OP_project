using System;
using System.Collections;
using System.Collections.Generic;
using Wordel.Util;

namespace Wordel.Model;

public class GameState
{
    public Settings Settings;
    public string CorrectAnswer;
    public List<string> Answers;
    public int CurrentTry;
    static Random _rand = new();

    public GameState(Settings settings)
    {
        Settings = settings;
        CorrectAnswer = RandomWord(settings.WordLength);
        Answers = new List<string>(settings.MaxAnswers);
        // TODO: Remove greedy answer init
        for (var i = 0; i < Answers.Capacity; i++)
        {
            Answers.Add(new string(""));
        }

        CurrentTry = 0;
    }

    public void Reset()
    {
        CorrectAnswer = RandomWord(CorrectAnswer.Length);
        Answers = new List<string>(Answers.Capacity);
        for (var i = 0; i < Answers.Capacity; i++)
        {
            Answers.Add(new string(""));
        }

        CurrentTry = 0;
    }

    static string RandomWord(int length)
    {
        var words = LocaleStorage.CurrentLocale!.WordList.GetSized(length);
        return words[_rand.Next(words.Length)];
    }
}