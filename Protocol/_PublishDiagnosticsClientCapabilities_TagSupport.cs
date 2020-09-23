using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Protocol
{
	[DataContract]
	public class _PublishDiagnosticsClientCapabilities_TagSupport
    {
		public _PublishDiagnosticsClientCapabilities_TagSupport() { }

		/**
		 * The tags supported by the client.
		 */
		[DataMember(Name = "valueSet")]
		[JsonProperty(Required = Required.Always)]
		public DiagnosticTag[] ValueSet { get; set; }
	}
}