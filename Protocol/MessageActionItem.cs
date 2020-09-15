using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represent an action the user performs after a window/showMessageRequest
    //     request is sent.
    [DataContract]
    public class MessageActionItem
    {
        public MessageActionItem() { }

        //
        // Summary:
        //     Gets or sets the title.
        [DataMember(Name = "title")]
        public string Title { get; set; }
    }
}
