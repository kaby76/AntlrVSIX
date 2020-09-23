using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Protocol
{
    [DataContract]
    public enum SymbolTag
    {
        /**
	     * Render a symbol as obsolete, usually using a strike-out.
	     */
        Deprecated = 1,
    }
}