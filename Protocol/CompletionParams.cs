using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the parameters for the textDocument/completion request.
    [DataContract]
    public class CompletionParams : TextDocumentPositionParams
    {
        public CompletionParams() { }

        //
        // Summary:
        //     Gets or sets the completion context.
        [DataMember(Name = "context")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CompletionContext Context { get; set; }
        //
        // Summary:
        //     Gets or sets the value of the PartialResultToken instance.
        [DataMember(Name = "partialResultToken", IsRequired = false)]
        public IProgress<SumType<CompletionItem[], CompletionList>?> PartialResultToken { get; set; }
    }
}
