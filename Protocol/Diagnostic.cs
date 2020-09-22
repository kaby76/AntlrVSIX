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
        // The range at which the message applies.
        [DataMember(Name = "range")]
        [JsonProperty(Required = Required.Always)]
        public Range Range { get; set; }
        //
        // Summary:
        // The diagnostic's severity. Can be omitted. If omitted it is up to the
        // client to interpret diagnostics as error, warning, info or hint.
        [DataMember(Name = "severity")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DiagnosticSeverity Severity { get; set; }
        //
        // Summary:
        // The diagnostic's code, which might appear in the user interface.
        [DataMember(Name = "code")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public SumType<string, int> Code { get; set; }
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
        [JsonProperty(Required = Required.Always)]
        public string Message { get; set; }
        //
        // Summary:
        //     Gets or sets the diagnostic's tags.
        [DataMember(Name = "tags")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DiagnosticTag[] Tags { get; set; }

        //
        // Summary:
        // An array of related diagnostic information, e.g. when symbol-names within
        // a scope collide all definitions can be marked via this property.
        [DataMember(Name = "relatedInformation")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DiagnosticRelatedInformation[] RelatedInformation { get; set; }
    }
}
