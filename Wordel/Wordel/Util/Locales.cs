using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Converters;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Media;
using Avalonia.Styling;
using Newtonsoft.Json;
using Wordel.Model;
using Wordel.ViewModels;

namespace Wordel.Util;

public class Keyboard
{
    public string[] upper = {};
    public string[] lower = {};

    private HashSet<char> _all_characters = new();

    public Keyboard(CultureInfo cultureInfo)
    {
        var loaded =
            Io.DeserializeFile<Dictionary<string, string[]>>($"Assets/i18n/keyboard.{cultureInfo.Name}.json");
        if (loaded == null) return;
        
        upper = loaded["upper"];
        lower = loaded["lower"];

        Chars();
    }

    public HashSet<char> Chars()
    {
        if (_all_characters.Count > 0) return _all_characters;
        
        foreach (var line in upper)
        {
            var letters = line.ToCharArray();
            foreach (var l in letters)
            {
                _all_characters.Add(l);
            }
        }
        foreach (var line in lower)
        {
            var letters = line.ToCharArray();
            foreach (var l in letters)
            {
                _all_characters.Add(l);
            }
        }

        return _all_characters;
    }

    public bool CanWrite(string check)
    {
        return check.ToCharArray().All(letter => Chars().Contains(letter));
    }
}

public class WordList
{
    public string[] Words;

    public WordList(CultureInfo cultureInfo)
    {
        Words = Io.WordList($"Assets/i18n/dict.{cultureInfo.Name}.txt");
    }

    public string[] GetSized(int size)
    {
        return Words.Where(s => s.Length == size).ToArray();
    }

    public int Count()
    {
        return Words.Length;
    }

    public int CountSized(int size)
    {
        return GetSized(size).Count();
    }

    public bool TestWord(string word)
    {
        return Words.Any(it => it == word);
    }
}

public class Locale
{
    private readonly CultureInfo _cultureInfo;
    public readonly Dictionary<string, string> Strings;
    public readonly Keyboard Keyboard;
    public readonly WordList WordList;

    public Locale(CultureInfo cultureInfo)
    {
        _cultureInfo = cultureInfo;
        {
            var loaded =
                Io.DeserializeFile<Dictionary<string, string>>($"Assets/i18n/interface.{cultureInfo.Name}.json");
            if (loaded != null)
            {
                Strings = loaded;
            }
        }
        Keyboard = new Keyboard(cultureInfo);
        WordList = new WordList(cultureInfo);
    }

    public string Name => _cultureInfo.Name;
}

public static class LocaleStorage
{
    private static CultureInfo[]? _supportedCultures;
    private static Dictionary<CultureInfo, Locale> _locales = new ();
    public static CultureInfo? CurrentCulture = new ("en");

    public static CultureInfo[] SupportedCultures
    {
        get
        {
            if (_supportedCultures != null) return _supportedCultures;

            return _supportedCultures = Io.DeserializeFile<string[]>("Assets/i18n/lang.json")?.Select(it => new CultureInfo(it)).ToArray()!;
        }
    }

    public static Dictionary<CultureInfo, Locale> Locales
    {
        get
        {
            if (SupportedCultures.Length == _locales.Count) return _locales;

            foreach (var culture in SupportedCultures)
            {
                var locale = new Locale(culture);
                _locales.Add(culture, locale);
            }

            return _locales;
        }
    }

    public static Locale? CurrentLocale => CurrentCulture == null ? null : Locales[CurrentCulture];

    public static string GetTranslation(string key)
    {
        return CurrentLocale?.Strings.ContainsKey(key) == true ? CurrentLocale.Strings[key] : key;
    }
    
    public static string Format(string formatted)
    {
        if (formatted.Length <= 4)
        {
            return formatted;
        }

        var remaining = formatted;
        var built = "";

        while (remaining.Length > 0)
        {
            var startIndex = remaining.IndexOf("{{", StringComparison.Ordinal);
            if (startIndex > 0 && remaining[startIndex - 1] == '\\')
            {
                built += remaining[..(startIndex + 2)];
                remaining = remaining[(startIndex + 2)..];
                continue;
            }
            if (startIndex == -1)
            {
                built += remaining;
                break;
            }
            if (startIndex > 0)
            {
                built += remaining[..startIndex];
            }

            var endIndex = remaining[(startIndex + 2)..].IndexOf("}}", StringComparison.Ordinal);
            if (endIndex == -1)
            {
                throw new ArgumentException($"unterminated localized string in '{formatted}' starting at {built.Length + startIndex}");
            }
            if (endIndex == 0)
            {
                throw new ArgumentException($"empty localized string in '{formatted}' starting at {built.Length + startIndex}");
            }
            
            var key = remaining[(startIndex + 2)..(startIndex + 2 + endIndex)];
            var value = GetTranslation(key);
            
            remaining = remaining[(startIndex + endIndex + 4)..];
            built += value;
        }

        return built;
    }
}