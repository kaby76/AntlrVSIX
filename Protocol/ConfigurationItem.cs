using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
	[DataContract]
	public class ConfigurationItem
	{
		public ConfigurationItem() { }

		/**
		 * The scope to get the configuration section for.
		 */
		[DataMember(Name = "scopeUri")]
		[JsonProperty(Required = Required.Default)]
		public string ScopeUri { get; set; }

		/**
		 * The configuration section asked for.
		 */
		[DataMember(Name = "section")]
		[JsonProperty(Required = Required.Default)]
		public string Section { get; set; }
	}
}