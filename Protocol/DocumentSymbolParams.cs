using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents the parameter sent with textDocument/documentSymbol requests.
    [DataContract]
    public class DocumentSymbolParams
    {
        public DocumentSymbolParams() { }

        //
        // Summary:
        //     Gets or sets the text document.
        [DataMember(Name = "textDocument")]
        public TextDocumentIdentifier TextDocument { get; set; }
    }
}
