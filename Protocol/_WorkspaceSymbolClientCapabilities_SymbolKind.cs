using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Protocol
{
    [DataContract]
    public class _WorkspaceSymbolClientCapabilities_SymbolKind
	{
		public _WorkspaceSymbolClientCapabilities_SymbolKind() { }

		/**
		 * The symbol kind values the client supports. When this
		 * property exists the client also guarantees that it will
		 * handle values outside its set gracefully and falls back
		 * to a default value when unknown.
		 *
		 * If this property is not present the client only supports
		 * the symbol kinds from `File` to `Array` as defined in
		 * the initial version of the protocol.
		 */
		[DataMember(Name = "valueSet")]
		[JsonProperty(Required = Required.Default)]
		public SymbolKind[] ValueSet { get; set; }
	}
}