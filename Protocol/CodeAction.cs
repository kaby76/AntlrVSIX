using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     A class representing a change that can be performed in code. A CodeAction must
    //     either set Microsoft.VisualStudio.LanguageServer.Protocol.CodeAction.Edit or
    //     Microsoft.VisualStudio.LanguageServer.Protocol.CodeAction.Command. If both are
    //     supplied, the edit will be applied first, then the command will be executed.
    [DataContract]
    public class CodeAction
    {
        public CodeAction() { }

        //
        // Summary:
        //     Gets or sets the human readable title for this code action.
        [DataMember(Name = "title")]
        public string Title { get; set; }
        //
        // Summary:
        //     Gets or sets the kind of code action this instance represents.
        [DataMember(Name = "kind")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CodeActionKind? Kind { get; set; }
        //
        // Summary:
        //     Gets or sets the diagnostics that this code action resolves.
        [DataMember(Name = "diagnostics")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Diagnostic[] Diagnostics { get; set; }
        //
        // Summary:
        //     Gets or sets the workspace edit that this code action performs.
        [DataMember(Name = "edit")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public WorkspaceEdit Edit { get; set; }
        //
        // Summary:
        //     Gets or sets the command that this code action executes.
        [DataMember(Name = "command")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Command Command { get; set; }
    }
}
