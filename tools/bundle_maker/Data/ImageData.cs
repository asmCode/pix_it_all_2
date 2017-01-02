using System.Runtime.Serialization;

namespace bundle_maker
{
    [DataContract]
    class ImageData
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string RawImageData { get; set; }
    }
}
