using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownloadMissingBundlesEventData
{
    public bool HasNewBundles
    {
        get;
        private set;
    }

    public bool Success
    {
        get;
        private set;
    }

    public DownloadMissingBundlesEventData(bool hasNewBundles, bool success)
    {
        HasNewBundles = hasNewBundles;
        Success = success;
    }
}
