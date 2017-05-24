using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSettings
{
	public static bool IsRestoreAvailable
	{
		get;
		private set;
	}

    public static string RateMeUrl
    {
        get;
        private set;
    }

    public static int RateMeWinsCount
    {
        get;
        private set;
    }

    public static int RateMeDaysCount
    {
        get;
        private set;
    }

    static GameSettings()
	{
#if UNITY_IPHONE
		IsRestoreAvailable = true;
#else
        IsRestoreAvailable = false;
#endif
        RateMeUrl = "http://google.com";

        RateMeWinsCount = 3;
        RateMeDaysCount = 1;
    }
}
