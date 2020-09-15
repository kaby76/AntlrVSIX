using System;
using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents a text document text range.
    [DataContract]
    public class Range : IEquatable<Range>
    {
        public Range() { }

        //
        // Summary:
        //     Gets or sets the text start position.
        [DataMember(Name = "start")]
        public Position Start { get; set; }
        //
        // Summary:
        //     Gets or sets the text end position.
        [DataMember(Name = "end")]
        public Position End { get; set; }

        public override bool Equals(object obj) { throw new NotImplementedException(); }
        public bool Equals(Range other) { throw new NotImplementedException(); }
        public override int GetHashCode() { throw new NotImplementedException(); }
    }
}
