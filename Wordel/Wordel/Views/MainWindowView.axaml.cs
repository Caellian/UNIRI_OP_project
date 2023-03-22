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
        var content = (DataContext as MainWindowViewModel)?.Content;
        if (content is not GameViewModel model) return;
        
        switch (e.Key)
        {
            case Key.Enter:
                model.ConfirmAnswer();
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