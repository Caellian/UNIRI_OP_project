using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ReactiveUI;
using Wordel.Model;
using Wordel.Util;

namespace Wordel.ViewModels;

public class SettingsViewModel : ViewModelBase
{
    private Settings _settings;

    public Settings Settings
    {
        get => _settings;
        set => _settings = this.RaiseAndSetIfChanged(ref _settings, value);
    }

    public string ScreenTitle
    {
        get => LocaleStorage.GetTranslation("ScreenSettings");
    }
    
    public IEnumerable<(FieldInfo field, Configurable conf)> SettingFields => (
            from field in typeof(Settings).GetFields() select
                (field, conf: field.GetCustomAttributes(typeof(Configurable), true).First() as Configurable)
            ).Where(it => it.conf != null);

    public SettingsViewModel()
    {
        _settings = new Settings();
    }
    
    public SettingsViewModel(Settings settings)
    {
        _settings = settings;
    }
}
