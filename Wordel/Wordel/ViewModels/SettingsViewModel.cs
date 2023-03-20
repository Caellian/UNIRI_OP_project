using ReactiveUI;
using Wordel.Model;

namespace Wordel.ViewModels;

public class SettingsViewModel: ViewModelBase
{
    private Settings _settings;

    public SettingsViewModel()
    {
        _settings = new Settings();
    }
    
    public SettingsViewModel(Settings settings)
    {
        _settings = settings;
    }
    
    public Settings Settings
    {
        get => _settings;
        set => this.RaiseAndSetIfChanged(ref _settings, value);
    }
}