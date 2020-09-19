using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the parameter information initialization setting.
    [DataContract]
    public class ParameterInformationSetting
    {
        public ParameterInformationSetting() { }

        //
        // Summary:
        //     Gets or sets a value indicating whether the client supports label offset.
        [DataMember(Name = "labelOffsetSupport")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? LabelOffsetSupport { get; set; }
    }
}
