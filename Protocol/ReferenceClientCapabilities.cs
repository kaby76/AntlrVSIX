using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    [DataContract]
    public class ReferenceClientCapabilities
    {
        public ReferenceClientCapabilities() { }

        /**
         * Whether references supports dynamic registration.
         */
        [DataMember(Name = "dynamicRegistration")]
        [JsonProperty(Required = Required.Default)]
        public bool DynamicRegistration { get; set; }
    }
}