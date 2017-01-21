using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageListView : MonoBehaviour
{
    public ImageView m_imageViewPrefab;
    public RectTransform m_imagesContainer;

    public event System.Action<string> ImageClicked;

    public void SetImages(List<ImageViewData> images)
    {
        Clear();

        if (images == null)
            return;

        foreach (var imageData in images)
        {
            var imageView = Instantiate(m_imageViewPrefab, m_imagesContainer);
            imageView.transform.localScale = Vector3.one;
            imageView.SetImage(imageData);
            imageView.Clicked += HandleBundleViewClicked;
        }
    }

    public void Clear()
    {
        foreach (Transform child in m_imagesContainer)
            Destroy(child.gameObject);
    }

    private void HandleBundleViewClicked(string bundleId)
    {
        if (ImageClicked != null)
            ImageClicked(bundleId);
    }
}
