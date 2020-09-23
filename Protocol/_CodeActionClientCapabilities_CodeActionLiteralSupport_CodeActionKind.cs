using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    [DataContract]
    public class _CodeActionClientCapabilities_CodeActionLiteralSupport_CodeActionKind
    {
        public _CodeActionClientCapabilities_CodeActionLiteralSupport_CodeActionKind() { }

        /**
		 * The code action kind values the client supports. When this
		 * property exists the client also guarantees that it will
		 * handle values outside its set gracefully and falls back
		 * to a default value when unknown.
		 */
        [DataMember(Name = "valueSet")]
        [JsonProperty(Required = Required.Default)]
        public CodeActionKind[] ValueSet { get; set; }
    }
}