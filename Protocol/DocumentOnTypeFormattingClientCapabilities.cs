using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    [DataContract]
    public class DocumentOnTypeFormattingClientCapabilities
    {
        public DocumentOnTypeFormattingClientCapabilities() { }

        /**
	     * Whether on type formatting supports dynamic registration.
	     */
        [DataMember(Name = "dynamicRegistration")]
        [JsonProperty(Required = Required.Default)]
        public bool DynamicRegistration { get; set; }
    }
}