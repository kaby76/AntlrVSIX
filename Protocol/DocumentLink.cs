using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the response of a textDocument/documentLink request.
    [DataContract]
    public class DocumentLink
    {
        public DocumentLink() { }

        //
        // Summary:
        //     Gets or sets the range the link applies to.
        [DataMember(Name = "range")]
        public Range Range { get; set; }
        //
        // Summary:
        //     Gets or sets the uri that the link points to.
        [DataMember(Name = "target")]
        //TODO [JsonConverter(typeof(DocumentUriConverter))]
        public string Target { get; set; }
    }
}
