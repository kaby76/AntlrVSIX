using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing reference context information for find reference request parameter.
    [DataContract]
    public class ReferenceContext
    {
        public ReferenceContext() { }

        //
        // Summary:
        //     Gets or sets a value indicating whether declaration should be included.
        [DataMember(Name = "includeDeclaration")]
        public bool IncludeDeclaration { get; set; }
    }
}
