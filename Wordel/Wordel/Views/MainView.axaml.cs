using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using DynamicData;
using Wordel.Components;
using Wordel.ViewModels;

namespace Wordel.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
        base.OnKeyUp(e);
    }
}