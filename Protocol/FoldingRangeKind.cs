using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Enum representing various code action kinds.
    [DataContract]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum FoldingRangeKind
    {
        //
        // Summary:
        //     Comment folding range.
        [EnumMember(Value = "comment")]
        Comment = 0,
        //
        // Summary:
        //     Imports folding range.
        [EnumMember(Value = "imports")]
        Imports = 1,
        //
        // Summary:
        //     Region folding range.
        [EnumMember(Value = "region")]
        Region = 2
    }
}
