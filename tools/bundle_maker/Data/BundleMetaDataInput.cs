using System.Runtime.Serialization;

namespace bundle_maker
{
    [DataContract]
    class BundleMetaDataInput
    {
        [DataMember]
        public string Name { get; set; }
    }
}
