using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using ReactiveUI;
using Wordel.Components;
using Wordel.ViewModels;

namespace Wordel.Views;

public partial class GameView : UserControl
{
    public GameView()
    {
        InitializeComponent();
    }

    private void RebuildLetterGrid()
    {
        var state = (DataContext as GameViewModel)?.State;

        AnswerStackPanel.Children.Clear();
        for (var i = 0; i < state.Settings.MaxAnswers; i++)
        {
            var af = new AnswerField();
            af.CurrentAnswer = state.Answers[i].Value;
            af.MaxLength = state.Settings.WordLength;
            af.Width = af.ContentWidth; // TODO(tin): Setting AnswerField width manually
            
            AnswerStackPanel.Children.Add(af);
        }
    }
    
    protected override void OnLoaded()
    {
        var ctx = (DataContext as GameViewModel);
        ctx.Changed.Subscribe(delegate(IReactivePropertyChangedEventArgs<IReactiveObject> args)
        {
            RebuildLetterGrid();
        });
        RebuildLetterGrid();
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