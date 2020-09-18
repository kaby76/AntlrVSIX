using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Enum which represents the various kinds of code actions.
    [DataContract]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CodeActionKind
    {
        //
        // Summary:
        //     Code action is a quick fix.
        [EnumMember(Value = "quickfix")]
        QuickFix = 0,
        //
        // Summary:
        //     Code action is a refactor
        [EnumMember(Value = "refactor")]
        Refactor = 1,
        //
        // Summary:
        //     Code action is a refactor for extracting methods, functions, variables, etc.
        [EnumMember(Value = "refactor.extract")]
        RefactorExtract = 2,
        //
        // Summary:
        //     Code action is a refactor for inlining methods, constants, etc.
        [EnumMember(Value = "refactor.inline")]
        RefactorInline = 3,
        //
        // Summary:
        //     Code action is a refactor for rewrite actions, such as making methods static.
        [EnumMember(Value = "refactor.rewrite")]
        RefactorRewrite = 4,
        //
        // Summary:
        //     Code action applies to the entire file.
        [EnumMember(Value = "source")]
        Source = 5,
        //
        // Summary:
        //     Code actions is for organizing imports.
        [EnumMember(Value = "source.organizeImports")]
        SourceOrganizeImports = 6
    }
}
