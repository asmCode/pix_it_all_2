using System.Runtime.Serialization;

namespace bundle_maker
{
    [DataContract]
    class BundleData
    {
        [DataMember(Order = 0)]
        public string Name { get; set; }

        [DataMember(Order = 1)]
        public ImageData[] Images { get; set; }
    }
}
