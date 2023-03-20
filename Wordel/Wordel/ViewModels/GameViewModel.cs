using System.Collections;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Layout;
using ReactiveUI;
using Wordel.Components;
using Wordel.Model;

namespace Wordel.ViewModels;

public class GameViewModel : ViewModelBase
{
    private GameState _state;

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

    public string CurrentAnswer
    {
        get => State.Answers[State.CurrentTry].Value;
        set => this.RaiseAndSetIfChanged(ref _state.Answers[_state.CurrentTry].Value, value);
    }

    public void StartNewGame()
    {
        this.RaiseAndSetIfChanged(ref _state, new GameState(_state.Settings));
    }

    public void EnterLetter(char letter)
    {
        if (CurrentAnswer.Length < State.Settings.WordLength)
        {
            CurrentAnswer += letter;
        }
    }

    public void ConfirmAnswer()
    {
        if (State.Answers[State.CurrentTry].Value.Length == State.Settings.WordLength)
        {
            // TODO: Check if correct
            State.CurrentTry += 1;
        }
    }
}