using System;
using System.Collections.Generic;
using Wordel.Util;
#if DEBUG
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
#endif

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
        CorrectAnswer = RandomWord(settings.WordLength) ?? "";
        Answers = new List<string>(settings.MaxAnswers);
        CurrentTry = 0;
    }

    public void Reset()
    {
        CorrectAnswer = RandomWord(Settings.WordLength) ?? "";
        #if DEBUG
            var dialog = MessageBoxManager.GetMessageBoxStandard("Answer",
                CorrectAnswer, icon: Icon.Info);
            dialog.ShowAsync();
        #endif
        Answers = new List<string>(Settings.MaxAnswers);
        CurrentTry = 0;
    }

    public static string? RandomWord(int length)
    {
        var words = LocaleStorage.CurrentLocale!.WordList.GetSized(length);
        
        if (words.Length > 0)
        {
            return words[_rand.Next(words.Length)];
        }

        return null;
    }
}