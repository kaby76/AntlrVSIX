using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents client capabilities.
    [DataContract]
    public class ClientCapabilities
    {
        public ClientCapabilities() { }

        /**
	     * Workspace specific client capabilities.
	     */
        [DataMember(Name = "workspace")]
        [JsonProperty(Required = Required.Default)]
        public _WorkspaceClientCapabilities Workspace { get; set; }

        /**
	     * Text document specific client capabilities.
	     */
        [DataMember(Name = "textDocument")]
        [JsonProperty(Required = Required.Default)]
        public TextDocumentClientCapabilities TextDocument { get; set; }

        /**
	     * Window specific client capabilities.
	     */
        [DataMember(Name = "experimental")]
        [JsonProperty(Required = Required.Default)]
        public _WindowClientCapabilities Window { get; set; }
     
        /**
	     * Experimental client capabilities.
	     */
        [DataMember(Name = "experimental")]
        [JsonProperty(Required = Required.Default)]
        public object Experimental { get; set; }
    }
}
