using ReactiveUI;
using Wordel.Model;

namespace Wordel.ViewModels;

public class MainViewModel: ViewModelBase
{
    private ViewModelBase _content;
    private Settings _settings;

    public MainViewModel()
    {
        _settings = new Settings();
        _content = new GameViewModel(_settings);
    }

    public ViewModelBase Content
    {
        get => _content;
        private set => this.RaiseAndSetIfChanged(ref _content, value);
    }

    public void ToggleSettings()
    {
        Content = Content switch
        {
            GameViewModel gameModel => new SettingsViewModel(gameModel.State.Settings),
            SettingsViewModel settingsModel => new GameViewModel(settingsModel.Settings),
            _ => Content
        };
    }
    
    public void ToggleLeaderboard()
    {
        switch (Content)
        {
            case GameViewModel gameModel:
                _settings = gameModel.State.Settings;
                Content = new StatsViewModel();
                break;
            case StatsViewModel:
                Content = new GameViewModel(_settings);
                break;
        }
    }
}