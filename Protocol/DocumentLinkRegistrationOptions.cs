using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the options for registering textDocument/documentLink support.
    [DataContract]
    public class DocumentLinkRegistrationOptions
    {
        public DocumentLinkRegistrationOptions() { }

        //
        // Summary:
        //     Gets or sets a value indicating whether resolve providers are supported as well.
        [DataMember(Name = "resolveProvider")]
        public bool ResolveProvider { get; set; }
    }
}
