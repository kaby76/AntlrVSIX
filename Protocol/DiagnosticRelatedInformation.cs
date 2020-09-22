using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Protocol
{
    [DataContract]
    public class DiagnosticRelatedInformation
    {
        public DiagnosticRelatedInformation() { }

		//
		// Summary:
		// The location of this related diagnostic information.
		[DataMember(Name = "location")]
		[JsonProperty(Required = Required.Always)]
		public Location Location { get; set; }

		//
		// Summary:
		// The message of this related diagnostic information.
		[DataMember(Name = "message")]
		[JsonProperty(Required = Required.Always)]
		public string Message { get; set; }
    }
}
