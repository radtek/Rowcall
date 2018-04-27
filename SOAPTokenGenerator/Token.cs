using System.Runtime.Serialization;

namespace SOAPTokenGenerator
{
    [DataContract]
    [System.Serializable]
    public class Token
    {
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public int Duration { get; set; }
    }
}