using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Enum which represents the various ways in which completion can be triggered.
    [DataContract]
    public enum CompletionTriggerKind
    {
        //
        // Summary:
        //     Completion was triggered by typing an identifier.
        Invoked = 1,
        //
        // Summary:
        //     Completion was triggered by typing a trigger character.
        TriggerCharacter = 2
    }
}
