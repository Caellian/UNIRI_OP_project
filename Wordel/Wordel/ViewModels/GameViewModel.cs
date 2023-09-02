using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using Wordel.Data;
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

    public string? CurrentAnswer
    {
        get => State.CurrentTry < State.Answers.Count ? State.Answers[State.CurrentTry] : null;
        set
        {
            if (value == null) return;
            
            if (State.CurrentTry == State.Answers.Count)
            {
                State.Answers.Add(value);
            }
            else
            {
                _state.Answers[_state.CurrentTry] = value;
            }
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
        Status = GameStatus.Play;
        this.RaisePropertyChanged(nameof(State));
    }

    public void EnterLetter(char letter)
    {
        if (CurrentAnswer == null)
        {
            CurrentAnswer = letter.ToString();
        } else if (CurrentAnswer.Length < State.Settings.WordLength)
        {
            CurrentAnswer += letter;
        }
    }

    public void RemoveLetter()
    {
        if (CurrentAnswer == null) return;
        
        if (CurrentAnswer.Length > 0)
        {
            CurrentAnswer = CurrentAnswer.Substring(0, CurrentAnswer.Length - 1);
        }
    }

    public void ConfirmAnswer()
    {
        if (Status == GameStatus.Win)
        {
            StartNewGame();
            return;
        }
        
        var current = State.Answers[State.CurrentTry];
        if (current.Length != State.Settings.WordLength) return;
        
            
        if (!LocaleStorage.CurrentLocale!.WordList.TestWord(current))
        {
            var dialog = MessageBoxManager.GetMessageBoxStandard(LocaleStorage.GetTranslation("UnknownWord"),
                LocaleStorage.GetTranslation("InputNotInDict"), icon: Icon.Warning);
            dialog.ShowAsync();
            return;
        }
        
        CurrentTry += 1;
        
        if (current == State.CorrectAnswer)
        {
            Status = GameStatus.Win;
            Database.GetInstance().SavePlay(new PlayedGame
            {
                Word = State.CorrectAnswer,
                Attempts = CurrentTry,
                Victory = true
            });
            return;
        }

        if (State.CurrentTry != State.Settings.MaxAnswers) return;
        
        Status = GameStatus.Lose;
        Database.GetInstance().SavePlay(new PlayedGame
        {
            Word = State.CorrectAnswer,
            Attempts = CurrentTry,
            Victory = false
        });
    }
}