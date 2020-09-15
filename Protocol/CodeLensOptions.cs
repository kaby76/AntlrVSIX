using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the options for code lens support.
    [DataContract]
    public class CodeLensOptions
    {
        public CodeLensOptions() { }

        //
        // Summary:
        //     Gets or sets a value indicating whether or not the code lens support has a resolve
        //     provider.
        [DataMember(Name = "resolveProvider")]
        public bool ResolveProvider { get; set; }
    }
}
