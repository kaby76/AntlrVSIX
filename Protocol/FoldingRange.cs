using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing a folding range in a document.
    [DataContract]
    public class FoldingRange
    {
        public FoldingRange() { }

        //
        // Summary:
        //     Gets or sets the start line value.
        [DataMember(Name = "startLine")]
        public int StartLine { get; set; }
        //
        // Summary:
        //     Gets or sets the start character value.
        [DataMember(Name = "startCharacter")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? StartCharacter { get; set; }
        //
        // Summary:
        //     Gets or sets the end line value.
        [DataMember(Name = "endLine")]
        public int EndLine { get; set; }
        //
        // Summary:
        //     Gets or sets the end character value.
        [DataMember(Name = "endCharacter")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? EndCharacter { get; set; }
        //
        // Summary:
        //     Gets or sets the folding range kind.
        [DataMember(Name = "kind")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public FoldingRangeKind? Kind { get; set; }
    }
}
