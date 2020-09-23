using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    [DataContract]
    public class DocumentLinkClientCapabilities
    {
        public DocumentLinkClientCapabilities() { }

        /**
         * Whether document link supports dynamic registration.
         */
        [DataMember(Name = "dynamicRegistration")]
        [JsonProperty(Required = Required.Default)]
        public bool DynamicRegistration { get; set; }

        /**
	     * Whether the client supports the `tooltip` property on `DocumentLink`.
	     *
	     * @since 3.15.0
	     */
        [DataMember(Name = "tooltipSupport")]
        [JsonProperty(Required = Required.Default)]
        public bool TooltipSupport { get; set; }
    }
}