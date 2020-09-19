using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the registration options for indication code lens support.
    [DataContract]
    public class CodeLensRegistrationOptions : TextDocumentRegistrationOptions
    {
        public CodeLensRegistrationOptions() { }

        //
        // Summary:
        //     Gets or sets a value indicating whether resolve is supported as well.
        [DataMember(Name = "resolveProvider")]
        public bool ResolveProvider { get; set; }
    }
}
