using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Wordel.Model;

namespace Wordel.Components;

public partial class AnswerField : UserControl
{
    public static readonly DirectProperty<AnswerField, int> MaxLengthProperty =
        AvaloniaProperty.RegisterDirect<AnswerField, int>(
            nameof(MaxLength),
            o => o.MaxLength,
            (o, v) => o.MaxLength = v);

    private int _maxLength = 5;

    public int MaxLength
    {
        get => _maxLength;
        set => SetAndRaise(MaxLengthProperty, ref _maxLength, value);
    }

    public static readonly StyledProperty<double> CellWidthProperty =
        AvaloniaProperty.Register<AnswerField, double>(nameof(CellWidth), 32.0);

    public double CellWidth
    {
        get => GetValue(CellWidthProperty);
        set => SetValue(CellWidthProperty, value);
    }

    public static readonly StyledProperty<double> CellHeightProperty =
        AvaloniaProperty.Register<AnswerField, double>(nameof(CellHeight), 50);

    public double CellHeight
    {
        get => GetValue(CellHeightProperty);
        set => SetValue(CellHeightProperty, value);
    }

    public static readonly StyledProperty<double> CellSpacingProperty =
        AvaloniaProperty.Register<AnswerField, double>(nameof(CellSpacing),8.0);

    public double CellSpacing
    {
        get => GetValue(CellSpacingProperty);
        set => SetValue(CellSpacingProperty, value);
    }

    public static readonly StyledProperty<double> BorderThicknessProperty =
        AvaloniaProperty.Register<AnswerField, double>(nameof(BorderThickness), 2.0);

    public double BorderThickness
    {
        get => GetValue(BorderThicknessProperty);
        set => SetValue(BorderThicknessProperty, value);
    }

    public static readonly DirectProperty<AnswerField, string> CurrentAnswerProperty =
        AvaloniaProperty.RegisterDirect<AnswerField, string>(
            nameof(CurrentAnswer),
            o => o.CurrentAnswer,
            (o, v) => o.CurrentAnswer = v);

    private string _currentAnswer = "";

    public string CurrentAnswer
    {
        get => _currentAnswer;
        set => SetAndRaise(CurrentAnswerProperty, ref _currentAnswer, value);
    }

    public static readonly DirectProperty<AnswerField, string> CorrectAnswerProperty =
        AvaloniaProperty.RegisterDirect<AnswerField, string>(
            nameof(CorrectAnswer),
            o => o.CorrectAnswer,
            (o, v) => o.CorrectAnswer = v);

    private string _correctAnswer = "";

    public string CorrectAnswer
    {
        get => _correctAnswer;
        set => SetAndRaise(CorrectAnswerProperty, ref _correctAnswer, value);
    }

    public double ContentWidth => MaxLength * (CellWidth + BorderThickness) + (MaxLength - 1) * CellSpacing;
    public double ContentHeight => CellHeight + BorderThickness * 2.0;

    public AnswerField()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public ISolidColorBrush[] UseColors(LetterUse[] uses)
    {
        var result = new ISolidColorBrush[uses.Length];
        for (var i = 0; i < uses.Length; i++)
        {
            result[i] = uses[i] switch
            {
                LetterUse.Currect => Brushes.DarkGreen,
                LetterUse.Possible => Brushes.DarkBlue,
                LetterUse.Wrong => Brushes.DarkRed,
                _ => Brushes.Transparent,
            };
        }
        return result;
    }

    public override void Render(DrawingContext context)
    {
        var fill = UseColors(WordList.LetterUseArray(_correctAnswer, _currentAnswer, MaxLength));
        
        for (var i = 0; i < MaxLength; i++)
        {
            var pos = new Point(BorderThickness + i * (CellWidth + CellSpacing), BorderThickness);
            
            context.DrawRectangle(fill[i], new Pen(Brushes.Gray, BorderThickness),
                new Rect(pos, new Size(CellWidth, CellHeight)), 4, 4);

            if (i >= _currentAnswer.Length) continue;
            var answerLetter = _currentAnswer.ToCharArray().GetValue(i)!;

            var text = new FormattedText(answerLetter.ToString(), CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                Typeface.Default, 22,
                Brushes.WhiteSmoke);
            var textPos = new Point(pos.X + (CellWidth / 2.0) - (text.Width / 2.0),
                pos.Y + (CellHeight / 2.0) - (text.Height / 2.0));
            context.DrawText(text, textPos);
        }
    }
}