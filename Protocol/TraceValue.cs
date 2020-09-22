using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Enum which represents language server trace setting.
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TraceValue
    {
        //
        // Summary:
        //     Setting for 'off'.
        [EnumMember(Value = "off")]
        Off = 0,
        //
        // Summary:
        //     Setting for 'message'
        [EnumMember(Value = "message")]
        Messages = 1,
        //
        // Summary:
        //     Setting for 'verbose'
        [EnumMember(Value = "verbose")]
        Verbose = 2
    }
}
