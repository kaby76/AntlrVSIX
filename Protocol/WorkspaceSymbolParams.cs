using System;
using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents the parameter that's sent with the 'workspace/symbol'
    //     request.
    [DataContract]
    public class WorkspaceSymbolParams
    {
        public WorkspaceSymbolParams() { }

        //
        // Summary:
        //     Gets or sets the query (a non-empty string).
        [DataMember(Name = "query")]
        public string Query { get; set; }
        //
        // Summary:
        //     Gets or sets the value of the Progress instance.
        [DataMember(Name = "partialResultToken", IsRequired = false)]
        public IProgress<SymbolInformation[]> PartialResultToken { get; set; }
    }
}
