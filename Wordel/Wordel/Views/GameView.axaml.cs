using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using DynamicData;
using ReactiveUI;
using Wordel.Components;
using Wordel.Model;
using Wordel.Util;
using Wordel.ViewModels;

namespace Wordel.Views;

public partial class GameView : UserControl
{
    public GameView()
    {
        InitializeComponent();
    }

    private void RebuildLetterGrid(GameViewModel model)
    {
        var state = model.State;
        var maxAnswers = state?.Settings.MaxAnswers ?? 0;

        var ans = new TextBox()
        {
            Text = state?.CorrectAnswer ?? "null"
        };
        
        
        AnswerStackPanel.Children.Clear();
        AnswerStackPanel.Children.Add(ans);
        
        for (var i = 0; i < maxAnswers; i++)
        {
            var currentAnswer = state.Answers.Count > i ? state.Answers[i] : "";
            var af = new AnswerField
            {
                CurrentAnswer = currentAnswer,
                CorrectAnswer = i < state.CurrentTry ? state.CorrectAnswer : "",
                MaxLength = state.Settings.WordLength
            };
            af.Width = af.ContentWidth; // TODO(tin): Setting AnswerField width manually

            AnswerStackPanel.Children.Add(af);
        }
    }

    private void BuildKeyboard(GameViewModel model)
    {
        KeyboardStackPanel.Children.Clear();
        
        var rows = LocaleStorage.CurrentLocale!.Keyboard.lower;
        foreach (var row in rows)
        {
            var currentRow = new StackPanel();
            currentRow.Orientation = Orientation.Horizontal;
            currentRow.Spacing = 5;
            currentRow.Margin = new Thickness(15.0 * KeyboardStackPanel.Children.Count, 0, 0, 0);
            
            foreach (var letter in row)
            {
                var button = new Button
                {
                    Content = letter.ToString(),
                    Command = ReactiveCommand.Create(() =>
                    {
                        model.EnterLetter(letter);
                    }),
                    FontSize = 20.0,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Width = 30.0,
                    Height = 32.0,
                    Padding = new Thickness()
                };
                button.Classes.Add("keyboard");
                currentRow.Children.Add(button);
            }

            if (KeyboardStackPanel.Children.Count == 2)
            {
                var eraseButton = new Button
                {
                    Content = "&lt;-",
                    Command = ReactiveCommand.Create(() =>
                    {
                        model.RemoveLetter();
                    })
                };
                eraseButton.Classes.Add("keyboard");
                currentRow.Children.Add(eraseButton);
            }
            KeyboardStackPanel.Children.Add(currentRow);
        }
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        var content = (DataContext as MainWindowViewModel)?.Content;
        if (content is not GameViewModel model) return;
        
        switch (e.Key)
        {
            case Key.Enter:
                if (model.State.CurrentTry < model.State.Settings.MaxAnswers)
                {
                    model.ConfirmAnswer();
                }
                else
                {
                    model.StartNewGame();
                }
                break;
            case Key.Back:
                model.RemoveLetter();
                break;
            default:
            {
                var entered = e.Key.ToString();
                if (entered.Length == 1)
                {
                    model.EnterLetter(entered.ToLower()[0]);
                }
                break;
            }
        }
    }

    private void ShowStatus(GameViewModel model)
    {
        var status = model.Status;
        
        var statusMsg = new TextPresenter
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            FontSize = 20,
            Margin = new Thickness(0, 5)
        };
        
        MessageStackPanel.Children.Clear();
        switch (status)
        {
            case GameStatus.Win:
                statusMsg.Text = LocaleStorage.GetTranslation("GameVictory");
                statusMsg.Foreground = Brushes.Green;
                
                MessageStackPanel.Children.Add(statusMsg);
                break;
            case GameStatus.Lose:
                statusMsg.Text = LocaleStorage.GetTranslation("GameFail");
                statusMsg.Foreground = Brushes.Red;
                MessageStackPanel.Children.Add(statusMsg);
                
                var correctDisplay = new TextPresenter
                {
                    Text = LocaleStorage.Format("{{CorrectAnswer}}: '" + model.State.CorrectAnswer + "'"),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontSize = 18,
                };
                MessageStackPanel.Children.Add(correctDisplay);

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
                    RebuildLetterGrid(ctx);
                    MessageStackPanel.Children.Clear();
                    break;
                case "Status":
                    ShowStatus(ctx);
                    break;
            }
        });
        
        if (ctx != null)
        {
            RebuildLetterGrid(ctx);
            BuildKeyboard(ctx);
        }
    }

    private void NewGame_OnPointerReleased(object? sender, RoutedEventArgs routedEventArgs)
    {
        var model = DataContext as GameViewModel;
        model?.StartNewGame();
    }

    private void Settings_OnPointerReleased(object? sender, RoutedEventArgs routedEventArgs)
    {
        var model = Parent?.DataContext as MainWindowViewModel;
        model?.ToggleSettings();
    }

    private void Leaderboard_OnPointerReleased(object? sender, RoutedEventArgs routedEventArgs)
    {
        var model = Parent?.DataContext as MainWindowViewModel;
        model?.ToggleLeaderboard();
    }
}