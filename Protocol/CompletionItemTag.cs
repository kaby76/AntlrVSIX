using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Enum which represents the various ways in which completion can be triggered.
    [DataContract]
    public enum CompletionItemTag
    {
        /**
	     * Render a completion as obsolete, usually using a strike-out.
	     */
        Deprecated = 1,
    }
}
