using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Enum which represents the various ways in which completion can be triggered.
    [DataContract]
    public enum SignatureHelpTriggerKind
    {
        //
        // Summary:
        //     Signature help was invoked manually by the user or a command.
        Invoked = 1,
        //
        // Summary:
        //     Signature help was triggered by a trigger character.
        TriggerCharacter = 2,
        //
        // Summary:
        //     Signature help was triggered by the cursor moving or by the document content
        //     changing.
        ContentChange = 3
    }
}
