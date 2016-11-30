using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImageData
{
    public string Id
    {
        get;
        private set;
    }

    public Texture2D Texture
    {
        get;
        private set;
    }

    public Color[] Colors
    {
        get;
        private set;
    }

    public static ImageData Load(string id)
    {
        string textureFileName = id + ".png";
        string path = Application.persistentDataPath + "/" + textureFileName;

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

        var imageData = new ImageData();
        imageData.Texture = texture;

        imageData.CountColors();

        return imageData;
    }

    private void CountColors()
    {
        if (Texture == null)
            return;

        var pixels = Texture.GetPixels32();
        if (pixels == null)
            return;

        var colorSet = new HashSet<Color>();
        
        for (int i = 0; i < pixels.Length; i++)
            colorSet.Add(pixels[i]);

        Colors = new Color[colorSet.Count];
        colorSet.CopyTo(Colors);
    }
}
