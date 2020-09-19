using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the folding range setting for initialization.
    [DataContract]
    public class FoldingRangeSetting : DynamicRegistrationSetting
    {
        public FoldingRangeSetting() { }

        //
        // Summary:
        //     Gets or sets the range limit for folding ranges.
        [DataMember(Name = "rangeLimit")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? RangeLimit { get; set; }
        //
        // Summary:
        //     Gets or sets a value indicating whether if client only supports entire line folding
        //     only.
        [DataMember(Name = "lineFoldingOnly")]
        public bool LineFoldingOnly { get; set; }
    }
}
