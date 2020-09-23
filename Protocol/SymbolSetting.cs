using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the symbol setting for initialization.
    [DataContract]
    public class SymbolSetting : DynamicRegistrationSetting
    {
        public SymbolSetting() { }

        //
        // Summary:
        //     Gets or sets the Microsoft.VisualStudio.LanguageServer.Protocol.SymbolKindSetting
        //     information.
        [DataMember(Name = "symbolKind")]
        public _DocumentSymbolClientCapabilities_SymbolKind SymbolKind { get; set; }
    }
}
