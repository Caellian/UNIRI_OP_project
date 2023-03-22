using System;
using ReactiveUI;
using Wordel.Model;

namespace Wordel.ViewModels;

public class MainWindowViewModel: ViewModelBase
{
    private ViewModelBase _content;

    public MainWindowViewModel()
    {
        _content = new GameViewModel(new Settings());
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
            GameViewModel gameModel => new SettingsViewModel(new Settings
            {
                MaxAnswers = gameModel.State.MaxAnswers, WordLength = gameModel.State.WordLength
            }),
            SettingsViewModel settingsModel => new GameViewModel(settingsModel.Settings),
            _ => Content
        };
    }
}