using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using Wordel.Components;
using Wordel.ViewModels;

namespace Wordel.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    protected override void OnLoaded()
    {
        var model = DataContext as GameViewModel;

        WordGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
        for (var i = 0; i < model.WordLength; i++)
        {
            WordGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
            WordGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
        }
        WordGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
        
        for (var r = 0; r < model.MaxAnswers; r++)
        {
            for (var c = 0; c < model.WordLength; c++)
            {
                var ls = new LetterSpace();
                ls.SetValue(Grid.RowProperty, r);
                ls.SetValue(Grid.ColumnProperty, c + 1);
                ls.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Center);
                ls.SetValue(VerticalAlignmentProperty, VerticalAlignment.Center);
                WordGrid.Children.Add(ls);
            }
        }
        
        WordGrid.Arrange(WordGrid.Bounds);
    }
    
    protected override void OnKeyUp(KeyEventArgs e)
    {
        base.OnKeyUp(e);
    }
}