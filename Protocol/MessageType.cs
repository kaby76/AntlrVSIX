using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Message type enum.
    [DataContract]
    public enum MessageType
    {
        //
        // Summary:
        //     Error message.
        Error = 1,
        //
        // Summary:
        //     Warning message.
        Warning = 2,
        //
        // Summary:
        //     Info message.
        Info = 3,
        //
        // Summary:
        //     Log message.
        Log = 4
    }
}
