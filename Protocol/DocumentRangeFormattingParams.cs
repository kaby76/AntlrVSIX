using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents the parameter that is sent with textDocument/rangeFormatting
    //     message.
    [DataContract]
    public class DocumentRangeFormattingParams
    {
        public DocumentRangeFormattingParams() { }

        //
        // Summary:
        //     Gets or sets the identifier for the text document to be formatted.
        [DataMember(Name = "textDocument")]
        public TextDocumentIdentifier TextDocument { get; set; }
        //
        // Summary:
        //     Gets or sets the selection range to be formatted.
        [DataMember(Name = "range")]
        public Range Range { get; set; }
        //
        // Summary:
        //     Gets or sets the formatting options.
        [DataMember(Name = "options")]
        public FormattingOptions Options { get; set; }
    }
}
