using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    private static readonly string TimeFormat = "{0:00}:{1:00}";
    private static readonly string TimeFormatWithH = "{0}:{1:00}:{2:00}";

    public static string TimeToString(float seconds)
    {
        var timeSpan = System.TimeSpan.FromSeconds(seconds);

        if (timeSpan.Hours > 0)
            return string.Format(TimeFormatWithH, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        else
            return string.Format(TimeFormat, timeSpan.Minutes, timeSpan.Seconds);
    }

    public static int SecondsToMs(float seconds)
    {
        var timeSpan = System.TimeSpan.FromSeconds(seconds);
        return (int)timeSpan.TotalMilliseconds;
    }

    public static float MsToSeconds(int ms)
    {
        var timeSpan = System.TimeSpan.FromMilliseconds(ms);
        return (float)timeSpan.TotalSeconds;
    }

    public static void DeleteDirectoryContent(string path)
    {
        if (string.IsNullOrEmpty(path))
            return;

        if (!System.IO.Directory.Exists(path))
            return;

        System.IO.Directory.Delete(path, true);
        System.IO.Directory.CreateDirectory(path);
    }
}
