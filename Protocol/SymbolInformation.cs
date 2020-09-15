using System;
using System.Collections.Generic;
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

        public override bool Equals(object obj) { return this.Equals(obj as SymbolInformation); }
        public bool Equals(SymbolInformation other)
        {
            return other != null && this.Name == other.Name && (this.Kind == other.Kind && EqualityComparer<Location>.Default.Equals(this.Location, other.Location)) && this.ContainerName == other.ContainerName;
        }
        public override int GetHashCode()
        {
            return (int)((((long)1633890234 * (long)-1521134295
                + EqualityComparer<string>.Default.GetHashCode(this.Name)) * -1521134295 
                + this.Kind.GetHashCode()) * -1521134295
                + EqualityComparer<Location>.Default.GetHashCode(this.Location)) * -1521134295 
                + EqualityComparer<string>.Default.GetHashCode(this.ContainerName);
        }
    }
}
