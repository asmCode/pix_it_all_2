using System.Collections.Generic;
using GameSparks.Api.Responses;
using GameSparks.Api.Requests;
using UnityEngine;

public class RemoteBundlesGSparks : IRemoteBundles
{
    private RemoteBundleData[] m_remoteBundleList;
    private object m_remoteBundleListLock = new object();

    public bool IsBundleListDownloading
    {
        get;
        private set;
    }

    public bool IsBundleListReady
    {
        get
        {
            bool isReady = false;
            lock (m_remoteBundleListLock)
                isReady = m_remoteBundleList != null;
            return isReady;
        }
    }

    public void DownloadBundleList(System.Action<bool> callback)
    {
        if (IsBundleListDownloading)
            return;

        IsBundleListDownloading = true;

        lock (m_remoteBundleListLock)
            m_remoteBundleList = null;

        new LogEventRequest()
            .SetEventKey("CC_GET_BUNDLES")
            .SetEventAttribute("CC_DUMMY", "")
            .Send((response) =>
            {
                Dispatcher.Dispatch((what) =>
                {
                    ProcessDownloadBundleListResponse(response, callback);
                });
            });
    }

    public RemoteBundleData[] GetBundleList()
    {
        RemoteBundleData[] result = null;

        lock (m_remoteBundleListLock)
        {
            if (m_remoteBundleList != null)
            {
                result = new RemoteBundleData[m_remoteBundleList.Length];
                System.Array.Copy(m_remoteBundleList, result, m_remoteBundleList.Length);
            }
        }

        return result;
    }

    public void DownloadBundle(RemoteBundleData remoteBundleData, System.Action<string> callback)
    {
        if (remoteBundleData == null || string.IsNullOrEmpty(remoteBundleData.RemoteId))
            return;

        new GetDownloadableRequest()
            .SetShortCode(remoteBundleData.RemoteId)
            .Send((response) =>
            {
                Dispatcher.Dispatch((what) =>
                {
                    ProcessDownloadBundleResponse(response, remoteBundleData.Crc, remoteBundleData.BundleId, callback);
                });
            });
    }

    private void ProcessDownloadBundleListResponse(LogEventResponse response, System.Action<bool> callback)
    {
        IsBundleListDownloading = false;

        if (response == null || response.ScriptData == null || response.HasErrors)
        {
            if (callback != null)
                callback(false);

            return;
        }

        var bundles = response.ScriptData.GetGSDataList("bundles");
        if (bundles == null || bundles.Count == 0)
        {
            if (callback != null)
                callback(false);

            return;
        }
            
        List<RemoteBundleData> bundles_data = new List<RemoteBundleData>();

        foreach (var item in bundles)
        {
            var shortCode = item.GetString("shortCode");
            var crc = item.GetString("crc");
            var bundleId = item.GetString("bundleId");

            var remote_bundle_data = new RemoteBundleData(shortCode, crc, bundleId);
            bundles_data.Add(remote_bundle_data);
        }

        lock (m_remoteBundleListLock)
            m_remoteBundleList = bundles_data.ToArray();

        if (callback != null)
            callback(true);
    }

    private void ProcessDownloadBundleResponse(
        GetDownloadableResponse response,
        string crc,
        string bundleId,
        System.Action<string> callback)
    {
        if (response.HasErrors)
        {
            if (callback != null)
                callback(null);

            return;
        }

        var basePath = Application.persistentDataPath + "/Bundles/";
        if (!System.IO.Directory.Exists(basePath))
        {
            Debug.LogErrorFormat("Path {0} doesn't exist.", basePath);

            if (callback != null)
                callback(null);

            return;
        }

        var dstPath = basePath + bundleId + ".pixbundle";
        var dstPathCrc = basePath + bundleId + ".pixcrc";

        FileDownloader.Download(response.Url, dstPath, (success) =>
        {
            if (success)
            {
                System.IO.File.WriteAllText(dstPathCrc, crc.ToString());
            }
            else
            {
                Debug.LogErrorFormat("Couldn't download bundle {0}", bundleId);
            }

            if (callback != null)
                callback(success ? bundleId : null);
        });
    }
}
