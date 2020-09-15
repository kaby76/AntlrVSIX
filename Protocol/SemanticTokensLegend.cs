using System.Runtime.Serialization;

namespace Protocol
{
    [DataContract]
    public class SemanticTokensLegend
    {
        /**
         * The token types a server uses.
         */
        [DataMember(Name = "tokenTypes")]
        public string[] tokenTypes { get; set; }

        /**
         * The token modifiers a server uses.
         */
        [DataMember(Name = "tokenModifiers")]
        public string[] tokenModifiers { get; set; }
    }
}
