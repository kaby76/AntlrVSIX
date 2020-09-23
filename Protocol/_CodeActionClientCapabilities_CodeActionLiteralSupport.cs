using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    [DataContract]
    public class _CodeActionClientCapabilities_CodeActionLiteralSupport
    {
        public _CodeActionClientCapabilities_CodeActionLiteralSupport() { }

        /**
		 * The code action kind is supported with the following value
		 * set.
		 */
        [DataMember(Name = "codeActionKind")]
        [JsonProperty(Required = Required.Default)]
        public _CodeActionClientCapabilities_CodeActionLiteralSupport_CodeActionKind CodeActionKind { get; set; }
    }
}