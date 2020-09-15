using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing human readable text that should be rendered.
    [DataContract]
    public class MarkedString
    {
        public MarkedString() { }

        //
        // Summary:
        //     Gets or sets the language of the code stored in Microsoft.VisualStudio.LanguageServer.Protocol.MarkedString.Value.
        [DataMember(Name = "language")]
        [JsonProperty(Required = Required.Always)]
        public string Language { get; set; }
        //
        // Summary:
        //     Gets or sets the code.
        [DataMember(Name = "value")]
        [JsonProperty(Required = Required.Always)]
        public string Value { get; set; }
    }
}
