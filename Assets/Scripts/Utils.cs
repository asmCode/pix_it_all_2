using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static string MsToString(int ms)
    {
        const int msInSecond = 1000;
        const int msInMinute = msInSecond * 60;

        int minutes = ms / msInMinute;
        int seconds = (ms % msInMinute) / msInSecond;
        int ms_ = ms % msInSecond;

        return string.Format("{0}:{1}:{2}", minutes, seconds, ms_);
    }
}
