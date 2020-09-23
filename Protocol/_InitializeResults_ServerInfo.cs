using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    [DataContract]
    public class _InitializeResults_ServerInfo
    {
        public _InitializeResults_ServerInfo() { }

		/**
		 * The name of the server as defined by the server.
		 */
		[DataMember(Name = "name")]
		[JsonProperty(Required = Required.Always)]
		public string Name { get; set; }

		/**
		 * The server's version as defined by the server.
		 */
		[DataMember(Name = "version")]
		[JsonProperty(Required = Required.Default)]
		public string Version { get; set; }
    }
}
