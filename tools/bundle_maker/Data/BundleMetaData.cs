using System.Runtime.Serialization;

namespace bundle_maker
{
    [DataContract]
    class BundleMetaData
    {
        [DataMember]
        public string Name { get; set; }
    }
}
