using ReactiveUI;
using Wordel.Model;

namespace Wordel.ViewModels;

public class SettingsViewModel: ViewModelBase
{
    private Limits _limits;

    public SettingsViewModel()
    {
        _limits = new Limits(5, 5);
    }
    
    public SettingsViewModel(Limits limits)
    {
        _limits = limits;
    }
    
    public Limits Limits
    {
        get => _limits;
        set => this.RaiseAndSetIfChanged(ref _limits, value);
    }
}