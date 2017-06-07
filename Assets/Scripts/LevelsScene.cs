using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelsScene : MonoBehaviour
{
    public static string m_bundleIdToSelect;

    public BundleListView m_bundleListView;
    public RectTransform m_imagesPanel;
    public ImageListView m_imageListView;
    public Text m_topBarTitle;

    private string m_selectedBundleId;
    private bool m_refreshBundlesOnNextEnter;

    private void Awake()
    {
    }

    void Start()
    {
        InitScene.PlayMusic();

        RefreshBundles();
        InitView();
        InitBundleList();

        if (!string.IsNullOrEmpty(m_bundleIdToSelect))
            ShowImagesInBundle(m_bundleIdToSelect);

        m_bundleIdToSelect = null;
    }

    private void RefreshBundles()
    {
        Pix.Game.GetInstance().ImageManager.RefreshBundles();
    }

    private void InitView()
    {
        ShowBundles();
    }

    private void InitBundleList()
    {
        var bundles = Pix.Game.GetInstance().ImageManager.Bundles;
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

    private bool IsInImageListView()
    {
        return m_imageListView.gameObject.activeInHierarchy;
    }

    private void HandleBundleClicked(string bundleId)
    {
        ShowImagesInBundle(bundleId);
    }

    private void HandleImageClicked(string imageId)
    {
        var game = Pix.Game.GetInstance();

        if (!game.ImageManager.IsBundleAvailable(m_selectedBundleId))
        {
             AttemptToBuyBundle(m_selectedBundleId);
             return;
        }

        var playerProgress = Pix.Game.GetInstance().PlayerProgress;
        var levelProgress = playerProgress.GetLevelProgress(m_selectedBundleId, imageId);
        if (levelProgress == null)
            return;

        if (levelProgress.IsInProgress)
        {
            Popup.Show("LEVEL IN PROGRESS\n\nCONTINUE?", (int)Popup.Button.No | (int)Popup.Button.Yes | (int)Popup.Button.Cancel, Popup.Button.Cancel, (button) =>
            {
                if (button == Popup.Button.Cancel)
                    return;

                if (button == Popup.Button.No)
                {
                    levelProgress.ClearContinue();
                    levelProgress.Save();
                }

                Pix.Game.GetInstance().StartLevel(m_selectedBundleId, imageId, levelProgress.IsInProgress);
            });
        }
        else
            Pix.Game.GetInstance().StartLevel(m_selectedBundleId, imageId, levelProgress.IsInProgress);
    }

    private void AttemptToBuyBundle(string bundleId)
    {
        var game = Pix.Game.GetInstance();

        var bundle = game.ImageManager.GetBundleById(bundleId);
        if (bundle == null)
            return;

        if (game.ImageManager.IsBundleAvailable(bundleId))
        {
            Debug.LogWarningFormat("Attempting to buy available bundle: {0}", bundle.ProductId);
            return;
        }

        var product = game.Purchaser.GetProductById(bundle.ProductId);
        if (product == null)
        {
            Debug.LogWarningFormat("Unknown product id: {0}", bundle.ProductId);
            return;
        }

        game.Purchaser.BuyProductId(product.Id);
    }

    private void HandleBuyClicked()
    {
        AttemptToBuyBundle(m_selectedBundleId);
    }

    private void ShowImagesInBundle(string bundleId)
    {
        m_bundleListView.gameObject.SetActive(false);
        m_imagesPanel.gameObject.SetActive(true);

        var game = Pix.Game.GetInstance();

        var bundle = game.ImageManager.GetBundleById(bundleId);
        if (bundle == null)
            return;

        m_selectedBundleId = bundleId;

        var images = bundle.GetImages();
        if (images == null)
            return;

        List<ImageViewData> imageViewDataList = new List<ImageViewData>();
        foreach (var image in images)
        {
            var imageViewData = CreateImageViewData(image, m_selectedBundleId);
            imageViewDataList.Add(imageViewData);
        }

        m_topBarTitle.text = bundle.Name;

        bool storeMode = !game.ImageManager.IsBundleAvailable(bundleId);
        m_imageListView.Init(imageViewDataList, storeMode, GetLocalizedPrice(bundle.ProductId));
    }

    private string GetLocalizedPrice(string productId)
    {
        var product = Pix.Game.GetInstance().Purchaser.GetProductById(productId);
        if (product == null || string.IsNullOrEmpty(product.LocalizedPrice))
            return null;
        else
            return product.LocalizedPrice;
    }

    private void ShowBundles()
    {
        m_topBarTitle.text = "Choose Bundle";
        m_bundleListView.gameObject.SetActive(true);
        m_imagesPanel.gameObject.SetActive(false);

        if (m_refreshBundlesOnNextEnter)
        {
            m_refreshBundlesOnNextEnter = false;
            InitBundleList();
        }

        m_selectedBundleId = null;
    }

    private void OnEnable()
    {
        m_bundleListView.BundleClicked += HandleBundleClicked;
        m_imageListView.ImageClicked += HandleImageClicked;
        m_imageListView.BuyClicked += HandleBuyClicked;
        Pix.Game.GetInstance().ImageManager.BundlesChanged += HandleBundlesChanged;
    }

    private void OnDisable()
    {
        m_bundleListView.BundleClicked -= HandleBundleClicked;
        m_imageListView.ImageClicked -= HandleImageClicked;
        m_imageListView.BuyClicked -= HandleBuyClicked;
        Pix.Game.GetInstance().ImageManager.BundlesChanged -= HandleBundlesChanged;
    }

    public void UiEvent_BackButtonClicked()
    {
        GoBack();
    }

    private void HandleBundlesChanged()
    {
        if (IsInImageListView())
        {
            // Set the flag to refresh bundle view next time when we enter to the bundles view
            m_refreshBundlesOnNextEnter = true;
            ShowImagesInBundle(m_selectedBundleId);
        }
        else
            InitBundleList();
    }

    public static ImageViewData CreateImageViewData(ImageData imageData, string bundleId)
    {
        var data = new ImageViewData();

        data.ImageData = imageData;

        int totalTiles = imageData.Texture.width * imageData.Texture.height;
        int totalColors = imageData.Colors.Length;

        var playerProgress = Pix.Game.GetInstance().PlayerProgress;
        var levelProgress = playerProgress.GetLevelProgress(bundleId, imageData.Id);
        int stars = StarRatingCalc.GetStars(levelProgress.BestTime, totalTiles, totalColors);

        data.LevelProgress = levelProgress;
        data.Stars = levelProgress.IsCompleted ? stars : 0;

        return data;
    }

    private BundleViewData CreateBundleViewData(BundleData bundleData)
    {
        var game = Pix.Game.GetInstance();

        var data = new BundleViewData();

        data.BundleData = bundleData;
        data.IsAvailable = game.ImageManager.IsBundleAvailable(bundleData.Id);

        return data;
    }

    public void HandleBackButton()
    {
        // First try to handle back button in the popup (if visible)
        if (Popup.IsVisible() && Popup.HandleBackButton())
            return;

        GoBack();
    }

    private void GoBack()
    {
        if (IsInImageListView())
            ShowBundles();
        else
            SceneManager.LoadScene("Wellcome");
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            HandleBackButton();
        }
    }
}
