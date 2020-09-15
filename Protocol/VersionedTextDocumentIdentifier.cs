using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents a text document, but has a version identifier.
    [DataContract]
    public class VersionedTextDocumentIdentifier : TextDocumentIdentifier
    {
        public VersionedTextDocumentIdentifier() { }

        //
        // Summary:
        //     Gets or sets the version of the document.
        [DataMember(Name = "version")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Version { get; set; }
    }
}
