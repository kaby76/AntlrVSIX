using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing text and an associated format that should be rendered.
    [DataContract]
    public class MarkupContent
    {
        public MarkupContent() { }

        //
        // Summary:
        //     Gets or sets the Microsoft.VisualStudio.LanguageServer.Protocol.MarkupKind representing
        //     the text's format.
        [DataMember(Name = "kind")]
        public MarkupKind Kind { get; set; }
        //
        // Summary:
        //     Gets or sets the text that should be rendered.
        [DataMember(Name = "value")]
        public string Value { get; set; }
    }
}
