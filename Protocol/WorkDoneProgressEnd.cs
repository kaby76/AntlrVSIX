using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    [DataContract]
    public class WorkDoneProgressEnd
    {
        public WorkDoneProgressEnd() { }

        [DataMember(Name = "kind")]
        [JsonProperty(Required = Required.Always)]
        public string Kind { get; set; }

        /**
	     * Optional, a final message indicating to for example indicate the outcome
	     * of the operation.
         */
        [DataMember(Name = "message")]
        [JsonProperty(Required = Required.Default)]
        public string Message { get; set; }
    }
}
