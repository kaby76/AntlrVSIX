using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Protocol
{
    [DataContract]
    public class RenameFile
    {
        public RenameFile() { }

		// A rename
		// Must always be 'rename'.
		[DataMember(Name = "kind")]
		[JsonProperty(Required = Required.Always)]
		public string Kind { get; set; }

		// The old (existing) location.
		[DataMember(Name = "oldUri")]
		[JsonProperty(Required = Required.Always)]
		public string OldUri { get; set; }

		// The new location.
		[DataMember(Name = "newUri")]
		[JsonProperty(Required = Required.Always)]
		public string NewUri { get; set; }

		// Rename options.
		[DataMember(Name = "options")]
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public RenameFileOptions Options { get; set; }

	}
}
