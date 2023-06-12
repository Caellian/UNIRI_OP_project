using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Logging;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Newtonsoft.Json;

namespace Wordel.Util;

public enum RunEnv
{
    Desktop,
    Android,
    iOS,
    Web,
}

// ReSharper disable once InconsistentNaming
public class Permissions
{
    public bool WebAccess;
}

public static class SysUtil
{
    private static RunEnv _env = RunEnv.Desktop;
    public static RunEnv Environment
    {
        get => _env;
        set
        {
            _env = value;
            if (_env == RunEnv.Desktop)
            {
                Permissions = new Permissions
                {
                    WebAccess = true
                };
            }
            else
            {
                Permissions = new Permissions();
            }
        }
    }

    public static Permissions Permissions = new();

    private static readonly Uri RepoUrl = new("https://raw.githubusercontent.com/Caellian/Wordel/main/Wordel/Wordel/");
    private static readonly Uri ResourceRoot = new("avares://Wordel/");

    public static async Task<string?> GetStringAsync(string path)
    {
        if (!Permissions.WebAccess) return null;
        
        string? result = null;
        try
        {
            Logger.TryGet(LogEventLevel.Information, "IO")?.Log(GetStringAsync, "Get string: {url}", RepoUrl + path);
            using var client = new HttpClient(new HttpClientHandler());
            var response = await client.GetAsync(RepoUrl + path).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                result = await response.Content.ReadAsStringAsync();
            }
            else
            {
                result = null;
            }
        }
        catch (HttpRequestException e)
        {
            Logger.TryGet(LogEventLevel.Error, "IO")?.Log(GetStringAsync, "Unable to get: {path}.\n{e}", path, e);
            result = null;
        }
        return result;
    }

    public static string? GetStringBlocking(string path)
    {
        return GetStringAsync(path).Result;
    }

    public static string? GetRemoteAssetString(string path)
    {
        // FIXME: Handle invalid references 
        var stream = AssetLoader.Open(new Uri(ResourceRoot + path));
        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
        {
            return reader.ReadToEnd();
        }
    }

    public static string? AccessAssetString(string path)
    {
        if (Environment == RunEnv.Desktop && File.Exists(path))
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch (FileNotFoundException e)
            {
                Logger.TryGet(LogEventLevel.Error, "IO")?.Log(WordList, "Unable to read local asset: {path}.\n{e}", path, e);
            }
        }

        var asset = GetRemoteAssetString(path);
        if (asset != null)
        {
            if (Environment == RunEnv.Desktop)
            {
                File.WriteAllText(path, asset);
            }
            
            return asset;
        }
        
        var remote = GetStringBlocking(path);
        if (remote == null) return null;

        if (Environment == RunEnv.Desktop)
        {
            Logger.TryGet(LogEventLevel.Information, "IO")?.Log(WordList, "Saving downloaded file: '{path}'", path);
            File.WriteAllText(path, remote);
        }
        return remote;
    }
    
    public static T? DeserializeFile<T>(string path)
    {
        var access_string = AccessAssetString(path) ?? "null";
        return JsonConvert.DeserializeObject<T?>(access_string);
    }

    public static string[]? WordList(string path)
    {
        return AccessAssetString(path)?.Split("\n");
    }
}