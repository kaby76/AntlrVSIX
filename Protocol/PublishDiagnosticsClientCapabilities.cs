using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Protocol
{
    [DataContract]
    public class PublishDiagnosticsClientCapabilities
    {
        public PublishDiagnosticsClientCapabilities() { }

		/**
		 * Whether the clients accepts diagnostics with related information.
		 */
		[DataMember(Name = "relatedInformation")]
		[JsonProperty(Required = Required.Default)]
		public bool RelatedInformation { get; set; }

		/**
		 * Client supports the tag property to provide meta data about a diagnostic.
		 * Clients supporting tags have to handle unknown tags gracefully.
		 *
		 * @since 3.15.0
		 */
		[DataMember(Name = "tagSupport")]
		[JsonProperty(Required = Required.Default)]
		//TODO public _PublishDiagnosticsClientCapabilities_TagSupport TagSupport { get; set; }
		public SumType<bool,_PublishDiagnosticsClientCapabilities_TagSupport> TagSupport { get; set; }

		/**
		 * Whether the client interprets the version property of the
		 * `textDocument/publishDiagnostics` notification's parameter.
		 *
		 * @since 3.15.0
		 */
		[DataMember(Name = "versionSupport")]
		[JsonProperty(Required = Required.Default)]
		public bool VersionSupport { get; set; }
    }
}