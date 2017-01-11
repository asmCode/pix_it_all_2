using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteBundlesController
{
    private IRemoteBundles m_remoteBundles = new RemoteBundlesGSparks();

    public void DownloadMissingBundles(BundleData[] localBundles, System.Action<DownloadMissingBundlesEventData> callback)
    {
        Debug.Log("Refreshing bundles...");

        if (GSpark.GetInstance().IsAuthenticated)
        {
            DownloadBundleListFromServer(localBundles, callback);
        }
        else
        {
            Debug.Log("GameSparks is not authenticated. Authenticating...");

            GSpark.GetInstance().Authenticate((success) =>
            {
                Debug.LogFormat("GameSparks authenticated: {0}", success);

                if (success)
                    DownloadBundleListFromServer(localBundles, callback);
            });
        }
    }

    private void DownloadBundleListFromServer(BundleData[] localBundles, System.Action<DownloadMissingBundlesEventData> callback)
    {
        Debug.Log("Downloading bundle list...");

        if (m_remoteBundles.IsBundleListDownloading)
            return;

        m_remoteBundles.DownloadBundleList((success) =>
        {
            Debug.LogFormat("Downloaded bundle list: {0}", success);

            if (success && m_remoteBundles.IsBundleListReady)
            {
                DownloadBundles(m_remoteBundles.GetBundleList(), localBundles, callback);
            }
        });
    }

    private void DownloadBundles(RemoteBundleData[] remoteBundles, BundleData[] localBundles, System.Action<DownloadMissingBundlesEventData> callback)
    {
        Debug.Log("Downloading bundles...");

        if (remoteBundles == null || remoteBundles.Length == 0)
            return;

        bool newBundles = false;
        int bundlesToDownload = 0;

        foreach (var remoteBundleData in remoteBundles)
        {
            var localBundle = System.Array.Find(localBundles, (b) => { return b.Id == remoteBundleData.BundleId; });

            if (localBundle != null && localBundle.Crc == remoteBundleData.Crc)
            {
                Debug.LogFormat("Bundle {0} already exists and has the same crc, skipping", remoteBundleData.BundleId);
                continue;
            }

            newBundles = true;
            bundlesToDownload++;

            if (localBundle != null)
                Debug.LogFormat("Bundle {0} has different crc, downloading", remoteBundleData.BundleId);
            else
                Debug.LogFormat("Bundle {0} doesn't exist locally, downloading", remoteBundleData.BundleId);

            m_remoteBundles.DownloadBundle(remoteBundleData, (success) =>
            {
                Debug.LogFormat("Finished downloading bundle {0}: {1}", remoteBundleData.BundleId, success);
                bundlesToDownload--;

                // 0 means, that it was the last bundle.
                if (bundlesToDownload == 0)
                {
                    if (callback != null)
                        callback(new DownloadMissingBundlesEventData(newBundles, true));
                }
            });
        }

        if (!newBundles)
        {
            if (callback != null)
                callback(new DownloadMissingBundlesEventData(false, true));
        }
    }
}
