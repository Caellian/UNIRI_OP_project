using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Wordel.Model;
using Wordel.Util;
using Wordel.ViewModels;
using Wordel.Views;

namespace Wordel;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        LocaleStorage.CurrentCulture = LocaleStorage.SupportedCultures[0];
        
        switch (ApplicationLifetime)
        {
            case IClassicDesktopStyleApplicationLifetime desktop:
                desktop.MainWindow = new MainWindowView
                {
                    DataContext = new MainWindowViewModel()
                };
                break;
            case ISingleViewApplicationLifetime singleViewPlatform:
                singleViewPlatform.MainView = new MainWindowView
                {
                    DataContext = new MainWindowViewModel()
                };
                break;
        }

        base.OnFrameworkInitializationCompleted();
    }
}