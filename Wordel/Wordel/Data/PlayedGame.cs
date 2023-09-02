using System;
using SQLite;

namespace Wordel.Model;

public class PlayedGame
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; } = 0;
    public string Word { get; set; }
    public int Attempts { get; set; }
    public bool Victory { get; set; }
}