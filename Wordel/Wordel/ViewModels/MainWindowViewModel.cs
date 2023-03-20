using System;
using ReactiveUI;
using Wordel.Model;

namespace Wordel.ViewModels;

public class MainWindowViewModel: ViewModelBase
{
    private ViewModelBase _content;

    public MainWindowViewModel(Limits limits)
    {
        Content = new GameViewModel(limits);
    }

    public ViewModelBase Content
    {
        get => _content;
        private set => this.RaiseAndSetIfChanged(ref _content, value);
    }

    public void OpenSettings()
    {
        var limits = (Content as GameViewModel)!.State.Limits;
        Content = new SettingsViewModel(limits);
    }
    
    public void CloseSettings()
    {
        var limits = (Content as SettingsViewModel)!.Limits;
        Content = new GameViewModel(limits);
    }
}