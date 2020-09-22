using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    [DataContract]
    public class PartialResultParams
    {
        public PartialResultParams() { }

        /**
	     * An optional token that a server can use to report partial results (e.g. streaming) to
	     * the client.
	     */
        [DataMember(Name = "partialResultToken")]
        [JsonProperty(Required = Required.Default)]
        public SumType<int, string> PartialResultToken { get; set; }
    }
}
