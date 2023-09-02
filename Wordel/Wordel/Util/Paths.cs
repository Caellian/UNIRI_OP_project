using System;
using System.IO;

namespace Wordel.Util;

public static class Paths
{
    public static string StorageRoot()
    {
        var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        if (SysUtil.Environment == RunEnv.Desktop)
        {
            path = Path.Combine(path, "Wordel");
        }
        return path;
    }

    public static string AssetPath(string file)
    {
        if (SysUtil.Environment == RunEnv.Desktop)
        {
            return file;
        }
        return Path.Combine(StorageRoot(), file);
    }

    public static string DBPath()
    {
        return Path.Combine(StorageRoot(), "db.sqlite");
    }
}