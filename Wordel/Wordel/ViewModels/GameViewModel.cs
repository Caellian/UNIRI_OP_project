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
    public static int DefaultWordLength = 5;
    public static int DefaultMaxAnswers = 5;

    private GameState _state = new(new Limits(DefaultWordLength, DefaultMaxAnswers));
    public GameState State
    {
        get => _state;
        set => this.RaiseAndSetIfChanged(ref _state, value);
    }

    public List<Answer> Answers => State.Answers;
    public int CurrentTry => State.Answers.Count;
    
    public Answer CurrentAnswer => State.Answers[CurrentTry];

    public void StartNewGame()
    {
        this.RaiseAndSetIfChanged(ref _state, new GameState(_state.Limits));
    }
}