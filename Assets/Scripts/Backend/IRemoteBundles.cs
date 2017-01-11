public interface IRemoteBundles
{
    bool IsBundleListDownloading { get; }
    bool IsBundleListReady{ get; }

    void DownloadBundleList(System.Action<bool> callback);
    RemoteBundleData[] GetBundleList();
    void DownloadBundle(RemoteBundleData remoteBundleData, System.Action<bool> callback);
}
