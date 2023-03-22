using System;
using ReactiveUI;
using Wordel.Model;

namespace Wordel.ViewModels;

public class SettingsViewModel : ViewModelBase
{
    private Settings _settings;

    public Settings Settings
    {
        get => _settings;
        set => _settings = this.RaiseAndSetIfChanged(ref _settings, value);
    }

    public SettingsViewModel(Settings settings)
    {
        _settings = settings;
    }
}
