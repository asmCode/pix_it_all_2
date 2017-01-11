using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteBundlesController
{
    private IRemoteBundles m_remoteBundles = new RemoteBundlesGSparks();

    public void DownloadMissingBundles(BundleData[] localBundles)
    {
        Debug.Log("Refreshing bundles...");

        if (GSpark.GetInstance().IsAuthenticated)
        {
            DownloadBundleListFromServer(localBundles);
        }
        else
        {
            Debug.Log("GameSparks is not authenticated. Authenticating...");

            GSpark.GetInstance().Authenticate((success) =>
            {
                Debug.LogFormat("GameSparks authenticated: {0}", success);

                if (success)
                    DownloadBundleListFromServer(localBundles);
            });
        }
    }

    private void DownloadBundleListFromServer(BundleData[] localBundles)
    {
        Debug.Log("Downloading bundle list...");

        if (m_remoteBundles.IsBundleListDownloading)
            return;

        m_remoteBundles.DownloadBundleList((success) =>
        {
            Debug.LogFormat("Downloaded bundle list: {0}", success);

            if (success && m_remoteBundles.IsBundleListReady)
            {
                DownloadBundles(m_remoteBundles.GetBundleList(), localBundles);
            }
        });
    }

    private void DownloadBundles(RemoteBundleData[] remoteBundles, BundleData[] localBundles)
    {
        Debug.Log("Downloading bundles...");

        if (remoteBundles == null || remoteBundles.Length == 0)
            return;

        foreach (var remoteBundleData in remoteBundles)
        {
            var localBundle = System.Array.Find(localBundles, (b) => { return b.Id == remoteBundleData.BundleId; });

            if (localBundle != null && localBundle.Crc == remoteBundleData.Crc)
            {
                Debug.LogFormat("Bundle {0} already exists and has the same crc, skipping", remoteBundleData.BundleId);
                continue;
            }

            if (localBundle != null)
                Debug.LogFormat("Bundle {0} has different crc, downloading", remoteBundleData.BundleId);
            else
                Debug.LogFormat("Bundle {0} doesn't exist locally, downloading", remoteBundleData.BundleId);

            m_remoteBundles.DownloadBundle(remoteBundleData, (success) =>
            {
                Debug.LogFormat("Finished downloading bundle {0}: {1}", remoteBundleData.BundleId, success);
            });
        }
    }
}
