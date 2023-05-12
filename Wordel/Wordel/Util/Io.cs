using System.IO;
using Newtonsoft.Json;

namespace Wordel.Util;

public static class Io
{
    public static string ReadToString(Stream stream)
    {
        string result;
        using (var reader = new StreamReader(stream))
        {
            result = reader.ReadToEnd();
        }

        return result;
    }

    public static T? DeserializeFile<T>(string path)
    {
        Stream file = File.Open(path, FileMode.Open);
        string json = ReadToString(file);
        return JsonConvert.DeserializeObject<T>(json);
    }
}