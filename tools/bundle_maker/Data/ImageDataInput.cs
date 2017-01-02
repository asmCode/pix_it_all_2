using System.Runtime.Serialization;

namespace bundle_maker
{
    [DataContract]
    class ImageDataInput
    {
        [DataMember]
        public string Name { get; set; }
    }
}
