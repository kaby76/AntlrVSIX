using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the parameters sent for the textDocument/willSave request.
    [DataContract]
    public class WillSaveTextDocumentParams
    {
        public WillSaveTextDocumentParams() { }

        //
        // Summary:
        //     Gets or sets the Microsoft.VisualStudio.LanguageServer.Protocol.TextDocumentIdentifier
        //     representing the document to be saved.
        [DataMember(Name = "textDocument")]
        public TextDocumentIdentifier TextDocument { get; set; }
        //
        // Summary:
        //     Gets or sets the reason that the text document was saved.
        [DataMember(Name = "reason")]
        public TextDocumentSaveReason Reason { get; set; }
    }
}
