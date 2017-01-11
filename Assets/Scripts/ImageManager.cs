using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImageManager
{
    public event System.Action BundlesChanged;

    private RemoteBundlesController m_remoteBundlesController = new RemoteBundlesController();

    public BundleData[] Bundles
    {
        get;
        private set;
    }

    /// <summary>
    /// Call this method after LoadImages
    /// </summary>
    public void RefreshBundles()
    {
        m_remoteBundlesController.DownloadMissingBundles(Bundles, DownloadMissingBundlesCallback);
    }

    public bool LoadImages()
    {
        Bundles = ImageDataLoader.LoadBundles();
 
        return true;
    }

    public BundleData GetBundleById(string bundleId)
    {
        return System.Array.Find(Bundles, t => { return t.Id == bundleId; });
    }

    public ImageData GetImageById(string bundleId, string imageId)
    {
        var bundle = GetBundleById(bundleId);
        if (bundle == null)
            return null;

        return bundle.GetImageById(imageId);
    }

    private void DownloadMissingBundlesCallback(DownloadMissingBundlesEventData eventData)
    {
        Debug.LogFormat("Downloading missing bundles finished. New bundles: {0}", eventData.HasNewBundles);

        LoadImages();

        if (BundlesChanged != null)
            BundlesChanged();
    }
}
