using System;
using Avalonia.Controls;
using Avalonia.Input;
using Wordel.ViewModels;

namespace Wordel.Views;

public partial class GameView : UserControl
{
    public GameView()
    {
        InitializeComponent();
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
        base.OnKeyUp(e);
    }

    private void NewGame_OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        var model = DataContext as GameViewModel;
        model?.StartNewGame();
    }

    private void Settings_OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        var model = Parent?.DataContext as MainWindowViewModel;
        model?.OpenSettings();
    }
}