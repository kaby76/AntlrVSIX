using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents parameter sent with window/logMessage requests.
    [DataContract]
    public class LogMessageParams
    {
        public LogMessageParams() { }

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
