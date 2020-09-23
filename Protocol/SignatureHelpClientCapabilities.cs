using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Protocol
{
    [DataContract]
    public class SignatureHelpClientCapabilities
    {
        public SignatureHelpClientCapabilities() { }

        /**
	     * Whether signature help supports dynamic registration.
	     */
        [DataMember(Name = "dynamicRegistration")]
        [JsonProperty(Required = Required.Default)]
        public bool DynamicRegistration { get; set; }

        /**
	     * The client supports the following `SignatureInformation`
	     * specific properties.
	     */
        [DataMember(Name = "signatureInformation")]
        [JsonProperty(Required = Required.Default)]
        public _SignatureHelpClientCapabilities_SignatureInformation SignatureInformation { get; set; }

        /**
	     * The client supports to send additional context information for a
	     * `textDocument/signatureHelp` request. A client that opts into
	     * contextSupport will also support the `retriggerCharacters` on
	     * `SignatureHelpOptions`.
	     *
	     * @since 3.15.0
	     */
        [DataMember(Name = "contextSupport")]
        [JsonProperty(Required = Required.Default)]
        public bool ContextSupport { get; set; }
    }
}
