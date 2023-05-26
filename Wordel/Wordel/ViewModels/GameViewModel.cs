using System.Collections;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Layout;
using MessageBox.Avalonia.Enums;
using ReactiveUI;
using Wordel.Components;
using Wordel.Model;
using Wordel.Util;

namespace Wordel.ViewModels;

public class GameViewModel : ViewModelBase
{
    private GameState _state;

    private GameStatus _status = GameStatus.Play;

    public GameViewModel()
    {
        _state = new GameState(new Settings());
    }

    public GameViewModel(Settings settings)
    {
        _state = new GameState(settings);
    }
    
    public GameState State
    {
        get => _state;
        set => this.RaiseAndSetIfChanged(ref _state, value);
    }

    public GameStatus Status
    {
        get => _status;
        set => this.RaiseAndSetIfChanged(ref _status, value);
    }

    public string CurrentAnswer
    {
        get => State.Answers[State.CurrentTry];
        set
        {
            _state.Answers[_state.CurrentTry] = value;
            this.RaisePropertyChanged();
        }
    }

    public int CurrentTry
    {
        get => _state.CurrentTry;
        set => this.RaiseAndSetIfChanged(ref _state.CurrentTry, value);
    }

    public void StartNewGame()
    {
        State.Reset();
        this.RaisePropertyChanged(nameof(State));
    }

    public void EnterLetter(char letter)
    {
        if (CurrentAnswer.Length < State.Settings.WordLength)
        {
            CurrentAnswer += letter;
        }
    }

    public void RemoveLetter()
    {
        if (CurrentAnswer.Length > 0)
        {
            CurrentAnswer = CurrentAnswer.Substring(0, CurrentAnswer.Length - 1);
        }
    }

    public void ConfirmAnswer()
    {
        var current = State.Answers[State.CurrentTry];
        if (current.Length == State.Settings.WordLength)
        {
            if (current == State.CorrectAnswer)
            {
                Status = GameStatus.Win;
                return;
            }
            
            if (!LocaleStorage.CurrentLocale!.WordList.TestWord(current))
            {
                var dialog = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(LocaleStorage.GetTranslation("UnknownWord"),
                    LocaleStorage.GetTranslation("InputNotInDict"), icon: Icon.Warning);
                dialog.Show();
                return;
            }
            
            CurrentTry += 1;

            if (State.CurrentTry == State.Settings.MaxAnswers)
            {
                Status = GameStatus.Lose;
            }
        }
    }
}