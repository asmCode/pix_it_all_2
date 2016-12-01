using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImageDataLoader
{
    public static ImageData[] LoadAll(string path)
    {
        Debug.LogFormat("Loading images from path: {0}", path);

        if (!System.IO.Directory.Exists(path))
            return null;

        var fileNames = System.IO.Directory.GetFiles(path, "*.png");
        if (fileNames == null)
            return null;

        List<ImageData> images = new List<ImageData>();

        foreach (var fileName in fileNames)
        {
            var id = System.IO.Path.GetFileNameWithoutExtension(fileName);

            Debug.LogFormat("Loading image {0}", id);

            var imageData = LoadImageData(fileName);
            if (imageData != null)
                images.Add(imageData);
        }

        return images.ToArray();
    }

    public static ImageData[] LoadFromResources()
    {
        var textures = Resources.LoadAll<Texture2D>("Bundles/base/");
        if (textures == null)
            return null;

        List<ImageData> images = new List<ImageData>();

        foreach (var texture in textures)
        {
            var imageData = new ImageData();
            imageData.Init(texture.name, texture);
            images.Add(imageData);
        }

        return images.ToArray();
    }

    public static ImageData LoadImageData(string path)
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
}
