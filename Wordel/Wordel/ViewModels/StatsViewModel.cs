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


    public int Wins {
        get => Stats.Wins;
        
    }
    public int Loses {
        get => Stats.Loses;
        
    }
    public int Plays {
        get => Stats.Plays;
        
    }
    public string LongestGuess {
        get => Stats.LongestGuess + " " + LocaleStorage.GetTranslation("Letters");
        
    }
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
    
    public string ScreenTitle
    {
        get => LocaleStorage.GetTranslation("ScreenStats");
    }
    
    public string StatWins {
        get => LocaleStorage.GetTranslation("StatWins") + ":";
    }
    public string StatLoses {
        get => LocaleStorage.GetTranslation("StatLoses") + ":";
    }
    public string StatTotal {
        get => LocaleStorage.GetTranslation("StatTotal") + ":";
    }
    public string StatLongest {
        get => LocaleStorage.GetTranslation("StatLongest") + ":";
    }
    public string StatQuickest {
        get => LocaleStorage.GetTranslation("StatQuickest") + ":";
    }
}