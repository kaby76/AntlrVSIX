using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the document link options for server capabilities.
    [DataContract]
    public class DocumentLinkOptions
    {
        public DocumentLinkOptions() { }

        //
        // Summary:
        //     Gets or sets a value indicating whether or not the server supports resolve providers.
        [DataMember(Name = "resolveProvider")]
        public bool ResolveProvider { get; set; }
    }
}
