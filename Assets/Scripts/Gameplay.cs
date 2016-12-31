using UnityEngine;
using System.Collections;

public class Gameplay
{
    public ImageData Image
    {
        get;
        private set;
    }

    public void Init(string bundleId, string imageId)
    {
        Image = Game.GetInstance().ImageManager.GetImageById(bundleId, imageId);
    }
}
