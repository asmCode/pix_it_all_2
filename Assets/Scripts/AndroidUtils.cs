#if UNITY_ANDROID

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidUtils
{
    private static float m_dpi = 0.0f;

    public static float Dpi()
    {
        if (m_dpi == 0)
        {
            AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = activityClass.GetStatic<AndroidJavaObject>("currentActivity");

            AndroidJavaObject metrics = new AndroidJavaObject("android.util.DisplayMetrics");
            activity.Call<AndroidJavaObject>("getWindowManager").Call<AndroidJavaObject>("getDefaultDisplay").Call("getMetrics", metrics);

            m_dpi = (metrics.Get<float>("xdpi") + metrics.Get<float>("ydpi")) * 0.5f;
        }

        return m_dpi;
    }
}

#endif