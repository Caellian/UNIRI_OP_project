using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace Wordel.Components;

public class IconButton : TemplatedControl
{
    public static readonly StyledProperty<IImage?> IconProperty =
        AvaloniaProperty.Register<IconButton, IImage?>(nameof(Icon));

    public IImage? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }
    
    public event EventHandler<RoutedEventArgs>? Click
    {
        add => AddHandler(Button.ClickEvent, value);
        remove => RemoveHandler(Button.ClickEvent, value);
    }
}