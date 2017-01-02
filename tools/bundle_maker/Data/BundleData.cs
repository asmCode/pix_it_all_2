using System.Runtime.Serialization;

namespace bundle_maker
{
    [DataContract]
    class BundleData
    {
        [DataMember]
        public BundleMetaData BundleMetaData { get; set; }

        [DataMember]
        public ImageData[] Images { get; set; }
    }
}
