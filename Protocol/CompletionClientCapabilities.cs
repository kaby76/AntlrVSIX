using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Protocol
{
    [DataContract]
    public class CompletionClientCapabilities
    {
		public CompletionClientCapabilities() { }

		/**
		 * Whether completion supports dynamic registration.
		 */
		[DataMember(Name = "dynamicRegistration")]
		[JsonProperty(Required = Required.Default)]
		public bool DynamicRegistration { get; set; }

		/**
		 * The client supports the following `CompletionItem` specific
		 * capabilities.
		 */
		[DataMember(Name = "completionItem")]
		[JsonProperty(Required = Required.Default)]
		public _CompletionClientCapabilities_CompletionItem completionItem { get; set; }
		
		[DataMember(Name = "completionItemKind")]
		[JsonProperty(Required = Required.Default)]
		public _CompletionClientCapabilities_CompletionItemKind completionItemKind { get; set; }

		/**
		 * The client supports to send additional context information for a
		 * `textDocument/completion` request.
		 */
		[DataMember(Name = "contextSupport")]
		[JsonProperty(Required = Required.Default)]
		public bool ContextSupport { get; set; }
	}
}
