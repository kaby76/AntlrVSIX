using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents a text edit to a document.
    [DataContract]
    public class TextEdit
    {
        public TextEdit() { }

        //
        // Summary:
        //     Gets or sets the value which indicates the range of the text edit.
        [DataMember(Name = "range")]
        public Range Range { get; set; }
        //
        // Summary:
        //     Gets or sets the value of the new text.
        [DataMember(Name = "newText")]
        public string NewText { get; set; }
    }
}
