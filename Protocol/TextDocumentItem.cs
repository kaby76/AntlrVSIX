using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents a text document.
    [DataContract]
    public class TextDocumentItem
    {
        public TextDocumentItem() { }

        //
        // Summary:
        //     Gets or sets the document URI.
        [DataMember(Name = "uri")]
        public string Uri { get; set; }
        //
        // Summary:
        //     Gets or sets the document language identifier.
        [DataMember(Name = "languageId")]
        public string LanguageId { get; set; }
        //
        // Summary:
        //     Gets or sets the document version.
        [DataMember(Name = "version")]
        public int Version { get; set; }
        //
        // Summary:
        //     Gets or sets the content of the opened text document.
        [DataMember(Name = "text")]
        public string Text { get; set; }
    }
}
