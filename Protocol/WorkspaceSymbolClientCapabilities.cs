using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Protocol
{
	[DataContract]
	public class WorkspaceSymbolClientCapabilities
    {
		public WorkspaceSymbolClientCapabilities() { }

		/**
		 * Symbol request supports dynamic registration.
		 */
		[DataMember(Name = "dynamicRegistration")]
		[JsonProperty(Required = Required.Default)]
		public bool DynamicRegistration { get; set; }

		/**
		 * Specific capabilities for the `SymbolKind` in the `workspace/symbol` request.
		 */
		[DataMember(Name = "symbolKind")]
		[JsonProperty(Required = Required.Default)]
		public _WorkspaceSymbolClientCapabilities_SymbolKind SymbolKind { get; set; }
	}
}