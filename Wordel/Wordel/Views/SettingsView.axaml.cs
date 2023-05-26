using System.Globalization;
using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using DynamicData;
using ReactiveUI;
using Wordel.Model;
using Wordel.Util;
using Wordel.ViewModels;

namespace Wordel.Views;

public partial class SettingsView : UserControl
{
    public SettingsView()
    {
        InitializeComponent();
    }

    /*
        public T KeepElementsData()
        {
            var item = new T();
            foreach (var Property in item.GetType().GetProperties())
            {
                item.GetType().GetProperty(Property.Name).SetValue(item, "TestData");  //this works
            }
        }
     */
    
    private void BuildOptions()
    {
        var model = (DataContext as SettingsViewModel)!;
        OptionsGrid.Children.Clear();
        OptionsGrid.ColumnDefinitions = new ColumnDefinitions("*,Auto");
        
        var rowDefs = new RowDefinitions();
        var row = 0;

        var fields = model.SettingFields;
        foreach (var (field, conf) in fields)
        {
            var label = new TextPresenter
            {
                Margin = new Thickness(10, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Left,
                FontSize = 18,
                Text = LocaleStorage.GetTranslation(field.Name + "SettingsOption")
            };
            label.SetValue(Grid.ColumnProperty, 0);
            label.SetValue(Grid.RowProperty, row);
            OptionsGrid.Children.Add(label);

            Control? control = null;
            switch (conf.Kind)
            {
                case ConfigKind.Integer:
                {
                    var value = new NumericUpDown
                    {
                        Margin = new Thickness(0, 0, 10, 0),
                        Width = 150,
                        Increment = conf.Increment ?? new decimal(1.0),
                        HorizontalContentAlignment = HorizontalAlignment.Right,
                        Value = new decimal((int?) field.GetValue(model.Settings) ?? (int?) conf.Default ?? 0.0)
                    };
                    if (conf.Limits.HasValue)
                    {
                        value.Minimum = new decimal((int) conf.Limits.Value.Item1);
                        value.Maximum = new decimal((int) conf.Limits.Value.Item2);
                    }
                    value.AddHandler(NumericUpDown.ValueChangedEvent, (sender, args) =>
                    {
                        if (!args.NewValue.HasValue) return;
                        field.SetValue(model.Settings, (int) args.NewValue);
                    });
                    
                    control = value;
                    break;
                }
                case ConfigKind.Float:
                {
                    var value = new NumericUpDown
                    {
                        Margin = new Thickness(0, 0, 10, 0),
                        Width = 150,
                        Increment = conf.Increment ?? new decimal(1.0),
                        HorizontalContentAlignment = HorizontalAlignment.Right,
                        Value = new decimal((float?) field.GetValue(model.Settings) ?? (float?) conf.Default ?? 0.0)
                    };
                    if (conf.Limits.HasValue)
                    {
                        value.Minimum = new decimal((float)conf.Limits.Value.Item1);
                        value.Maximum = new decimal((float)conf.Limits.Value.Item2);
                    }
                    value.AddHandler(NumericUpDown.ValueChangedEvent, (sender, args) =>
                    {
                        if (!args.NewValue.HasValue) return;
                        field.SetValue(model.Settings, (float) args.NewValue);
                    });
                    
                    control = value;
                    break;
                }
                case ConfigKind.String:
                {
                    var value = new TextBox
                    {
                        Margin = new Thickness(0, 0, 10, 0),
                        Width = 200,
                        HorizontalContentAlignment = HorizontalAlignment.Right
                    };
                    if (conf.Default != null)
                    {
                        value.Text = (string?) conf.Default;
                    }
                    value.AddHandler(TextBox.TextChangedEvent, (sender, args) =>
                    {
                        if (value.Text == null) return;
                        field.SetValue(model.Settings, value.Text);
                    });
                    
                    control = value;
                    break;
                }
                case ConfigKind.Option:
                {
                    var value = new ComboBox
                    {
                        HorizontalContentAlignment = HorizontalAlignment.Right
                    };
                    var choices = field.Name == "Language" ? LocaleStorage.SupportedCultures : conf.Choices;
                    foreach (var confChoice in choices)
                    {
                        var item = new ComboBoxItem
                        {
                            Content = confChoice
                        };
                        value.Items.Add(item);
                    }
                    if (field.Name == "Language")
                    {
                        value.SelectedIndex = choices.IndexOf(LocaleStorage.CurrentCulture);
                        value.AddHandler(SelectingItemsControl.SelectionChangedEvent, (sender, args) =>
                        {
                            if (value.SelectedItem == null) return;
                            var cbi = (ComboBoxItem) value.SelectedItem;
                            LocaleStorage.CurrentCulture = (CultureInfo?) cbi.Content;
                            field.SetValue(model.Settings, LocaleStorage.CurrentCulture?.Name);
                            BuildOptions();
                        });
                    }
                    else
                    {
                        value.SelectedIndex = choices.IndexOf(conf.Default);
                        value.AddHandler(SelectingItemsControl.SelectionChangedEvent, (sender, args) =>
                        {
                            if (value.SelectedItem == null) return;
                            var cbi = (ComboBoxItem) value.SelectedItem;
                            field.SetValue(model.Settings, cbi.Content);
                        });
                    }

                    control = value;
                    break;
                }
            }
            if (control == null) continue;
            
            control.SetValue(Grid.ColumnProperty, 1);
            control.SetValue(Grid.RowProperty, row);
            OptionsGrid.Children.Add(control);
            rowDefs.Add(new RowDefinition(GridLength.Auto));
            row += 1;
        }
        
        OptionsGrid.RowDefinitions = rowDefs;
    }

    protected override void OnLoaded()
    {
        base.OnLoaded();
        BuildOptions();
    }

    private void Close_OnPointerReleased(object? sender, RoutedEventArgs routedEventArgs)
    {
        var model = Parent?.DataContext as MainWindowViewModel;
        model?.ToggleSettings();
    }
}