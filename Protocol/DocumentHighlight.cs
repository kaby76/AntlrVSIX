using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the response from a textDocument/documentHighlight request.
    [DataContract]
    public class DocumentHighlight
    {
        public DocumentHighlight() { }

        //
        // Summary:
        //     Gets or sets the range that the highlight applies to.
        [DataMember(Name = "range")]
        public Range Range { get; set; }
        //
        // Summary:
        //     Gets or sets the kind of highlight.
        [DataMember(Name = "kind")]
        public DocumentHighlightKind Kind { get; set; }
    }
}
