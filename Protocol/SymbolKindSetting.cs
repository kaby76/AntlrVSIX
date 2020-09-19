using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the symbol kind setting in initialization.
    [DataContract]
    public class SymbolKindSetting
    {
        public SymbolKindSetting() { }

        //
        // Summary:
        //     Gets or sets the types of symbol kind the client supports.
        [DataMember(Name = "valueSet")]
        public SymbolKind[] ValueSet { get; set; }
    }
}
