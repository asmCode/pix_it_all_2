﻿using UnityEngine;
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

    public static void RemoveDownloadedBundles()
    {
        var pathes = new List<string>();
        GetDownloadedBundlePathes(pathes);

        foreach (var path in pathes)
            System.IO.File.Delete(path);
    }

    private static BundleData LoadBundle(string path)
    {
        var bundleData = FileReader.ReadAllText(path);
        if (bundleData == null)
            return null;

        var bundleFileData = JsonUtility.FromJson<BundleFileData>(bundleData);
        if (bundleFileData == null || bundleFileData.Images == null || bundleFileData.Images.Length == 0)
            return null;

        List<ImageData> images = new List<ImageData>();

        foreach (var imageFileData in bundleFileData.Images)
        {
            var imageData = LoadImageData(imageFileData);
            if (imageData == null)
            {
                Debug.LogErrorFormat("Couldn't load image: {0}", imageFileData.Id);
                continue;
            }

            images.Add(imageData);
        }

        var bundleId = System.IO.Path.GetFileNameWithoutExtension(path);

        var bundleDataRaw = System.Text.Encoding.UTF8.GetBytes(bundleData);
        var crc = DamienG.Security.Cryptography.Crc32.Compute(bundleDataRaw).ToString();

        return new BundleData(bundleId, bundleFileData.ProductId, bundleFileData.Name, crc, images.ToArray());
    }

    private static ImageData LoadImageData(ImageFileData imageFileData)
    {
        if (string.IsNullOrEmpty(imageFileData.Id) ||
            string.IsNullOrEmpty(imageFileData.Name) ||
            string.IsNullOrEmpty(imageFileData.RawImageData))
            return null;

        byte[] textureData = System.Convert.FromBase64String(imageFileData.RawImageData);
        if (textureData == null)
            return null;

        var texture = new Texture2D(2, 2);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        if (!texture.LoadImage(textureData))
            return null;

        int size = Mathf.NextPowerOfTwo(Mathf.Max(texture.width, texture.height));
        var texture2 = new Texture2D(size, size);
        texture2.filterMode = FilterMode.Point;
        texture2.wrapMode = TextureWrapMode.Clamp;

        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                texture2.SetPixel(x, y, texture.GetPixel(x, y));
            }
        }
        texture2.Apply(false);

        var imageData = new ImageData();
        imageData.Init(imageFileData.Id, imageFileData.Name, texture.width, texture.height, texture2);

        return imageData;
    }

    private static List<string> GetBundlePathes()
    {
        var pathes = new List<string>();

        GetBaseBundlePathes(pathes);
        GetDownloadedBundlePathes(pathes);

        return pathes;
    }

    private static void GetBaseBundlePathes(List<string> pathes)
    {
        var path = Application.streamingAssetsPath + "/Bundles";

        pathes.Add(path + "/appetizer.pixbundle");
        pathes.Add(path + "/good_start.pixbundle");
        pathes.Add(path + "/one_by_one.pixbundle");
        pathes.Add(path + "/pixel_lovers.pixbundle");
        pathes.Add(path + "/tiny_ones.pixbundle");
    }

    private static void GetDownloadedBundlePathes(List<string> pathes)
    {
        var path = Application.persistentDataPath + "/Bundles";
        if (System.IO.Directory.Exists(path))
            pathes.InsertRange(0, System.IO.Directory.GetFiles(path, "*.pixbundle"));
    }
}
