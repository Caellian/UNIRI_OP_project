namespace Wordel.Model;

public record struct Settings
{
    private int _wordLength = 5;
    private int _maxAnswers = 5;

    public int WordLength { get => _wordLength; set => _wordLength = value; }
    public int MaxAnswers { get => _maxAnswers; set => _maxAnswers = value; }
    
    public Settings()
    {
        
    }
};
