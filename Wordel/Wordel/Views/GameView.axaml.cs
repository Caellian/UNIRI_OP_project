using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
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

    private void RebuildLetterGrid()
    {
        if (DataContext is not GameViewModel model) return;
        
        var state = model.State;
        var maxAnswers = state?.Settings.MaxAnswers ?? 0;
        AnswerStackPanel.Children.Clear();
        if (state == null) return;
        
        var rowWidth = ((ScrollViewer?) AnswerStackPanel.Parent)?.Bounds.Width ?? 440.0;
        var cellWidth = rowWidth / state.Settings.WordLength;
        if (cellWidth > 60.0)
        {
            cellWidth = 60.0;
        }
        var cellHeight = cellWidth * 1.2;
        
        for (var i = 0; i < maxAnswers; i++)
        {
            var currentAnswer = state.Answers.Count > i ? state.Answers[i] : "";
            var af = new AnswerField
            {
                CurrentAnswer = currentAnswer,
                CorrectAnswer = i < state.CurrentTry ? state.CorrectAnswer : "",
                MaxLength = state.Settings.WordLength,
                Width = cellWidth * state.Settings.WordLength,
                Height = cellHeight,
            };

            AnswerStackPanel.Children.Add(af);
        }
    }

    private void RebuildKeyboard()
    {
        if (DataContext is not GameViewModel model) return;
        
        KeyboardStackPanel.Children.Clear();
        var rowWidth = ((Grid?) KeyboardStackPanel.Parent)?.Bounds.Width ?? 440.0;
        
        var rows = LocaleStorage.CurrentLocale!.Keyboard.lower;
        var keySpacing = 8.0;
        var keyWidth = rowWidth / rows[0].Length - (6.0 + keySpacing);
        switch (keyWidth)
        {
            case > 50.0:
                keySpacing = 9.0;
                keyWidth = 50.0;
                break;
            case < 26.0:
                keySpacing -= 4.0;
                keyWidth += 4.0;
                break;
        }
        var keyHeight = keyWidth * 1.15;
        var fontSize = keyHeight * 0.5;

        KeyboardStackPanel.Spacing = keySpacing;
        
        foreach (var row in rows)
        {
            var currentRow = new StackPanel();
            currentRow.Orientation = Orientation.Horizontal;
            currentRow.Spacing = keySpacing;

            if (KeyboardStackPanel.Children.Count != 2)
            {
                currentRow.Margin = new Thickness((keyWidth / 2.0) * KeyboardStackPanel.Children.Count, 0, 0, 0);
            }
            else
            {
                currentRow.Margin = new Thickness((keyWidth / 2.0) * KeyboardStackPanel.Children.Count - keyWidth, 0, 0, 0);
            }
            
            if (KeyboardStackPanel.Children.Count == 2)
            {
                var eraseButton = new Button
                {
                    Content = "⌫",
                    Command = ReactiveCommand.Create(model.RemoveLetter),
                    Padding = new Thickness(),
                    Width = keyWidth * 2.0,
                    Height = keyHeight,
                        FontSize = fontSize,
                    IsEnabled = model.Status == GameStatus.Play
                };
                eraseButton.Classes.Add("keyboard");
                eraseButton.Classes.Add("erase");
                currentRow.Children.Add(eraseButton);
            }
            
            foreach (var letter in row)
            {
                var button = new Button
                {
                    Content = letter.ToString(),
                    Command = ReactiveCommand.Create(() =>
                    {
                        model.EnterLetter(letter);
                    }),
                    Padding = new Thickness(),
                    Width = keyWidth,
                    Height = keyHeight,
                    FontSize = fontSize,
                };
                button.Classes.Add("keyboard");
                currentRow.Children.Add(button);
            }
            
            if (KeyboardStackPanel.Children.Count == 2)
            {
                var confirmButton = new Button
                {
                    Content = model.Status == GameStatus.Play ? "↩" : "⟳",
                    Command = ReactiveCommand.Create(model.ConfirmAnswer),
                    Padding = new Thickness(),
                    Width = keyWidth * 2.0,
                    Height = keyHeight,
                    FontSize = fontSize,
                };
                confirmButton.Classes.Add("keyboard");
                confirmButton.Classes.Add("confirm");
                currentRow.Children.Add(confirmButton);
            }

            KeyboardStackPanel.Children.Add(currentRow);
        }
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        var content = (DataContext as MainViewModel)?.Content;
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
            case GameStatus.Play:
                break;
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

    protected override void OnSizeChanged(SizeChangedEventArgs e)
    {
        base.OnSizeChanged(e);
        RebuildLetterGrid();
        RebuildKeyboard();
    }

    protected override void OnLoaded(RoutedEventArgs args)
    {
        base.OnLoaded(args);

        var ctx = (DataContext as GameViewModel);
        ctx?.Changed.Subscribe(delegate(IReactivePropertyChangedEventArgs<IReactiveObject> propertyChanged)
        {
            switch (propertyChanged.PropertyName)
            {
                case "State":
                case "CurrentAnswer":
                case "CurrentTry":
                    RebuildLetterGrid();
                    MessageStackPanel.Children.Clear();
                    break;
                case "Status":
                    RebuildLetterGrid();
                    ShowStatus(ctx);
                    RebuildKeyboard();
                    break;
            }
        });
        
        if (ctx != null)
        {
            RebuildLetterGrid();
            RebuildKeyboard();
        }
    }

    private void NewGame_OnPointerReleased(object? sender, RoutedEventArgs routedEventArgs)
    {
        var model = DataContext as GameViewModel;
        model?.StartNewGame();
    }

    private void Settings_OnPointerReleased(object? sender, RoutedEventArgs routedEventArgs)
    {
        var model = Parent?.DataContext as MainViewModel;
        model?.ToggleSettings();
    }

    private void Leaderboard_OnPointerReleased(object? sender, RoutedEventArgs routedEventArgs)
    {
        var model = Parent?.DataContext as MainViewModel;
        model?.ToggleLeaderboard();
    }
}