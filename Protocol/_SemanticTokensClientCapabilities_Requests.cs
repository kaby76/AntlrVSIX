using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
	[DataContract]
	public class _SemanticTokensClientCapabilities_Requests
    {
		public _SemanticTokensClientCapabilities_Requests() { }

		/**
		 * The client will send the `textDocument/semanticTokens/range` request if
		 * the server provides a corresponding handler.
		 */
		[DataMember(Name = "range")]
		[JsonProperty(Required = Required.Default)]
		public SumType<bool, object> Range { get; set; }

		/**
		 * The client will send the `textDocument/semanticTokens/full` request if
		 * the server provides a corresponding handler.
		 */
		[DataMember(Name = "full")]
		[JsonProperty(Required = Required.Default)]
		public SumType<bool, object> Full { get; set; }
	}
}