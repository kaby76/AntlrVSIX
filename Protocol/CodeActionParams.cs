using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the parameters sent from the client to the server for the
    //     textDocument/codeAction request.
    [DataContract]
    public class CodeActionParams
    {
        public CodeActionParams() { }

        //
        // Summary:
        //     Gets or sets the document identifier indicating where the command was invoked.
        [DataMember(Name = "textDocument")]
        public TextDocumentIdentifier TextDocument { get; set; }
        //
        // Summary:
        //     Gets or sets the range in the document for which the command was invoked.
        [DataMember(Name = "range")]
        public Range Range { get; set; }
        //
        // Summary:
        //     Gets or sets the additional diagnostic information about the code action context.
        [DataMember(Name = "context")]
        public CodeActionContext Context { get; set; }
    }
}
