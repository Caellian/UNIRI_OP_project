using System.Globalization;
using Avalonia;
using Avalonia.Media;

namespace Wordel.Components;

public class FilledLetterSpace: LetterSpace
{
    public char Letter = ' ';
    
    private FormattedText _text => new(Letter.ToString(), CultureInfo.CurrentCulture,
        FlowDirection.LeftToRight, Typeface.Default, 24.0, Brushes.WhiteSmoke);

    public override void Render(DrawingContext context)
    {
        base.Render(context);
        context.DrawText(_text, new Point(5, 10));
    }
}