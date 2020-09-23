using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Protocol
{
    [DataContract]
    public class HoverClientCapabilities
    {
        public HoverClientCapabilities() { }

        /**
	     * Whether hover supports dynamic registration.
	     */
        [DataMember(Name = "dynamicRegistration")]
        [JsonProperty(Required = Required.Default)]
        public bool DynamicRegistration { get; set; }

        /**
          * Client supports the follow content formats for the content
          * property. The order describes the preferred format of the client.
          */
        [DataMember(Name = "contentFormat")]
        [JsonProperty(Required = Required.Default)]
        public MarkupKind[] ContentFormat { get; set; }
    }
}
