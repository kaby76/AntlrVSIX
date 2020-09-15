using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents the parameter that is sent with textDocument/didChange
    //     message.
    [DataContract]
    public class DidChangeTextDocumentParams
    {
        public DidChangeTextDocumentParams() { }

        //
        // Summary:
        //     Gets or sets the document that changed.
        [DataMember(Name = "textDocument")]
        public VersionedTextDocumentIdentifier TextDocument { get; set; }
        //
        // Summary:
        //     Gets or sets the content changes.
        [DataMember(Name = "contentChanges")]
        public TextDocumentContentChangeEvent[] ContentChanges { get; set; }
    }
}
