using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    [DataContract]
    public class InitializeResult
    {
        public InitializeResult() { }

        /**
	     * The capabilities the language server provides.
	     */
        [DataMember(Name = "capabilities")]
        public ServerCapabilities Capabilities { get; set; }

        /**
	     * Information about the server.
	     *
	     * @since 3.15.0
	     */
        [DataMember(Name = "serverInfo")]
        [JsonProperty(Required = Required.Default)]
        public _InitializeResults_ServerInfo ServerInfo { get; set; }
    }
}