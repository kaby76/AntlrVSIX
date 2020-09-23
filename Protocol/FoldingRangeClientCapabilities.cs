using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    [DataContract]
    public class FoldingRangeClientCapabilities
    {
        public FoldingRangeClientCapabilities() { }

        /**
	     * Whether implementation supports dynamic registration for folding range providers. If this is set to `true`
	     * the client supports the new `FoldingRangeRegistrationOptions` return value for the corresponding server
	     * capability as well.
	     */
        [DataMember(Name = "dynamicRegistration")]
        [JsonProperty(Required = Required.Default)]
        public bool DynamicRegistration { get; set; }

        /**
	     * The maximum number of folding ranges that the client prefers to receive per document. The value serves as a
	     * hint, servers are free to follow the limit.
	     */
        [DataMember(Name = "rangeLimit")]
        [JsonProperty(Required = Required.Default)]
        public int RangeLimit { get; set; }

        /**
         * If set, the client signals that it only supports folding complete lines. If set, client will
         * ignore specified `startCharacter` and `endCharacter` properties in a FoldingRange.
         */
        [DataMember(Name = "lineFoldingOnly")]
        [JsonProperty(Required = Required.Default)]
        public bool LineFoldingOnly { get; set; }
    }
}