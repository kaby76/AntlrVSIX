using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Protocol
{
    //
    // Summary:
    //     Enum which represents language server trace setting.
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TraceSetting
    {
        //
        // Summary:
        //     Setting for 'off'.
        Off = 0,
        //
        // Summary:
        //     Setting for 'messages'
        Messages = 1,
        //
        // Summary:
        //     Setting for 'verbose'
        Verbose = 2
    }
}
