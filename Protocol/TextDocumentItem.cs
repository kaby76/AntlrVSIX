using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents a text document.
    [DataContract]
    public class TextDocumentItem
    {
        public TextDocumentItem() { }

        //
        // Summary:
        //     Gets or sets the document URI.
        [DataMember(Name = "uri")]
        [JsonProperty(Required = Required.Always)]
        public string Uri { get; set; }
        //
        // Summary:
        //     Gets or sets the document language identifier.
        [DataMember(Name = "languageId")]
        [JsonProperty(Required = Required.Always)]
        public string LanguageId { get; set; }
        //
        // Summary:
        //     Gets or sets the document version.
        [DataMember(Name = "version")]
        [JsonProperty(Required = Required.Always)]
        public int Version { get; set; }
        //
        // Summary:
        //     Gets or sets the content of the opened text document.
        [DataMember(Name = "text")]
        [JsonProperty(Required = Required.Always)]
        public string Text { get; set; }
    }
}
