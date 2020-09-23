using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Protocol
{
    /**
     * The parameters of a Workspace Symbol Request.
     */
    [DataContract]
    public class WorkspaceSymbolParams : WorkDoneProgressParams, IPartialResultParams
    {
        public WorkspaceSymbolParams() { }

        /**
	     * A query string to filter symbols by. Clients may send an empty
	     * string here to request all symbols.
	     */
        [DataMember(Name = "query")]
        public string Query { get; set; }

        /**
	     * An optional token that a server can use to report partial results (e.g. streaming) to
	     * the client.
	     */
        [DataMember(Name = "partialResultToken")]
        [JsonProperty(Required = Required.Default)]
        public SumType<int, string> PartialResultToken { get; set; }
    }
}
