using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing a parameter of a callable signature.
    [DataContract]
    [JsonConverter(typeof(ParameterInformationConverter))]
    public class ParameterInformation
    {
        public ParameterInformation() { }

        //
        // Summary:
        //     Gets or sets the label of the parameter.
        [DataMember(Name = "label")]
        public SumType<string, Tuple<int, int>> Label { get; set; }
        //
        // Summary:
        //     Gets or sets the human-readable documentation of the parameter.
        [DataMember(Name = "documentation")]
        public SumType<string, MarkupContent> Documentation { get; set; }
    }
}
