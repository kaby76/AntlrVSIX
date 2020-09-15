using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents the parameter that is sent with textDocument/formatting
    //     message.
    [DataContract]
    public class DocumentFormattingParams
    {
        public DocumentFormattingParams() { }

        //
        // Summary:
        //     Gets or sets the identifier for the text document to be formatted.
        [DataMember(Name = "textDocument")]
        public TextDocumentIdentifier TextDocument { get; set; }
        //
        // Summary:
        //     Gets or sets the formatting options.
        [DataMember(Name = "options")]
        public FormattingOptions Options { get; set; }
    }
}
