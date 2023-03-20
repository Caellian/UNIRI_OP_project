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
        if (content is GameViewModel model)
        {
            if (e.Key == Key.Enter)
            {
                model.ConfirmAnswer();
            }
            else if (e.Key == Key.Back)
            {
                model.RemoveLetter();
            }
            else
            {
                var entered = e.Key.ToString();
                if (entered.Length == 1)
                {
                    model.EnterLetter(entered.ToLower()[0]);
                }
            }
        }
    }
}