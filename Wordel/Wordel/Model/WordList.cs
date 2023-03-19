using System;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Platform;
using Newtonsoft.Json;

namespace Wordel.Model.Game;

public static class WordList
{
    private static string[]? _words = null;

    public static string[] Get()
    {
        if (_words != null) return _words;
        
        // Ovdje bi išla ograda (engl. fence) za provjeru je li Get već pozvan
        // u slučaju višenitnog pristupa

        var assets = AvaloniaLocator.Current.GetService<IAssetLoader>()!;
        Stream stream = assets.Open(new Uri("avares://Wordel/Assets/dictionary.json"));

        string json;
        using (var reader = new StreamReader(stream))
        {
            json = reader.ReadToEnd();
        }
        _words = JsonConvert.DeserializeObject<string[]>(json);

        return _words!;
    }

    public static string[] GetSized(int size)
    {
        return Get().Where(s => s.Length == size).ToArray();
    }

    public static int Count()
    {
        return Get().Length;
    }

    public static int CountSized(int size)
    {
        return GetSized(size).Length;
    }
}