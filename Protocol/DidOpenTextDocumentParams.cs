using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents the parameter that is sent with textDocument/didOpen message.
    [DataContract]
    public class DidOpenTextDocumentParams
    {
        public DidOpenTextDocumentParams() { }

        //
        // Summary:
        //     Gets or sets the Microsoft.VisualStudio.LanguageServer.Protocol.TextDocumentItem
        //     which represents the text document that was opened.
        [DataMember(Name = "textDocument")]
        public TextDocumentItem TextDocument { get; set; }
    }
}
