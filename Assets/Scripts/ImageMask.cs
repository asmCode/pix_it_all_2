using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageMask
{
    public static string Encode(bool[] tiles)
    {
        if (tiles == null)
            return null;

        var bitArray = new BitArray(tiles);
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
