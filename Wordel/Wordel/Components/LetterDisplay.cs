using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Wordel.Components;

public class LetterDisplay: Control
{
    public string Letter = "";
    private FormattedText _text => new(Letter, CultureInfo.CurrentCulture,
        FlowDirection.LeftToRight, Typeface.Default, 24.0, Brushes.Lime);
    
    public LetterDisplay()
    {
        
    }

    public override void Render(DrawingContext context)
    {
        context.DrawText(_text, new Point());
    }
}