using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents parameter sent with window/showMessage requests.
    [DataContract]
    public class ShowMessageParams
    {
        public ShowMessageParams() { }

        //
        // Summary:
        //     Gets or sets the type of message.
        [DataMember(Name = "type")]
        public MessageType MessageType { get; set; }
        //
        // Summary:
        //     Gets or sets the message.
        [DataMember(Name = "message")]
        public string Message { get; set; }
    }
}
