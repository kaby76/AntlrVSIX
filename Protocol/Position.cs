using System;
using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents a position on a text document.
    [DataContract]
    public class Position : IEquatable<Position>
    {
        //
        // Summary:
        //     Initializes a new instance of the Microsoft.VisualStudio.LanguageServer.Protocol.Position
        //     class.
        public Position() { }
        //
        // Summary:
        //     Initializes a new instance of the Microsoft.VisualStudio.LanguageServer.Protocol.Position
        //     class.
        //
        // Parameters:
        //   line:
        //     Line number.
        //
        //   character:
        //     Character number.
        public Position(int line, int character) { throw new NotImplementedException(); }

        //
        // Summary:
        //     Gets or sets the line number.
        [DataMember(Name = "line")]
        public int Line { get; set; }
        //
        // Summary:
        //     Gets or sets the character number.
        [DataMember(Name = "character")]
        public int Character { get; set; }

        //
        // Summary:
        //     Overrides base class method System.Object.Equals(System.Object). Two positions
        //     are equal if their line and character are the same.
        //
        // Parameters:
        //   obj:
        //     Object to compare to.
        //
        // Returns:
        //     True if the given position has the same line and character; false otherwise.
        public override bool Equals(object obj) { throw new NotImplementedException(); }
        public bool Equals(Position other) { throw new NotImplementedException(); }
        //
        // Summary:
        //     Overrides base class method System.Object.GetHashCode
        //
        // Returns:
        //     Hashcode for this object.
        public override int GetHashCode() { throw new NotImplementedException(); }

        //
        // Summary:
        //     Overrides default equals operator. Two positions are equal if they are both null
        //     or one of them is the object equivalent of the other.
        //
        // Parameters:
        //   firstPosition:
        //     The first position to compare.
        //
        //   secondPosition:
        //     The second position to compare.
        //
        // Returns:
        //     True if both positions are null or one of them is the object equivalent of the
        //     other, false otherwise.
        public static bool operator ==(Position firstPosition, Position secondPosition) { throw new NotImplementedException(); }
        //
        // Summary:
        //     Overrides the default not equals operator.
        //
        // Parameters:
        //   firstPosition:
        //     The first position to compare.
        //
        //   secondPosition:
        //     The second position to compare.
        //
        // Returns:
        //     True if first and second positions are not equivalent.
        public static bool operator !=(Position firstPosition, Position secondPosition) { throw new NotImplementedException(); }
    }
}
