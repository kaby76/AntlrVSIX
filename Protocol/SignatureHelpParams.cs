using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the parameters for the textDocument/signatureHelp request.
    [DataContract]
    public class SignatureHelpParams : TextDocumentPositionParams
    {
        public SignatureHelpParams() { }

        //
        // Summary:
        //     Gets or sets the signature help context.
        [DataMember(Name = "context")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public SignatureHelpContext Context { get; set; }
    }
}
