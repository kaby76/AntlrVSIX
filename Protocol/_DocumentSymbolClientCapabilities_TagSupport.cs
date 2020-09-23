using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    [DataContract]
    public class _DocumentSymbolClientCapabilities_TagSupport
    {
        public _DocumentSymbolClientCapabilities_TagSupport() { }

        /**
		 * The tags supported by the client.
		 */
        [DataMember(Name = "valueSet")]
        [JsonProperty(Required = Required.Default)]
        public SymbolTag[] ValueSet { get; set; }
    }
}