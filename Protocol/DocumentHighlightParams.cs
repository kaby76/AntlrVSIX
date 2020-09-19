using System;
using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the parameters sent for a textDocument/documentHighlight request.
    public class DocumentHighlightParams : TextDocumentPositionParams
    {
        public DocumentHighlightParams() { }

        //
        // Summary:
        //     Gets or sets the value of the PartialResultToken instance.
        [DataMember(Name = "partialResultToken", IsRequired = false)]
        public IProgress<DocumentHighlight[]> PartialResultToken { get; set; }
    }
}
