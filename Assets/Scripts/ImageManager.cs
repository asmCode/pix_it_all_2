using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImageManager
{
    private List<ImageData> m_images = new List<ImageData>();

    public bool LoadImages()
    {
        m_images.Clear();

        var dataPath = Application.persistentDataPath + "/Bundles/extra/";
 
        var downloadedImages = ImageDataLoader.LoadAll(dataPath);
        if (downloadedImages != null)
            m_images.AddRange(downloadedImages);

        var dataImages = ImageDataLoader.LoadFromResources();
        if (dataImages != null)
            m_images.AddRange(dataImages);

        return true;
    }

    public ImageData GetImageById(string id)
    {
        return m_images.Find(t => { return t.Id == id; });
    }
}
