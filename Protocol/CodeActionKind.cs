using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Protocol
{
    [DataContract]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CodeActionKind
    {
        /**
	     * Empty kind.
	     */
        [EnumMember(Value = "")]
        Empty = 0,

        /**
	     * Base kind for quickfix actions: 'quickfix'.
	     */
        [EnumMember(Value = "quickfix")]
        QuickFix = 1,

        /**
	     * Base kind for refactoring actions: 'refactor'.
	     */
        [EnumMember(Value = "refactor")]
        Refactor = 2,

        /**
	     * Base kind for refactoring extraction actions: 'refactor.extract'.
	     *
	     * Example extract actions:
	     *
	     * - Extract method
	     * - Extract function
	     * - Extract variable
	     * - Extract interface from class
	     * - ...
	     */
        [EnumMember(Value = "refactor.extract")]
        RefactorExtract = 3,

        /**
	     * Base kind for refactoring inline actions: 'refactor.inline'.
	     *
	     * Example inline actions:
	     *
	     * - Inline function
	     * - Inline variable
	     * - Inline constant
	     * - ...
	     */
        [EnumMember(Value = "refactor.inline")]
        RefactorInline = 4,

		/**
		 * Base kind for refactoring rewrite actions: 'refactor.rewrite'.
		 *
		 * Example rewrite actions:
		 *
		 * - Convert JavaScript function to class
		 * - Add or remove parameter
		 * - Encapsulate field
		 * - Make method static
		 * - Move method to base class
		 * - ...
		 */
		[EnumMember(Value = "refactor.rewrite")]
        RefactorRewrite = 5,

		/**
		 * Base kind for source actions: `source`.
		 *
		 * Source code actions apply to the entire file.
		 */
		[EnumMember(Value = "source")]
        Source = 6,

		/**
		 * Base kind for an organize imports source action: `source.organizeImports`.
		 */
		[EnumMember(Value = "source.organizeImports")]
        SourceOrganizeImports = 7,
    }
}
