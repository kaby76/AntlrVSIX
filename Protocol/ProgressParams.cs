using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    [DataContract]
    public class ProgressParams<T>
    {
        public ProgressParams() { }

        /**
	     * The progress token provided by the client or server.
	     */
        [DataMember(Name = "token")]
        [JsonProperty(Required = Required.Always)]
        public SumType<string, int> Token { get; set; }

        /**
	     * The progress data.
	     */
        [DataMember(Name = "value")]
        [JsonProperty(Required = Required.Always)]
        public T Value { get; set; }
    }
}
