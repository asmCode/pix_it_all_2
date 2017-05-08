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

    public void Init(List<ImageViewData> images, bool storeMode, string localizedPrice)
    {
        Clear();

        m_buyButton.SetActive(storeMode); 

        if (images == null)
            return;

        if (string.IsNullOrEmpty(localizedPrice))
            m_buyButtonLabel.text = "BUY";
        else
            m_buyButtonLabel.text = "BUY " + localizedPrice;

        foreach (var imageData in images)
        {
            var imageView = Instantiate(m_imageViewPrefab, m_imagesContainer);
            imageView.transform.localScale = Vector3.one;
            imageView.SetImage(imageData);
            imageView.SetStoreMode(storeMode);
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

    public void UiEvent_BuyButtonClicked()
    {
        if (BuyClicked != null)
            BuyClicked();
    }
}
