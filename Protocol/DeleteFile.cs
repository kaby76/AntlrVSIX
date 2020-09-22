using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Protocol
{
	// Delete file operation
	[DataContract]
    public class DeleteFile
    {
		public DeleteFile() { }

		// A delete
		// Must always be 'delete'.
		[DataMember(Name = "kind")]
		[JsonProperty(Required = Required.Always)]
		public string Kind { get; set; }

		// The file to delete.
		[DataMember(Name = "uri")]
		[JsonProperty(Required = Required.Always)]
		public string Uri { get; set; }

		// Delete options.
		[DataMember(Name = "options")]
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public DeleteFileOptions Options { get; set; }
	}
}
