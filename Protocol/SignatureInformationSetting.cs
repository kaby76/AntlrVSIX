using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the signature information initialization setting.
    [DataContract]
    public class SignatureInformationSetting
    {
        public SignatureInformationSetting() { }

        //
        // Summary:
        //     Gets or sets the set of documentation formats the client supports.
        [DataMember(Name = "documentationFormat")]
        public MarkupKind[] DocumentationFormat { get; set; }
        //
        // Summary:
        //     Gets or sets the parameter information the client supports.
        [DataMember(Name = "parameterInformation")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ParameterInformationSetting ParameterInformation { get; set; }
    }
}
