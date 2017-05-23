using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This class is a little bit hacky but I didn't have time to implement it correctly.
// All logic is achieved here, which is not cool.
public class DebugView : MonoBehaviour
{
    public void UiEvent_RemoveGamePersistentData()
    {
        var path = Persistent.GetFilePath();
        System.IO.File.Delete(path);   
    }

    public void UiEvent_RemoveDownloadedBundles()
    {
        ImageDataLoader.RemoveDownloadedBundles();
    }

    public void UiEvent_RemoveLevelData()
    {
        Utils.DeleteDirectoryContent(LevelProgress.GetBasePath());
    }

    public void UiEvent_RemoveStoreData()
    {
        var purchasedBundlesPath = Paths.GetPurchasedBundlesDataPath();
        System.IO.File.Delete(purchasedBundlesPath);
    }
}
