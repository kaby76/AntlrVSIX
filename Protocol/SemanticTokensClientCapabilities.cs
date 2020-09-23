using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    [DataContract]
    public class SemanticTokensClientCapabilities
    {
        public SemanticTokensClientCapabilities() { }

        /**
	     * Whether implementation supports dynamic registration. If this is set to `true`
	     * the client supports the new `(TextDocumentRegistrationOptions & StaticRegistrationOptions)`
	     * return value for the corresponding server capability as well.
	     */
        [DataMember(Name = "dynamicRegistration")]
        [JsonProperty(Required = Required.Default)]
        public bool DynamicRegistration { get; set; }

        /**
         * Which requests the client supports and might send to the server.
         */
        [DataMember(Name = "requests")]
        [JsonProperty(Required = Required.Default)]
        public _SemanticTokensClientCapabilities_Requests Requests { get; set; }

        /**
         * The token types that the client supports.
         */
        [DataMember(Name = "tokenTypes")]
        [JsonProperty(Required = Required.Default)]
        public string[] TokenTypes { get; set; }

        /**
	     * The token modifiers that the client supports.
	     */
        [DataMember(Name = "tokenModifiers")]
        [JsonProperty(Required = Required.Default)]
        public string[] TokenModifiers { get; set; }

        /**
	     * The formats the clients supports.
	     */
        [DataMember(Name = "formats")]
        [JsonProperty(Required = Required.Default)]
        public TokenFormat[] Formats { get; set; }
    }
}