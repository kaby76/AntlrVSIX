using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    [DataContract]
    public class DefinitionClientCapabilities
    {
        public DefinitionClientCapabilities() { }

        /**
	     * Whether definition supports dynamic registration.
	     */
        [DataMember(Name = "dynamicRegistration")]
        [JsonProperty(Required = Required.Default)]
        public bool DynamicRegistration { get; set; }

        /**
	     * The client supports additional metadata in the form of definition links.
	     *
	     * @since 3.14.0
	     */
        [DataMember(Name = "linkSupport")]
        [JsonProperty(Required = Required.Default)]
        public bool LinkSupport { get; set; }
    }
}