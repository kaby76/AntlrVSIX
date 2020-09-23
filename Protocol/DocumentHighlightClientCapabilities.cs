using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    [DataContract]
    public class DocumentHighlightClientCapabilities
    {
        public DocumentHighlightClientCapabilities() { }

        /**
         * Whether document highlight supports dynamic registration.
         */
        [DataMember(Name = "dynamicRegistration")]
        [JsonProperty(Required = Required.Default)]
        public bool DynamicRegistration { get; set; }
    }
}