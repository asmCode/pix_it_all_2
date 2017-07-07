using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSettings
{
    public static string TutorialBundleId
    {
        get;
        private set;
    }

    public static string TutorialImageId
    {
        get;
        private set;
    }

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

#if UNITY_IPHONE
        RateMeUrl = "itms-apps://itunes.apple.com/app/id508919690";
#elif UNITY_ANDROID
        RateMeUrl = "market://details?id=com.semiseriousgames.pixitallfree";
#else
        RateMeUrl = "http://semiseriousgames.com";
#endif

        RateMeWinsCount = 3;
        RateMeDaysCount = 1;

        TutorialBundleId = "test_bundle";
        TutorialImageId = "flower_";
    }
}
