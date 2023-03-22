using System.Collections;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Layout;
using MessageBox.Avalonia.Enums;
using ReactiveUI;
using Wordel.Components;
using Wordel.Model;

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
        if (CurrentAnswer.Length < State.WordLength)
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
        if (current.Length == State.WordLength)
        {
            if (!WordList.TestWord(current))
            {
                var dialog = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("Nepoznata riječ",
                    "Unesena riječ se ne nalazi u rječniku.", icon: Icon.Warning);
                dialog.Show();
                return;
            }
            
            if (current == State.CorrectAnswer)
            {
                Status = GameStatus.Win;
                return;
            }
            
            CurrentTry += 1;

            if (State.CurrentTry == State.MaxAnswers)
            {
                Status = GameStatus.Lose;
            }
        }
    }
}