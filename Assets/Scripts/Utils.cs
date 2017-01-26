using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static string TimeToString(float seconds)
    {
        var timeSpan = System.TimeSpan.FromSeconds(seconds);
        return string.Format("{0}:{1}:{2}", timeSpan.Minutes + timeSpan.Hours * 60, timeSpan.Seconds, timeSpan.Milliseconds);
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
}
