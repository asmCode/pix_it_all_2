using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImageManager
{
    public event System.Action BundlesChanged;

    private RemoteBundlesController m_remoteBundlesController = new RemoteBundlesController();
    private PurchasedBundles m_purchasedBundles = new PurchasedBundles();

    public BundleData[] Bundles
    {
        get;
        private set;
    }

    public void Init()
    {
        m_purchasedBundles.Load();
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

    public void SetBundleAvailable(string bundleId)
    {
        m_purchasedBundles.SetPurchased(bundleId);
    }

    public bool IsBundleAvailable(string bundleId)
    {
        if (string.IsNullOrEmpty(bundleId))
            return false;

        var bundle = GetBundleById(bundleId);
        if (bundleId == null)
            return false;

        // Bundles with empty StoreId are considered as free and always available.
        if (string.IsNullOrEmpty(bundle.StoreId))
            return true;

        return m_purchasedBundles.IsPurchased(bundle.StoreId);
    }

    private void DownloadMissingBundlesCallback(DownloadMissingBundlesEventData eventData)
    {
        Debug.LogFormat("Downloading missing bundles finished. New bundles: {0}", eventData.HasNewBundles);

        if (eventData.HasNewBundles)
            LoadImages();

        if (BundlesChanged != null)
            BundlesChanged();
    }
}
