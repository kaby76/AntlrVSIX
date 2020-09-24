using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    [DataContract]
    public class DocumentSymbolClientCapabilities
    {
        public DocumentSymbolClientCapabilities() { }

        /**
	     * Whether document symbol supports dynamic registration.
	     */
        [DataMember(Name = "dynamicRegistration")]
        [JsonProperty(Required = Required.Default)]
        public bool DynamicRegistration { get; set; }

        /**
          * Specific capabilities for the `SymbolKind` in the `textDocument/documentSymbol` request.
          */
        [DataMember(Name = "symbolKind")]
        [JsonProperty(Required = Required.Default)]
        public _DocumentSymbolClientCapabilities_SymbolKind SymbolKind { get; set; }

        /**
	     * The client supports hierarchical document symbols.
	     */
        [DataMember(Name = "hierarchicalDocumentSymbolSupport")]
        [JsonProperty(Required = Required.Default)]
        public bool? HierarchicalDocumentSymbolSupport { get; set; }

        /**
	     * The client supports tags on `SymbolInformation`. Tags are supported on
	     * `DocumentSymbol` if `hierarchicalDocumentSymbolSupport` is set to true.
	     * Clients supporting tags have to handle unknown tags gracefully.
	     *
	     * @since 3.16.0
	     */
        [DataMember(Name = "tagSupport")]
        [JsonProperty(Required = Required.Default)]
        public _DocumentSymbolClientCapabilities_TagSupport TagSupport { get; set; }
    }
}
