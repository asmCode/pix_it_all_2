﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BundleData
{
    private List<ImageData> m_images;

    public string Id
    {
        get;
        private set;
    }

    public string ProductId
    {
        get;
        private set;
    }

    public string Name
    {
        get;
        private set;
    }

    public string Crc
    {
        get;
        private set;
    }

    public BundleData(string id, string productId, string name, string crc, ImageData[] images)
    {
        Id = id;
        ProductId = productId;
        Name = name;
        Crc = crc;
        m_images = new List<ImageData>(images);
    }

    public ImageData[] GetImages()
    {
        return m_images.ToArray();
    }

    public ImageData GetImageById(string imageId)
    {
        return m_images.Find(t => { return t.Id == imageId; });
    }
}
