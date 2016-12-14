using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImageDataLoader
{
    public static BundleData[] LoadBundles()
    {
        var bundles = new List<BundleData>();

        var bundlePathes = GetBundlePathes();

        foreach (var bundlePath in bundlePathes)
        {
            var bundle = LoadBundle(bundlePath);
            if (bundle == null)
            {
                Debug.LogErrorFormat("Couldn't load bundle from path: {0}", bundlePath);
                continue;
            }

            bundles.Add(bundle);
        }

        return bundles.ToArray();
    }

    private static BundleData LoadBundle(string path)
    {
        if (!System.IO.Directory.Exists(path))
            return null;

        var fileNames = System.IO.Directory.GetFiles(path, "*.png");
        if (fileNames == null)
            return null;

        List<ImageData> images = new List<ImageData>();

        foreach (var fileName in fileNames)
        {
            var imageData = LoadImageData(fileName);
            if (imageData == null)
            {
                Debug.LogErrorFormat("Couldn't load image from path: {0}", fileName);
                continue;
            }

            images.Add(imageData);
        }

        var bundleId = System.IO.Path.GetFileNameWithoutExtension(path);

        return new BundleData(bundleId, "tmp name", images.ToArray());
    }

    private static ImageData LoadImageData(string path)
    {
        if (!System.IO.File.Exists(path))
            return null;

        byte[] textureData = System.IO.File.ReadAllBytes(path);
        if (textureData == null)
            return null;

        var texture = new Texture2D(2, 2);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        if (!texture.LoadImage(textureData))
            return null;

        var id = System.IO.Path.GetFileNameWithoutExtension(path);

        var imageData = new ImageData();
        imageData.Init(id, texture);

        return imageData;
    }

    private static List<string> GetBundlePathes()
    {
        var pathes = new List<string>();

        var path = Application.persistentDataPath + "/Bundles";
        pathes.InsertRange(0, System.IO.Directory.GetDirectories(path));

        path = Application.streamingAssetsPath + "/Bundles";
        pathes.InsertRange(0, System.IO.Directory.GetDirectories(path));

        return pathes;
    }
}
