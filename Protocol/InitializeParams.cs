using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents the parameter sent with an initialize method request.
    [DataContract]
    public class InitializeParams : WorkDoneProgressParams
    {
        public InitializeParams() { }

        /**
	     * The process Id of the parent process that started
	     * the server. Is null if the process has not been started by another process.
	     * If the parent process is not alive then the server should exit (see exit notification) its process.
	     */
        [DataMember(Name = "processId")]
        [JsonProperty(Required = Required.Always)]
        public int? ProcessId { get; set; }

        /**
	     * Information about the client
	     *
	     * @since 3.15.0
	     */
        [DataMember(Name = "clientInfo")]
        [JsonProperty(Required = Required.Default)]
        public ClientInfo ClientInfo { get; set; }

        /**
	     * The rootPath of the workspace. Is null
	     * if no folder is open.
	     *
	     * @deprecated in favour of rootUri.
	     */
        [DataMember(Name = "rootPath")]
        [JsonProperty(Required = Required.Default)]
        public string RootPath { get; set; }

        /**
	     * The rootUri of the workspace. Is null if no
	     * folder is open. If both `rootPath` and `rootUri` are set
	     * `rootUri` wins.
	     */
        [DataMember(Name = "rootUri")]
        [JsonConverter(typeof(DocumentUriConverter))]
        [JsonProperty(Required = Required.Always)]
        public Uri RootUri { get; set; }

        /**
	     * User provided initialization options.
	     */
        [DataMember(Name = "initializationOptions")]
        [JsonProperty(Required = Required.Default)]
        public object InitializationOptions { get; set; }

        /**
	     * The capabilities provided by the client (editor or tool)
	     */
        [DataMember(Name = "capabilities")]
        public ClientCapabilities Capabilities { get; set; }

        /**
	     * The initial trace setting. If omitted trace is disabled ('off').
	     */
        [DataMember(Name = "trace")]
        [JsonProperty(Required = Required.Default)]
        public TraceValue Trace { get; set; }

        /**
	     * The workspace folders configured in the client when the server starts.
	     * This property is only available if the client supports workspace folders.
	     * It can be `null` if the client supports workspace folders but none are
	     * configured.
	     *
	     * @since 3.6.0
	     */
        [DataMember(Name = "workspaceFolders")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public WorkspaceFolder[] WorkspaceFolders { get; set; }
    }
}
