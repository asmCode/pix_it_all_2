using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageListView : MonoBehaviour
{
    public ImageView m_imageViewPrefab;
    public RectTransform m_imagesContainer;
    public GameObject m_buyButton;
    public Text m_buyButtonLabel;

    public event System.Action<string> ImageClicked;
    public event System.Action BuyClicked;

    private List<ImageView> m_imageViews = new List<ImageView>();

    public void Init(List<ImageViewData> images, bool storeMode, string localizedPrice)
    {
        Clear();

        m_buyButton.SetActive(storeMode);
        m_buyButton.SetActive(!storeMode);
        m_buyButton.SetActive(storeMode);

        if (images == null)
            return;

        if (string.IsNullOrEmpty(localizedPrice))
            m_buyButtonLabel.text = "";
        else
            m_buyButtonLabel.text = localizedPrice;

        foreach (var imageData in images)
        {
            var imageView = Instantiate(m_imageViewPrefab, m_imagesContainer);
            imageView.transform.localScale = Vector3.one;
            imageView.SetImage(imageData);
            imageView.SetStoreMode(storeMode);
            imageView.Clicked += HandleBundleViewClicked;

            m_imageViews.Add(imageView);
        }
    }

    public ImageView GetImageView(string imageId)
    {
        int index = GetImageIndex(imageId);
        if (index == -1)
            return null;

        return m_imageViews[index];
    }

    public int GetImageIndex(string imageId)
    {
        for (int i = 0; i < m_imageViews.Count; i++)
        {
            var child = m_imageViews[i];
            var imageView = child.GetComponent<ImageView>();
            if (imageView == null)
                continue;

            if (imageView.ImageId == imageId)
                return i;
        }

        return -1;
    }

    public void ScrollToImage(string imageId)
    {
        int index = GetImageIndex(imageId);
        if (index == -1 || m_imagesContainer.childCount == 0)
            return;

        float normalizePosition = (float)index / (float)m_imagesContainer.childCount;
        GetComponent<ScrollRect>().verticalNormalizedPosition = 1 - normalizePosition;
    }

    public void Clear()
    {
        m_imageViews.Clear();

        foreach (Transform child in m_imagesContainer)
            Destroy(child.gameObject);
    }

    private void HandleBundleViewClicked(string bundleId)
    {
        if (ImageClicked != null)
            ImageClicked(bundleId);
    }

    public void UiEvent_BuyButtonClicked()
    {
        if (BuyClicked != null)
            BuyClicked();
    }
}
