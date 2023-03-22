using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Wordel.ViewModels;

namespace Wordel.Views;

public partial class SettingsView : UserControl
{
    public SettingsView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void Close_OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        var model = Parent?.DataContext as MainWindowViewModel;
        model?.ToggleSettings();
    }

    private void WordLength_OnValueChanged(object? sender, NumericUpDownValueChangedEventArgs e)
    {
        var model = (DataContext as SettingsViewModel);
        model.Settings = model.Settings with {WordLength = (int) e.NewValue};
    }

    private void MaxAnswers_OnValueChanged(object? sender, NumericUpDownValueChangedEventArgs e)
    {
        var model = (DataContext as SettingsViewModel);
        model.Settings = model.Settings with {MaxAnswers = (int) e.NewValue};
    }
}