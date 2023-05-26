using Wordel.Util;

namespace Wordel.Model;

public class Settings
{
    /// <summary>
    /// Dummy variable for setting screen generation.
    /// </summary>
    [Configurable("Language", new object[]{"en", "hr"})]
    public string Language = LocaleStorage.CurrentLocale?.Name ?? "en";
    
    [Configurable("WordLength", 5, 4, 10)]
    public int WordLength = 5;

    [Configurable("MaxAnswers", 5, 3, 10)]
    public int MaxAnswers = 5;
};
