using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which encapsulates a text document changed event.
    [DataContract]
    public class TextDocumentContentChangeEvent
    {
        public TextDocumentContentChangeEvent() { }

        //
        // Summary:
        //     Gets or sets the range of the text that was changed.
        [DataMember(Name = "range")]
        public Range Range { get; set; }
        //
        // Summary:
        //     Gets or sets the length of the range that got replaced.
        [DataMember(Name = "rangeLength")]
        public int RangeLength { get; set; }
        //
        // Summary:
        //     Gets or sets the new text of the range/document.
        [DataMember(Name = "text")]
        public string Text { get; set; }
    }
}
