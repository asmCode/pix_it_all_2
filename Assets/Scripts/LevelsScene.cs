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
        RefreshBundles();
        InitView();
        InitBundleList();
    }

    private void RefreshBundles()
    {
        Game.GetInstance().ImageManager.RefreshBundles();
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
        var playerProgress = Game.GetInstance().PlayerProgress;
        var levelProgress = playerProgress.GetLevelProgress(m_selectedBundleId, imageId);
        if (levelProgress == null)
            return;

        // TODO: "continue or start new image" popup

        Game.GetInstance().StartLevel(m_selectedBundleId, imageId, levelProgress.IsInProgress);
    }

    private void ShowImagesInBundle(string bundleId)
    {
        m_bundleListView.gameObject.SetActive(false);
        m_imagesPanel.gameObject.SetActive(true);

        var bundle = Game.GetInstance().ImageManager.GetBundleById(bundleId);
        if (bundle == null)
            return;

        m_selectedBundleId = bundleId;

        var images = bundle.GetImages();
        if (images == null)
            return;

        List<ImageViewData> imageViewDataList = new List<ImageViewData>();
        foreach (var image in images)
        {
            var imageViewData = CreateImageViewData(image);
            imageViewDataList.Add(imageViewData);
        }

        m_imageListView.SetImages(imageViewDataList);
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
        Game.GetInstance().ImageManager.BundlesChanged += HandleBundlesChanged;
    }

    private void OnDisable()
    {
        m_bundleListView.BundleClicked -= HandleBundleClicked;
        m_imageListView.ImageClicked -= HandleImageClicked;
        Game.GetInstance().ImageManager.BundlesChanged -= HandleBundlesChanged;
    }

    public void UiEvent_BackButtonClicked()
    {
        ShowBundles();
    }

    private void HandleBundlesChanged()
    {
        InitBundleList();
    }

    private ImageViewData CreateImageViewData(ImageData imageData)
    {
        var data = new ImageViewData();

        data.ImageData = imageData;

        int totalTiles = imageData.Texture.width * imageData.Texture.height;
        int totalColors = imageData.Colors.Length;

        var playerProgress = Game.GetInstance().PlayerProgress;
        var levelProgress = playerProgress.GetLevelProgress(m_selectedBundleId, imageData.Id);
        int stars = StarRatingCalc.GetStars(levelProgress.BestTime, totalTiles, totalColors);

        if (levelProgress != null)
        {
            data.BestTime = levelProgress.BestTime;
            data.InProgress = levelProgress.IsInProgress;
            data.Stars = levelProgress.IsCompleted ? stars : 0;
        }

        return data;
    }
}
