using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the data returned by a textDocument/hover request.
    [DataContract]
    public class Hover
    {
        public Hover() { }

        //
        // Summary:
        //     Gets or sets the content for the hover. Object can either be an array or a single
        //     object. If the object is an array the array can contain objects of type Microsoft.VisualStudio.LanguageServer.Protocol.MarkedString
        //     and System.String. If the object is not an array it can be of type Microsoft.VisualStudio.LanguageServer.Protocol.MarkedString,
        //     System.String, or Microsoft.VisualStudio.LanguageServer.Protocol.MarkupContent.
        [DataMember(Name = "contents")]
        public SumType<SumType<string, MarkedString>, SumType<string, MarkedString>[], MarkupContent> Contents { get; set; }
        //
        // Summary:
        //     Gets or sets the range over which the hover applies.
        [DataMember(Name = "range")]
        public Range Range { get; set; }
    }
}
