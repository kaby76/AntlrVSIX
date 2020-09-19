using System;
using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Enum representing the type of changes to watch for.
    [DataContract]
    [Flags]
    public enum WatchKind
    {
        //
        // Summary:
        //     Create events.
        Create = 1,
        //
        // Summary:
        //     Change events.
        Change = 2,
        //
        // Summary:
        //     Delete events.
        Delete = 4
    }
}
