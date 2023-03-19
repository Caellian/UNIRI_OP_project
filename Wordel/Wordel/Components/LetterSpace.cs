using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Wordel.Components;

public class LetterSpace: Control
{
    public LetterSpace()
    {
        
    }

    public override void Render(DrawingContext context)
    {
        
        context.DrawRectangle(Brushes.Transparent, new Pen(Brushes.Gray), new Rect(new Point(), new Size(25, 40)), 5, 5);
        //context.DrawText(_text, new Point());
    }
}