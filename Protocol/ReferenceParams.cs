using System;
using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing find reference parameter for find reference request.
    [DataContract]
    public class ReferenceParams : TextDocumentPositionParams
    {
        public ReferenceParams() { }

        //
        // Summary:
        //     Gets or sets the reference context.
        [DataMember(Name = "context")]
        public ReferenceContext Context { get; set; }
        //
        // Summary:
        //     Gets or sets the value of the PartialResultToken instance.
        [DataMember(Name = "partialResultToken", IsRequired = false)]
        public IProgress<object> PartialResultToken { get; set; }
    }
}
