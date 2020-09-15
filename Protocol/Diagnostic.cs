using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents a source code diagnostic message.
    [DataContract]
    public class Diagnostic
    {
        public Diagnostic() { }

        //
        // Summary:
        //     Gets or sets the source code range.
        [DataMember(Name = "range")]
        public Range Range { get; set; }
        //
        // Summary:
        //     Gets or sets the diagnostic severity.
        [DataMember(Name = "severity")]
        public DiagnosticSeverity Severity { get; set; }
        //
        // Summary:
        //     Gets or sets the diagnostic's code.
        [DataMember(Name = "code")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }
        //
        // Summary:
        //     Gets or sets the source of this diagnostic.
        [DataMember(Name = "source")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Source { get; set; }
        //
        // Summary:
        //     Gets or sets the diagnostic's message.
        [DataMember(Name = "message")]
        public string Message { get; set; }
        //
        // Summary:
        //     Gets or sets the diagnostic's tags.
        [DataMember(Name = "tags")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DiagnosticTag[] Tags { get; set; }
    }
}
