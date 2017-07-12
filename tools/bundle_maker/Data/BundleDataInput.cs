using System.Runtime.Serialization;

namespace bundle_maker
{
    [DataContract]
    class BundleDataInput
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string ProductId { get; set; }
    }
}
