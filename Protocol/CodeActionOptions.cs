using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the registration options for code actions support.
    [DataContract]
    public class CodeActionOptions
    {
        public CodeActionOptions() { }

        //
        // Summary:
        //     Gets or sets the kinds of code action that this server may return.
        //
        // Remarks:
        //     The list of kinds may be generic, such as `CodeActionKind.Refactor`, or the server
        //     may list out every specific kind they provide.
        [DataMember(Name = "codeActionKinds")]
        public CodeActionKind[] CodeActionKinds { get; set; }
    }
}
