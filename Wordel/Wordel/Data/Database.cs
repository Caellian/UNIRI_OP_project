using System;
using System.IO;
using System.Linq;
using DynamicData.Kernel;
using SQLite;
using Wordel.Model;
using Wordel.Util;

namespace Wordel.Data;

public class Database
{
    private readonly SQLiteConnection _database;
    private static Database? _instance = null;

    public Database()
    {
        var path = Paths.DBPath();
        var parent_dir = Path.GetDirectoryName(path);
        if (parent_dir != null && !Directory.Exists(parent_dir))
        {
            Directory.CreateDirectory(parent_dir);
        }
        _database = new(path);
        _database.CreateTable<PlayedGame>();
    }

    public static Database GetInstance()
    {
        if (_instance != null) return _instance;
        _instance = new Database();

        return _instance;
    }

    public PlayStats GetStats()
    {
        var plays = _database.Table<PlayedGame>();
        var total = plays.Count();
        var wins = plays.Count(game => game.Victory);
        var loses = total - wins;

        var quickest = plays.OrderBy(game => game.Attempts).FirstOrOptional(game => game.Victory).Convert(it => it.Attempts).ValueOrDefault();

        var longestGuess = 0;
        try
        {
            var tableName = plays.Table.TableName!; 
            longestGuess = _database.Query<PlayedGame>($"SELECT * FROM {tableName} ORDER BY LENGTH(Word) DESC")?.First()?.Word
                .Length ?? 0;
        }
        catch (InvalidOperationException) {}
        
        return new PlayStats
        {
            Wins = wins,
            Loses = loses,
            Plays = total,
            LongestGuess = longestGuess,
            QuickestVictory = quickest
        };
    }

    public int SavePlay(PlayedGame game)
    {
        if (game.ID != 0)
        {
            return _database.Update(game);
        }
        
        return _database.Insert(game);
    }
}