namespace Wordel.Model;

public record struct Settings()
{
    public int WordLength { get; set; } = 5;
    public int MaxAnswers { get; set; } = 5;
};
