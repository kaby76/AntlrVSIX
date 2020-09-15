using System;
using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing information about programming constructs like variables, classes,
    //     interfaces, etc.
    [DataContract]
    public class SymbolInformation : IEquatable<SymbolInformation>
    {
        public SymbolInformation() { }

        //
        // Summary:
        //     Gets or sets the name of this symbol.
        [DataMember(Name = "name")]
        public string Name { get; set; }
        //
        // Summary:
        //     Gets or sets the Microsoft.VisualStudio.LanguageServer.Protocol.SymbolKind of
        //     this symbol.
        [DataMember(Name = "kind")]
        public SymbolKind Kind { get; set; }
        //
        // Summary:
        //     Gets or sets the Microsoft.VisualStudio.LanguageServer.Protocol.Location of this
        //     symbol.
        [DataMember(Name = "location")]
        public Location Location { get; set; }
        //
        // Summary:
        //     Gets or sets the name of the symbol containing this symbol.
        [DataMember(Name = "containerName")]
        public string ContainerName { get; set; }

        public override bool Equals(object obj) { throw new NotImplementedException(); }
        public bool Equals(SymbolInformation other) { throw new NotImplementedException(); }
        public override int GetHashCode() { throw new NotImplementedException(); }
    }
}
