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

    public string Name
    {
        get;
        private set;
    }

    public Texture2D Texture
    {
        get;
        private set;
    }

    public int Width
    {
        get;
        private set;
    }

    public int Height
    {
        get;
        private set;
    }

    public Color[] Colors
    {
        get;
        private set;
    }

    public void Init(string id, string name, int width, int height,  Texture2D texture)
    {
        if (texture == null)
            return;

        Id = id;
        Name = name;
        Width = width;
        Height = height;
        Texture = texture;

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
        
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
                colorSet.Add(pixels[y * Texture.width + x]);
        }
        
        Colors = new Color[colorSet.Count];
        colorSet.CopyTo(Colors);
    }
}
