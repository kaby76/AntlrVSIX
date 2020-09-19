using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents the parameter that is sent with textDocument/didClose
    //     message.
    [DataContract]
    public class DidCloseTextDocumentParams
    {
        public DidCloseTextDocumentParams() { }

        //
        // Summary:
        //     Gets or sets the text document identifier.
        [DataMember(Name = "textDocument")]
        public TextDocumentIdentifier TextDocument { get; set; }
    }
}
