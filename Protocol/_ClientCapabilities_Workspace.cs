using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Protocol
{
    [DataContract]
    public class _ClientCapabilities_Workspace
    {
        public _ClientCapabilities_Workspace() { }

        /**
		 * The client supports applying batch edits
		 * to the workspace by supporting the request
		 * 'workspace/applyEdit'
		 */
        [DataMember(Name = "applyEdit")]
        [JsonProperty(Required = Required.Default)]
        public bool ApplyEdit { get; set; }

        /**
	 	 * Capabilities specific to `WorkspaceEdit`s
		 */
        [DataMember(Name = "workspaceEdit")]
        [JsonProperty(Required = Required.Default)]
        public WorkspaceEditClientCapabilities WorkspaceEdit { get; set; }

        /**
		 * Capabilities specific to the `workspace/didChangeConfiguration` notification.
		 */
        [DataMember(Name = "didChangeConfiguration")]
        [JsonProperty(Required = Required.Default)]
        public DidChangeConfigurationClientCapabilities DidChangeConfiguration { get; set; }

        /**
		 * Capabilities specific to the `workspace/didChangeWatchedFiles` notification.
		 */
        [DataMember(Name = "didChangeWatchedFiles")]
        [JsonProperty(Required = Required.Default)]
        public DidChangeWatchedFilesClientCapabilities DidChangeWatchedFiles { get; set; }

        /**
		 * Capabilities specific to the `workspace/symbol` request.
		 */
        [DataMember(Name = "symbol")]
        [JsonProperty(Required = Required.Default)]
        public WorkspaceSymbolClientCapabilities Symbol { get; set; }

        /**
		 * Capabilities specific to the `workspace/executeCommand` request.
		 */
        [DataMember(Name = "executeCommand")]
        [JsonProperty(Required = Required.Default)]
        public ExecuteCommandClientCapabilities ExecuteCommand { get; set; }

        /**
		 * The client has support for workspace folders.
		 *
		 * Since 3.6.0
		 */
        [DataMember(Name = "workspaceFolders")]
        [JsonProperty(Required = Required.Default)]
        public bool WorkspaceFolders { get; set; }

        /**
		 * The client supports `workspace/configuration` requests.
		 *
		 * Since 3.6.0
		 */
        [DataMember(Name = "configuration")]
        [JsonProperty(Required = Required.Default)]
        public bool Configuration { get; set; }
    }
}