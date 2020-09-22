using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Protocol
{
    // The kind of resource operations supported by the client.
    [DataContract]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum FailureHandlingKind
    {
		// Applying the workspace change is simply aborted if one of the changes provided
		// fails. All operations executed before the failing operation stay executed.
		[EnumMember(Value = "abort")]
		Abort = 0,

		// All operations are executed transactional. That means they either all
		// succeed or no changes at all are applied to the workspace.
		[EnumMember(Value = "transactional")]
		Transactional = 1,

		// If the workspace edit contains only textual file changes they are executed transactional.
		// If resource changes (create, rename or delete file) are part of the change the failure
		// handling strategy is abort.
		[EnumMember(Value = "textOnlyTransactional")]
		TextOnlyTransactional = 2,

		// The client tries to undo the operations already executed. But there is no
		// guarantee that this is succeeding.
		[EnumMember(Value = "undo")]
		Undo = 3,
	}
}