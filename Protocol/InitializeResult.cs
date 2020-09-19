using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents the result returned by the initialize request.
    [DataContract]
    public class InitializeResult
    {
        public InitializeResult() { }

        //
        // Summary:
        //     Gets or sets the server capabilities.
        [DataMember(Name = "capabilities")]
        public ServerCapabilities Capabilities { get; set; }
    }
}