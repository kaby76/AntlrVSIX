using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    [DataContract]
    public class DocumentColorClientCapabilities
    {
        public DocumentColorClientCapabilities() { }

        /**
	     * Whether document color supports dynamic registration.
	     */
        [DataMember(Name = "dynamicRegistration")]
        [JsonProperty(Required = Required.Default)]
        public bool DynamicRegistration { get; set; }
    }
}