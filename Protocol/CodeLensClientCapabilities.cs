using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    [DataContract]
    public class CodeLensClientCapabilities
    {
        public CodeLensClientCapabilities() { }

        /**
         * Whether code lens supports dynamic registration.
         */
        [DataMember(Name = "dynamicRegistration")]
        [JsonProperty(Required = Required.Default)]
        public bool DynamicRegistration { get; set; }
    }
}