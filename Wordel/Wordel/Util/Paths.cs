using System;
using System.IO;

namespace Wordel.Util;

public static class Paths
{
    public static string StorageRoot()
    {
        switch (SysUtil.Environment)
        {
            case RunEnv.Desktop:
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Wordel");
            case RunEnv.Android:
                return "/data/data/net.tinsvagelj.Wordel";
            default:
                throw new SystemException("StorageRoot doesn't exist for environment");
        }
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
        switch (SysUtil.Environment)
        {
            case RunEnv.Android:
                return Path.Combine(StorageRoot(), "databases", "db.sqlite");
            default:
                return Path.Combine(StorageRoot(), "db.sqlite");
        }
    }
}