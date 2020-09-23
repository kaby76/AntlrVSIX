using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Protocol
{
    [DataContract]
    public class ApplyWorkspaceEditResponse
    {
        public ApplyWorkspaceEditResponse() { }

        /**
	     * Indicates whether the edit was applied or not.
	     */
        [DataMember(Name = "applied")]
        [JsonProperty(Required = Required.Always)]
        public bool Applied { get; set; }

        /**
	     * An optional textual description for why the edit was not applied.
	     * This may be used may be used by the server for diagnostic
	     * logging or to provide a suitable error for a request that
	     * triggered the edit.
	     */
        [DataMember(Name = "failureReason")]
        [JsonProperty(Required = Required.Default)]
        public string FailureReason { get; set; }
    }
}
