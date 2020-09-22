using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    [DataContract]
    public class TextDocumentRegistrationOptions
    {
        public TextDocumentRegistrationOptions() { }

        /**
         * A document selector to identify the scope of the registration. If set to null
         * the document selector provided on the client side will be used.
         */
        [DataMember(Name = "documentSelector")]
        [JsonProperty(Required = Required.Always)]
        public DocumentFilter[] documentSelector { get; set; }
    }
}
