using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    [DataContract]
    public class WorkDoneProgressParams
    {
        public WorkDoneProgressParams() { }

        /**
	     * An optional token that a server can use to report work done progress.
	     */
        [DataMember(Name = "workDoneToken")]
        [JsonProperty(Required = Required.Default)]
        public SumType<string, int> WorkDoneToken { get; set; }
    }
}
