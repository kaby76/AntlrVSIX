using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing a single signature of a callable item.
    [DataContract]
    public class SignatureInformation
    {
        public SignatureInformation() { }

        //
        // Summary:
        //     Gets or sets the label of this signature.
        [DataMember(Name = "label")]
        public string Label { get; set; }
        //
        // Summary:
        //     Gets or sets the human-readable documentation of this signature.
        [DataMember(Name = "documentation")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public SumType<string, MarkupContent> Documentation { get; set; }
        //
        // Summary:
        //     Gets or sets the parameters of this signature.
        [DataMember(Name = "parameters")]
        public ParameterInformation[] Parameters { get; set; }
    }
}
