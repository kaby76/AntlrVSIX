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
        QuickFix = 0,
        //
        // Summary:
        //     Code action is a refactor
        Refactor = 1,
        //
        // Summary:
        //     Code action is a refactor for extracting methods, functions, variables, etc.
        RefactorExtract = 2,
        //
        // Summary:
        //     Code action is a refactor for inlining methods, constants, etc.
        RefactorInline = 3,
        //
        // Summary:
        //     Code action is a refactor for rewrite actions, such as making methods static.
        RefactorRewrite = 4,
        //
        // Summary:
        //     Code action applies to the entire file.
        Source = 5,
        //
        // Summary:
        //     Code actions is for organizing imports.
        SourceOrganizeImports = 6
    }
}
