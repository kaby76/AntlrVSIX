using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which identifies a text document.
    [DataContract]
    public class TextDocumentIdentifier
    {
        public TextDocumentIdentifier() { }

        //
        // Summary:
        //     Gets or sets the URI of the text document.
        [DataMember(Name = "uri")]
        public string Uri { get; set; }
    }
}
