using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
	[DataContract]
	public class WorkspaceEditClientCapabilities
	{
		// The client supports versioned document changes in `WorkspaceEdit`s
		[DataMember(Name = "documentChanges")]
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public bool DocumentChanges { get; set; }

		// The resource operations the client supports. Clients should at least
		// support 'create', 'rename' and 'delete' files and folders.
		//
		// @since 3.13.0
		[DataMember(Name = "resourceOperations")]
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public ResourceOperationKind[] ResourceOperations;

		// The failure handling strategy of a client if applying the workspace edit
		// fails.
		//
		// @since 3.13.0
		[DataMember(Name = "failureHandling")]
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public FailureHandlingKind[] FailureHandling;
	}
}