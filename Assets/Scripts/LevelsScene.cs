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

        List<BundleViewData> bundleViews = new List<BundleViewData>();

        foreach (var bundle in bundles)
        {
            var bundleView = CreateBundleViewData(bundle);
            if (bundleView == null)
                continue;

            bundleViews.Add(bundleView);
        }

        m_bundleListView.SetBundles(bundleViews.ToArray());
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

        if (levelProgress.IsInProgress)
        {
            Popup.Show("CONTINUE?\nTAP \"NO\" TO START FROM SCRATCH", (int)Popup.Button.No | (int)Popup.Button.Yes, (button) =>
            {
                if (button == Popup.Button.No)
                {
                    levelProgress.ClearContinue();
                    levelProgress.Save();
                }

                Game.GetInstance().StartLevel(m_selectedBundleId, imageId, levelProgress.IsInProgress);
            });
        }
        else
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

    private BundleViewData CreateBundleViewData(BundleData bundleData)
    {
        var game = Game.GetInstance();

        var data = new BundleViewData();

        data.BundleData = bundleData;
        data.IsAvailable = game.ImageManager.IsBundleAvailable(bundleData.Id);

        if (!data.IsAvailable)
        {
            var product = game.Purchaser.GetProductById(data.BundleData.StoreId);
            if (product == null)
                return null;

            data.LocalizedPrice = product.LocalizedPrice;
        }
    
        return data;
    }
}
