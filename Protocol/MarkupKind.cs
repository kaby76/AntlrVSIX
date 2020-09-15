using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Enum which represents the various formats of markup text.
    [DataContract]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MarkupKind
    {
        //
        // Summary:
        //     Markup type is plain text.
        PlainText = 0,
        //
        // Summary:
        //     Markup type is Markdown.
        Markdown = 1
    }
}
