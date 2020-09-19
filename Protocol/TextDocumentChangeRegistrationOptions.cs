using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the registration options for textDocument/didChange support.
    [DataContract]
    public class TextDocumentChangeRegistrationOptions : TextDocumentRegistrationOptions
    {
        public TextDocumentChangeRegistrationOptions() { }

        //
        // Summary:
        //     Gets or sets the Microsoft.VisualStudio.LanguageServer.Protocol.TextDocumentSyncKind
        //     that indicates how documents should be synced.
        [DataMember(Name = "syncKind")]
        public TextDocumentSyncKind SyncKind { get; set; }
    }
}
