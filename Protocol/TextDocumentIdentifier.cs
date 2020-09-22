using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Protocol
{
    //
    // Summary:
    //     Class which identifies a text document.
    [DataContract]
    public class TextDocumentIdentifier
    {
        public TextDocumentIdentifier() { }

        //
        // Summary:
        //     Gets or sets the URI of the text document.
        [DataMember(Name = "uri")]
        [JsonProperty(Required = Required.Always)]
        public string Uri { get; set; }
    }
}
