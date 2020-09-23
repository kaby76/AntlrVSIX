using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Protocol
{
	[DataContract]
	public class _ClientCapabilities_Window
    {
		public _ClientCapabilities_Window() { }

		/**
		 * Whether client supports handling progress notifications. If set servers are allowed to
		 * report in `workDoneProgress` property in the request specific server capabilities.
		 *
		 * Since 3.15.0
		 */
		[DataMember(Name = "workDoneProgress")]
		[JsonProperty(Required = Required.Default)]
		public bool WorkDoneProgress { get; set; }
	}
}