using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Platform;
using Newtonsoft.Json;

namespace Wordel.Model;

public static class WordList
{
    private static List<string>? _words;
    private static readonly Random _random = new();

    public static IEnumerable<string> Get()
    {
        if (_words != null) return _words;
        
        // Ovdje bi išla ograda (engl. fence) za provjeru je li Get već pozvan
        // u slučaju višenitnog pristupa

        Stream stream = File.Open("Assets/words", FileMode.Open);
        
        _words = new List<string>(1024);
        using (var reader = new StreamReader(stream))
        {
            while (reader.ReadLine() is { } word)
            {
                _words.Add(word);
            }
        }
        
        return _words;
    }

    public static IEnumerable<string> GetSized(int size)
    {
        return Get().Where(s => s.Length == size);
    }

    public static string GetRandomSized(int size)
    {
        var count = CountSized(size);
        return GetSized(size).Skip(_random.Next(0, count - 1)).First();
    }

    public static int Count()
    {
        return _words.Count;
    }

    public static int CountSized(int size)
    {
        return GetSized(size).Count();
    }

    public static bool TestWord(string word)
    {
        return Get().Any(it => it == word);
    }

    public static LetterUse[] LetterUseArray(string target, string current, int maxLength)
    {
        var result = new LetterUse[maxLength];
        for (var i = 0; i < maxLength; i++)
        {
            result[i] = LetterUse.Unknown;
        }
        if (target == "")
        {
            return result;
        }
        
        var remLet = new Dictionary<char, int>(target.Length);
        foreach (var letter in target.ToCharArray())
        {
            if (remLet.ContainsKey(letter))
            {
                remLet[letter] += 1;
            }
            else
            {
                remLet[letter] = 1;
            }
        }

        for (var i = 0; i < maxLength; i++)
        {
            var letter = current[i];
            
            if (target[i] == current[i])
            {
                remLet[letter] -= 1;
                result[i] = LetterUse.Currect;
                continue;
            }

            if (remLet.GetValueOrDefault(letter) > 0)
            {
                remLet[letter] -= 1;
                result[i] = LetterUse.Possible;
                continue;
            }

            result[i] = LetterUse.Wrong;
        }

        return result;
    }
}