using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Wordel.ViewModels;

namespace Wordel.Views;

public partial class StatsView : UserControl
{
    public StatsView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void Close_OnPointerReleased(object? sender, RoutedEventArgs routedEventArgs)
    {
        var model = Parent?.DataContext as MainViewModel;
        model?.ToggleLeaderboard();
    }
}