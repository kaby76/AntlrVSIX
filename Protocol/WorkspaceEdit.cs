using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing a request sent from a language server to modify resources
    //     in the workspace.
    [DataContract]
    public class WorkspaceEdit
    {
        public WorkspaceEdit() { }

        /**
         * Holds changes to existing resources.
         */
        [DataMember(Name = "changes")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, TextEdit[]> Changes { get; set; }

        /**
	     * Depending on the client capability `workspace.workspaceEdit.resourceOperations` document changes
	     * are either an array of `TextDocumentEdit`s to express changes to n different text documents
	     * where each text document edit addresses a specific version of a text document. Or it can contain
	     * above `TextDocumentEdit`s mixed with create, rename and delete file / folder operations.
	     *
	     * Whether a client supports versioned document edits is expressed via
	     * `workspace.workspaceEdit.documentChanges` client capability.
	     *
	     * If a client neither supports `documentChanges` nor `workspace.workspaceEdit.resourceOperations` then
	     * only plain `TextEdit`s using the `changes` property are supported.
	     */
        [DataMember(Name = "documentChanges")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>[] DocumentChanges { get; set; }
    }
}
