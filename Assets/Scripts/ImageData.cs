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

    public void Init(string id, Texture2D texture)
    {
        if (texture == null)
            return;

        Texture = texture;
        Id = id;

        CountColors();
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
