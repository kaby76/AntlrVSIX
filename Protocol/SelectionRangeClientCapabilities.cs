using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    [DataContract]
    public class SelectionRangeClientCapabilities
    {
        public SelectionRangeClientCapabilities() { }

        /**
	     * Whether implementation supports dynamic registration for selection range providers. If this is set to `true`
	     * the client supports the new `SelectionRangeRegistrationOptions` return value for the corresponding server
	     * capability as well.
	     */
        [DataMember(Name = "dynamicRegistration")]
        [JsonProperty(Required = Required.Default)]
        public bool DynamicRegistration { get; set; }
    }
}