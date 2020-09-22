using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    [DataContract]
    public class DocumentFilter
    {
        public DocumentFilter() { }

        /**
         * A language id, like `typescript`.
         */
        [DataMember(Name = "language")]
        [JsonProperty(Required = Required.Default)]
        string language { get; set; }

        /**
	     * A Uri [scheme](#Uri.scheme), like `file` or `untitled`.
	     */
        [DataMember(Name = "scheme")]
        [JsonProperty(Required = Required.Default)]
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
        [DataMember(Name = "pattern")]
        [JsonProperty(Required = Required.Default)]
        string pattern { get; set; }
    }
}
