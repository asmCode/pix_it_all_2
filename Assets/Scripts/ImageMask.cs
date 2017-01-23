using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageMask
{
    public static string Encode(Texture2D image)
    {
        if (image == null)
            return null;

        int size = image.width * image.height;

        var bitArray = new BitArray(size);

        for (int y = 0; y < image.height; y++)
        {
            for (int x = 0; x < image.width; x++)
            {
                bool isPixelSet = image.GetPixel(x, y).a != 0.0f;
                int index = y * image.width + x;

                bitArray.Set(index, isPixelSet);
            }
        }

        int bytes_count = bitArray.Count / 8 + Mathf.Min(1, bitArray.Count % 8);
        var data = new byte[bytes_count];
        bitArray.CopyTo(data, 0);

        return System.Convert.ToBase64String(data);
    }

    public static bool[] Decode(string data)
    {
        if (string.IsNullOrEmpty(data))
            return null;
        
        var rawData = System.Convert.FromBase64String(data);
        if (rawData == null)
            return null;

        var bitArray = new BitArray(rawData);
        var result = new bool[bitArray.Count];
        bitArray.CopyTo(result, 0);

        return result;
    }
}
