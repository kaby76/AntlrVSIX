using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    [DataContract]
    public class RenameClientCapabilities
    {
        public RenameClientCapabilities() { }

        /**
	     * Whether rename supports dynamic registration.
	     */
        [DataMember(Name = "dynamicRegistration")]
        [JsonProperty(Required = Required.Default)]
        public bool DynamicRegistration { get; set; }

        /**
	     * Client supports testing for validity of rename operations
	     * before execution.
	     *
	     * @since version 3.12.0
	     */
        [DataMember(Name = "prepareSupport")]
        [JsonProperty(Required = Required.Default)]
        public bool PrepareSupport { get; set; }

        /**
	     * Client supports the default behavior result (`{ defaultBehavior: boolean }`).
	     *
	     * @since version 3.16.0
	     */
        [DataMember(Name = "prepareSupportDefaultBehavior")]
        [JsonProperty(Required = Required.Default)]
        public bool PrepareSupportDefaultBehavior { get; set; }
    }
}