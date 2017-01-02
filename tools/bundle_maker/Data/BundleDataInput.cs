using System.Runtime.Serialization;

namespace bundle_maker
{
    [DataContract]
    class BundleDataInput
    {
        [DataMember]
        public string Name { get; set; }
    }
}
