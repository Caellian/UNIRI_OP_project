using System;
using System.IO;
using System.Net;
using System.Net.Http;
using Avalonia.Logging;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;

namespace Wordel.Util;

public enum RunEnv
{
    Desktop,
    Android,
    iOS,
    Web,
}

public static class Io
{
    public static RunEnv Environment = RunEnv.Desktop;

    private static Uri REPO_URL = new("https://github.com/Caellian/Wordel/blob/main/Wordel/Wordel");
    
    public static T? DeserializeFile<T>(string path)
    {
        if (Environment == RunEnv.Desktop)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            }
            catch (Exception e)
            {
                Logger.TryGet(LogEventLevel.Error, "IO::WordList")?.Log(DeserializeFile<T>, "Unable to read word list.\n{e}", e);
            }
        }
        
        Logger.TryGet(LogEventLevel.Information, "IO::WordList")?.Log(WordList, "Downloading resource: {path}", path);
        using var client = new HttpClient();
        var stream = client.GetStringAsync(REPO_URL + path);
        stream.Wait();
        
        if (Environment == RunEnv.Desktop)
        {
            Logger.TryGet(LogEventLevel.Information, "IO::WordList")?.Log(DeserializeFile<T>, "Saving downloaded file: '{path}'", path);
            File.WriteAllText(path, stream.Result);
        }
        return JsonConvert.DeserializeObject<T>(stream.Result);
    }

    public static string[] WordList(string path)
    {
        if (Environment == RunEnv.Desktop)
        {
            try
            {
                return File.ReadAllLines(path);
            }
            catch (Exception e)
            {
                Logger.TryGet(LogEventLevel.Error, "IO::WordList")?.Log(WordList, "Unable to read word list.\n{e}", e);
            }
        }
        
        Logger.TryGet(LogEventLevel.Information, "IO::WordList")?.Log(WordList, "Fetching word list: '{path}'", path);
        using var client = new HttpClient();
        var stream = client.GetStringAsync(REPO_URL + path);
        stream.Wait();
        if (Environment == RunEnv.Desktop)
        {
            Logger.TryGet(LogEventLevel.Information, "IO::WordList")?.Log(WordList, "Saving downloaded file: '{path}'", path);
            File.WriteAllText(path, stream.Result);
        }
        var result = stream.Result.Split("\n");
        return result;
    }
}