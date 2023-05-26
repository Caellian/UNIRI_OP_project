using System;
using Avalonia.Controls;
using Avalonia.Input;
using Wordel.ViewModels;

namespace Wordel.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        var content = (DataContext as MainWindowViewModel)?.Content;
        if (content is not GameViewModel model) return;
        
        // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
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
}