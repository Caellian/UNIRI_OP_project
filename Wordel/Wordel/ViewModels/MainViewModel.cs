using Wordel.Model.Game;

namespace Wordel.ViewModels;

public class MainViewModel : ViewModelBase
{
    public GameState State = new();
    
    public string Greeting => State.CorrectAnswer;
}