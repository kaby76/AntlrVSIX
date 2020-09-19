using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Enum representing the possible reasons for an initialization error.
    [DataContract]
    public enum InitializeErrorCode
    {
        //
        // Summary:
        //     Protocol version can't be handled by the server.
        UnknownProtocolVersion = 1
    }
}
