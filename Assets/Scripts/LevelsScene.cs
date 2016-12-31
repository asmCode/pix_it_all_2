using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsScene : MonoBehaviour
{
    public BundleListView m_bundleListView;
    public RectTransform m_imagesPanel;
    public ImageListView m_imageListView;

    private string m_selectedBundleId;

    private void Awake()
    {
    }

    void Start()
    {
        InitView();
        InitBundleList();
    }

    private void InitView()
    {
        ShowBundles();
    }

    private void InitBundleList()
    {
        var bundles = Game.GetInstance().ImageManager.Bundles;
        if (bundles == null)
            return;

        m_bundleListView.SetBundles(bundles);
    }

    private void HandleBundleClicked(string bundleId)
    {
        ShowImagesInBundle(bundleId);
    }

    private void HandleImageClicked(string imageId)
    {
        Game.GetInstance().StartLevel(m_selectedBundleId, imageId);
    }

    private void ShowImagesInBundle(string bundleId)
    {
        m_bundleListView.gameObject.SetActive(false);
        m_imagesPanel.gameObject.SetActive(true);

        var bundle = Game.GetInstance().ImageManager.GetBundleById(bundleId);
        if (bundle == null)
            return;

        m_selectedBundleId = bundleId;

        m_imageListView.SetImages(bundle.GetImages());
    }

    private void ShowBundles()
    {
        m_bundleListView.gameObject.SetActive(true);
        m_imagesPanel.gameObject.SetActive(false);

        m_selectedBundleId = null;
    }

    private void OnEnable()
    {
        m_bundleListView.BundleClicked += HandleBundleClicked;
        m_imageListView.ImageClicked += HandleImageClicked;
    }

    private void OnDisable()
    {
        m_bundleListView.BundleClicked -= HandleBundleClicked;
        m_imageListView.ImageClicked -= HandleImageClicked;
    }

    public void UiEvent_BackButtonClicked()
    {
        ShowBundles();
    }
}
