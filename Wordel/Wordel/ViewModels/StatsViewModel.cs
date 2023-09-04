using Wordel.Data;
using Wordel.Util;

namespace Wordel.ViewModels;

public class StatsViewModel: ViewModelBase
{
    private PlayStats? _stats;

    public PlayStats Stats
    {
        get
        {
            _stats ??= Database.GetInstance().GetStats();
            return (PlayStats) _stats;
        }
    }

    public int Wins => Stats.Wins;
    public int Loses => Stats.Loses;
    public int Plays => Stats.Plays;
    public string LongestGuess => Stats.LongestGuess + " " + LocaleStorage.GetTranslation("Letters");
    
    public string QuickestVictory
    {
        get
        {
            if (Stats.QuickestVictory == 1)
            {
                return Stats.QuickestVictory + " " + LocaleStorage.GetTranslation("Try");
            } else {
                return Stats.QuickestVictory + " " + LocaleStorage.GetTranslation("Tries");
            }
        }
    }
    
    public string ScreenTitle => LocaleStorage.GetTranslation("ScreenStats");
    public string StatWins => LocaleStorage.GetTranslation("StatWins") + ":";
    public string StatLoses => LocaleStorage.GetTranslation("StatLoses") + ":";
    public string StatTotal => LocaleStorage.GetTranslation("StatTotal") + ":";
    public string StatLongest => LocaleStorage.GetTranslation("StatLongest") + ":";
    public string StatQuickest => LocaleStorage.GetTranslation("StatQuickest") + ":";
}