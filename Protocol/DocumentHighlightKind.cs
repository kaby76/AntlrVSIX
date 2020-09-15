using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Enum representing the different types of document highlight.
    [DataContract]
    public enum DocumentHighlightKind
    {
        //
        // Summary:
        //     A textual occurance.
        Text = 1,
        //
        // Summary:
        //     Read access of a symbol.
        Read = 2,
        //
        // Summary:
        //     Write access of a symbol.
        Write = 3
    }
}
