using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents the parameter sent with an initialize method request.
    [DataContract]
    public class InitializeParams
    {
        public InitializeParams() { }

        //
        // Summary:
        //     Gets or sets the ID of the process which launched the language server.
        [DataMember(Name = "processId")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? ProcessId { get; set; }
        //
        // Summary:
        //     Gets or sets the workspace root path.
        [DataMember(Name = "rootPath")]
        public string RootPath { get; set; }
        //
        // Summary:
        //     Gets or sets the workspace root path.
        //
        // Remarks:
        //     This should be a string representation of an URI.
        [DataMember(Name = "rootUri")]
        [JsonConverter(typeof(DocumentUriConverter))]
        public Uri RootUri { get; set; }
        //
        // Summary:
        //     Gets or sets the capabilities supported by the client.
        [DataMember(Name = "capabilities")]
        public ClientCapabilities Capabilities { get; set; }
        //
        // Summary:
        //     Gets or sets the initialization options as specified by the client.
        [DataMember(Name = "initializationOptions")]
        public object InitializationOptions { get; set; }
        //
        // Summary:
        //     Gets or sets the initial trace setting.
        [DataMember(Name = "trace")]
        public TraceSetting Trace { get; set; }
    }
}
