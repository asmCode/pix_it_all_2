using UnityEngine;

public class Paths
{
    public static string GetPurchasedBundlesDataPath()
    {
        return Application.persistentDataPath + "/purchased_bundles";
    }
}
