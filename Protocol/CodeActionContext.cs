using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing diagnostic information about the context of a code action
    [DataContract]
    public class CodeActionContext
    {
        public CodeActionContext() { }

        //
        // Summary:
        //     Gets or sets an array of diagnostics relevant to a code action
        [DataMember(Name = "diagnostics")]
        public Diagnostic[] Diagnostics { get; set; }
        //
        // Summary:
        //     Gets or sets an array of code action kinds to filter for.
        [DataMember(Name = "only")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CodeActionKind[] Only { get; set; }
    }
}
