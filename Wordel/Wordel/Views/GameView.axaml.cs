using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using ReactiveUI;
using Wordel.Components;
using Wordel.Model;
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
        for (var i = 0; i < state.MaxAnswers; i++)
        {
            var af = new AnswerField
            {
                CurrentAnswer = state.Answers[i],
                CorrectAnswer = i < state.CurrentTry ? state.CorrectAnswer : "",
                MaxLength = state.WordLength
            };
            af.Width = af.ContentWidth; // TODO(tin): Setting AnswerField width manually
            
            AnswerStackPanel.Children.Add(af);
        }
    }

    private void ShowStatus()
    {
        var ctx = (DataContext as GameViewModel);
        var status = ctx?.Status;
        
        var statusMsg = new TextPresenter
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            FontSize = 20,
            Margin = new Thickness(0, 5)
        };
        
        switch (status)
        {
            case GameStatus.Win:
                statusMsg.Text = "Pobjeda!";
                statusMsg.Foreground = Brushes.Green;
                
                AnswerStackPanel.Children.Add(statusMsg);
                break;
            case GameStatus.Lose:
                statusMsg.Text = "Izgubili ste!";
                statusMsg.Foreground = Brushes.Red;
                AnswerStackPanel.Children.Add(statusMsg);
                
                var correctDisplay = new TextPresenter
                {
                    Text = "Točan odgovor: '" + ctx.State.CorrectAnswer + "'",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontSize = 18,
                };
                AnswerStackPanel.Children.Add(correctDisplay);

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    protected override void OnLoaded()
    {
        base.OnLoaded();
        
        var ctx = (DataContext as GameViewModel);
        ctx?.Changed.Subscribe(delegate(IReactivePropertyChangedEventArgs<IReactiveObject> args)
        {
            switch (args.PropertyName)
            {
                case "State":
                case "CurrentAnswer":
                case "CurrentTry":
                    RebuildLetterGrid();
                    break;
                case "Status":
                    ShowStatus();
                    break;
            }
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
        model?.ToggleSettings();
    }
}