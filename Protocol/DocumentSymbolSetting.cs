using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the initialization setting for document symbols.
    [DataContract]
    public class DocumentSymbolSetting : DynamicRegistrationSetting
    {
        public DocumentSymbolSetting() { }

        //
        // Summary:
        //     Gets or sets the Microsoft.VisualStudio.LanguageServer.Protocol.SymbolKindSetting
        //     capabilities.
        [DataMember(Name = "symbolKind")]
        public SymbolKindSetting SymbolKind { get; set; }
        //
        // Summary:
        //     Gets or sets a value indicating whether the document has hierarchical symbol
        //     support.
        [DataMember(Name = "hierarchicalDocumentSymbolSupport")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? HierarchicalDocumentSymbolSupport { get; set; }
    }
}
