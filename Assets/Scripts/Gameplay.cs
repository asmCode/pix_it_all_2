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

    public ImageData Image
    {
        get;
        private set;
    }

    public void Init(string bundleId, string imageId)
    {
        Image = Game.GetInstance().ImageManager.GetImageById(bundleId, imageId);

        BundleId = bundleId;
        ImageId = imageId;
    }

    public void Complete(int time)
    {
        var playerProgress = Game.GetInstance().PlayerProgress;

        var levelProgress = playerProgress.GetLevelProgress(BundleId, ImageId);
        if (levelProgress == null)
            return;

        levelProgress.Complete(time);
        levelProgress.Save();
    }
}
