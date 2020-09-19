using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the parameters sent from the client to the server for a textDocument/codeLens
    //     request.
    [DataContract]
    public class CodeLensParams
    {
        public CodeLensParams() { }

        //
        // Summary:
        //     Gets or sets the document identifier to fetch code lens results for.
        [DataMember(Name = "textDocument")]
        public TextDocumentIdentifier TextDocument { get; set; }
    }
}
