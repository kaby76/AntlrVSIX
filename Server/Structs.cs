using System.Runtime.Serialization;


namespace Server
{

    [DataContract]
    public class SemanticTokensOptions : WorkDoneProgressOptions
    {
        [DataMember(Name = "legend")]
        public SemanticTokensLegend legend { get; set; }

        [DataMember(Name = "range")]
        public bool range { get; set; }

        [DataMember(Name = "full")] 
        public bool full { get; set; }
    }

    [DataContract]
    public class WorkDoneProgressOptions
    {
        [DataMember(Name = "workDoneProgress")]
        public bool WorkDoneProgress { get; set; }
    }

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

    [DataContract]
    public class SemanticTokensRegistrationOptions : TextDocumentRegistrationOptions
    {
    }

    [DataContract]
    public class TextDocumentRegistrationOptions
    {
        /**
         * A document selector to identify the scope of the registration. If set to null
         * the document selector provided on the client side will be used.
         */
        public DocumentFilter[] documentSelector { get; set; }
    }

    [DataContract]
    public class DocumentFilter
    {
        /**
         * A language id, like `typescript`.
         */
        string language { get; set; }

	    /**
	     * A Uri [scheme](#Uri.scheme), like `file` or `untitled`.
	     */
	    string scheme { get; set; }

        /**
         * A glob pattern, like `*.{ts,js}`.
         *
         * Glob patterns can have the following syntax:
         * - `*` to match one or more characters in a path segment
         * - `?` to match on one character in a path segment
         * - `**` to match any number of path segments, including none
         * - `{}` to group conditions (e.g. `**​/*.{ts,js}` matches all TypeScript and JavaScript files)
         * - `[]` to declare a range of characters to match in a path segment (e.g., `example.[0-9]` to match on `example.0`, `example.1`, …)
         * - `[!...]` to negate a range of characters to match in a path segment (e.g., `example.[!0-9]` to match on `example.a`, `example.b`, but not `example.0`)
         */
        string pattern { get; set; }
    }

    /**
     * Static registration options to be returned in the initialize request.
     */
    [DataContract]
    public class StaticRegistrationOptions
    {
        /**
         * The id used to register the request. The id can be used to deregister
         * the request again. See also Registration#id.
         */
        string id { get; set; }
    }


    [DataContract]
    public class SemanticTokens
    {
        /**
	     * An optional result id. If provided and clients support delta updating
	     * the client will include the result id in the next semantic token request.
	     * A server can then instead of computing all semantic tokens again simply
	     * send a delta.
	     */
        [DataMember(Name = "resultId")]
        public string resultId { get; set; }

        [DataMember(Name = "data")]
        public int[] data { get; set; }
    }
}
