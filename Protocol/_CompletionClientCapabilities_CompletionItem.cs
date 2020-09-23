using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Protocol
{
    [DataContract]
    public class _CompletionClientCapabilities_CompletionItem
    {
		public _CompletionClientCapabilities_CompletionItem() { }

		/**
		 * Client supports snippets as insert text.
		 *
		 * A snippet can define tab stops and placeholders with `$1`, `$2`
		 * and `${3:foo}`. `$0` defines the final tab stop, it defaults to
		 * the end of the snippet. Placeholders with equal identifiers are linked,
		 * that is typing in one will update others too.
		 */
		[DataMember(Name = "snippetSupport")]
		[JsonProperty(Required = Required.Default)]
		public bool SnippetSupport { get; set; }

		/**
		 * Client supports commit characters on a completion item.
		 */
		[DataMember(Name = "commitCharactersSupport")]
		[JsonProperty(Required = Required.Default)]
		public bool CommitCharactersSupport { get; set; }

		/**
		 * Client supports the follow content formats for the documentation
		 * property. The order describes the preferred format of the client.
		 */
		[DataMember(Name = "documentationFormat")]
		[JsonProperty(Required = Required.Default)]
		public MarkupKind[] DocumentationFormat { get; set; }

		/**
		 * Client supports the deprecated property on a completion item.
		 */
		[DataMember(Name = "deprecatedSupport")]
		[JsonProperty(Required = Required.Default)]
		public bool DeprecatedSupport { get; set; }

		/**
		 * Client supports the preselect property on a completion item.
		 */
		[DataMember(Name = "preselectSupport")]
		[JsonProperty(Required = Required.Default)]
		public bool PreselectSupport { get; set; }

		/**
		 * Client supports the tag property on a completion item. Clients supporting
		 * tags have to handle unknown tags gracefully. Clients especially need to
		 * preserve unknown tags when sending a completion item back to the server in
		 * a resolve call.
		 *
		 * @since 3.15.0
		 */
		[DataMember(Name = "tagSupport")]
		[JsonProperty(Required = Required.Default)]
		public _CompletionClientCapabilities_CompletionItem_TagSupport TagSupport { get; set; }
	}
}