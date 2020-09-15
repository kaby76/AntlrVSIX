using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the rename parameters for the textDocument/rename request.
    [DataContract]
    public class RenameParams
    {
        public RenameParams() { }

        //
        // Summary:
        //     Gets or sets the document identifier for the document to format.
        [DataMember(Name = "textDocument")]
        public TextDocumentIdentifier TextDocument { get; set; }
        //
        // Summary:
        //     Gets or sets the position at which the request was sent.
        [DataMember(Name = "position")]
        public Position Position { get; set; }
        //
        // Summary:
        //     Gets or sets the new name of the renamed symbol.
        [DataMember(Name = "newName")]
        public string NewName { get; set; }
    }
}
