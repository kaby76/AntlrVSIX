using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the folding range request parameter.
    [DataContract]
    public class FoldingRangeParams
    {
        public FoldingRangeParams() { }

        //
        // Summary:
        //     Gets or sets the text document associated with the folding range request.
        [DataMember(Name = "textDocument")]
        public TextDocumentIdentifier TextDocument { get; set; }
    }
}
