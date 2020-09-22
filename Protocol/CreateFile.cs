using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Protocol
{
	[DataContract]
	public class CreateFile
    {
		public CreateFile() { }

		// A create
		// Must always be 'create'.
		[DataMember(Name = "kind")]
		[JsonProperty(Required = Required.Always)]
		public string Kind { get; set; }

		// The resource to create.
		[DataMember(Name = "uri")]
		[JsonProperty(Required = Required.Always)]
		public string Uri { get; set; }

		// Additional options
		[DataMember(Name = "options")]
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public CreateFileOptions Options { get; set; }
    }
}
