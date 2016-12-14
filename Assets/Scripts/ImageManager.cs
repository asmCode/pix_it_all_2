using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImageManager
{
    public BundleData[] Bundles
    {
        get;
        private set;
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
}
