public class RemoteBundleData
{
    public string RemoteId
    {
        get;
        private set;
    }

    public string Crc
    {
        get;
        private set;
    }

    public string BundleId
    {
        get;
        private set;
    }

    public RemoteBundleData(string remoteId, string crc, string bundleId)
    {
        RemoteId = remoteId;
        Crc = crc;
        BundleId = bundleId;
    }
}
