using UnityEngine;
using System.Collections;

public class Gameplay
{
    public string BundleId
    {
        get;
        private set;
    }

    public string ImageId
    {
        get;
        private set;
    }

    public ImageData ReferenceImage
    {
        get;
        private set;
    }

    public ImageProgress ImageProgress
    {
        get;
        private set;
    }

    public LevelProgress LevelProgress
    {
        get;
        private set;
    }

    public float Time
    {
        get;
        private set;
    }

    public void Init(string bundleId, string imageId, bool continueLevel)
    {
        BundleId = bundleId;
        ImageId = imageId;

        ReferenceImage = Game.GetInstance().ImageManager.GetImageById(bundleId, imageId);

        ImageProgress = new ImageProgress();
        ImageProgress.Init(ReferenceImage.Texture.width, ReferenceImage.Texture.height);

        InitLevelProgress();

        if (continueLevel)
            InitContinue();
    }

    public void AddSeconds(float deltaTime)
    {
        Time += deltaTime;
    }

    public Color GetReferenceColor(int x, int y)
    {
        return ReferenceImage.Texture.GetPixel(x, y);
    }

    public void Complete()
    {
        if (LevelProgress == null)
            return;

        LevelProgress.Complete(Time);
        LevelProgress.Save();
    }

    public void SaveProgress(Texture2D image)
    {
        if (LevelProgress == null)
            return;

        var tiles = ImageProgress.GetTiles();
        if (tiles == null)
            return;

        LevelProgress.SaveProgress(Time, tiles);
        LevelProgress.Save();
    }

    private void InitLevelProgress()
    {
        LevelProgress = null;

        var playerProgress = Game.GetInstance().PlayerProgress;

        LevelProgress = playerProgress.GetLevelProgress(BundleId, ImageId);
        if (LevelProgress == null)
            return;
    }

    private void InitContinue()
    {
        if (!LevelProgress.IsInProgress)
            return;

        var tiles = LevelProgress.GetContinueImageData();
        if (tiles == null)
            return;

        ImageProgress.SetTiles(tiles);

        Time = LevelProgress.ContinueTime;
    }
}
