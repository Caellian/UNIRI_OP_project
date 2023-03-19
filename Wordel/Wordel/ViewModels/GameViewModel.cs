using Avalonia.Controls;
using ReactiveUI;
using Wordel.Model;

namespace Wordel.ViewModels;

public class GameViewModel : ViewModelBase
{
    public static int DefaultWordLength = 5;
    public static int DefaultMaxAnswers = 5;
    
    private int _wordLenght = DefaultWordLength;
    public int WordLength
    {
        get => _wordLenght;
        set => this.RaiseAndSetIfChanged(ref _wordLenght, value);
    }
    
    private int _maxAnswers = DefaultMaxAnswers;
    public int MaxAnswers
    {
        get => _maxAnswers;
        set => this.RaiseAndSetIfChanged(ref _maxAnswers, value);
    }
    
    private GameState _state = new(DefaultWordLength);
    
    public string Greeting => _state.CorrectAnswer;

    public void StartNewGame()
    {
        this.RaiseAndSetIfChanged(ref _state, new(WordLength));
    }
}