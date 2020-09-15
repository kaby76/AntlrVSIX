using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents a completion list.
    [DataContract]
    public class CompletionList
    {
        public CompletionList() { }

        //
        // Summary:
        //     Gets or sets a value indicating whether Items is the complete list of items or
        //     not. If incomplete is true, then filtering should ask the server again for completion
        //     item.
        [DataMember(Name = "isIncomplete")]
        public bool IsIncomplete { get; set; }
        //
        // Summary:
        //     Gets or sets the list of completion items.
        [DataMember(Name = "items")]
        public CompletionItem[] Items { get; set; }
    }
}
