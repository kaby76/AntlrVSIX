using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    [DataContract]
    public class CodeActionClientCapabilities
    {
        public CodeActionClientCapabilities() { }

        /**
         * Whether code action supports dynamic registration.
         */
        [DataMember(Name = "dynamicRegistration")]
        [JsonProperty(Required = Required.Default)]
        public bool DynamicRegistration { get; set; }

        /**
	     * The client supports code action literals as a valid
	     * response of the `textDocument/codeAction` request.
	     *
	     * @since 3.8.0
	     */
        [DataMember(Name = "codeActionLiteralSupport")]
        [JsonProperty(Required = Required.Default)]
        public _CodeActionClientCapabilities_CodeActionLiteralSupport CodeActionLiteralSupport { get; set; }

        /**
	     * Whether code action supports the `isPreferred` property.
	     * @since 3.15.0
	     */
        [DataMember(Name = "isPreferredSupport")]
        [JsonProperty(Required = Required.Default)]
        public bool IsPreferredSupport { get; set; }
    }
}