using Microsoft.VisualStudio.LanguageServer.Protocol;
using Newtonsoft.Json.Linq;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Workspaces;

namespace LanguageServer.Exec
{
    public class LanguageServerTarget
    {
        private readonly LSPServer server;
        private bool trace = true;
        private readonly Workspaces.Workspace _workspace;

        public LanguageServerTarget(LSPServer server)
        {
            this.server = server;
            this._workspace = new Workspace();
        }

        [JsonRpcMethod(Methods.InitializeName)]
        public object Initialize(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- Initialize");
                System.Console.Error.WriteLine(arg.ToString());
            }
            var capabilities = new ServerCapabilities();
            capabilities.TextDocumentSync = new TextDocumentSyncOptions();
            capabilities.TextDocumentSync.OpenClose = true;
            capabilities.TextDocumentSync.Change = TextDocumentSyncKind.Incremental;
            capabilities.TextDocumentSync.Save = new SaveOptions();
            capabilities.TextDocumentSync.Save.IncludeText = true;

            capabilities.HoverProvider = true;

            capabilities.CompletionProvider = new CompletionOptions();
            capabilities.CompletionProvider.ResolveProvider = false;
            capabilities.CompletionProvider.TriggerCharacters = new string[] { ",", "." };

            capabilities.DefinitionProvider = true;

            capabilities.TypeDefinitionProvider = true;

            capabilities.ImplementationProvider = true;

            capabilities.DocumentHighlightProvider = true;

            capabilities.DocumentSymbolProvider = true;

            capabilities.WorkspaceSymbolProvider = false;

            capabilities.DocumentFormattingProvider = true;

            capabilities.DocumentRangeFormattingProvider = false;

            capabilities.RenameProvider = true;

            var result = new InitializeResult();
            result.Capabilities = capabilities;
            string json = JsonConvert.SerializeObject(result);
            if (trace)
            {
                System.Console.Error.WriteLine("--> " + json);
            }
            return result;
        }

        [JsonRpcMethod(Methods.InitializedName)]
        public async void InitializedName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- Initialized");
                System.Console.Error.WriteLine(arg.ToString());
            }
        }

        [JsonRpcMethod(Methods.ShutdownName)]
        public async System.Threading.Tasks.Task<JToken> ShutdownName()
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- Shutdown");
            }
            return null;
        }

        [JsonRpcMethod(Methods.ExitName)]
        public async void ExitName()
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- Exit");
            }
            server.Exit();
        }

        // ======= WINDOW ========

        [JsonRpcMethod(Methods.WorkspaceDidChangeConfigurationName)]
        public async void WorkspaceDidChangeConfigurationName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- WorkspaceDidChangeConfiguration");
                System.Console.Error.WriteLine(arg.ToString());
            }
            //var parameter = arg.ToObject<DidChangeConfigurationParams>();
            //this.server.SendSettings(parameter);
        }

        [JsonRpcMethod(Methods.WorkspaceDidChangeWatchedFilesName)]
        public async void WorkspaceDidChangeWatchedFilesName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- WorkspaceDidChangeWatchedFiles");
                System.Console.Error.WriteLine(arg.ToString());
            }
        }

        [JsonRpcMethod(Methods.WorkspaceSymbolName)]
        public async System.Threading.Tasks.Task<JToken> WorkspaceSymbolName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- WorkspaceSymbol");
                System.Console.Error.WriteLine(arg.ToString());
            }
            return null;
        }

        [JsonRpcMethod(Methods.WorkspaceExecuteCommandName)]
        public async System.Threading.Tasks.Task<JToken> WorkspaceExecuteCommandName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- WorkspaceExecuteCommand");
                System.Console.Error.WriteLine(arg.ToString());
            }
            return null;
        }

        [JsonRpcMethod(Methods.WorkspaceApplyEditName)]
        public async System.Threading.Tasks.Task<JToken> WorkspaceApplyEditName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- WorkspaceApplyEdit");
                System.Console.Error.WriteLine(arg.ToString());
            }
            return null;
        }

        // ======= TEXT SYNCHRONIZATION ========

        [JsonRpcMethod(Methods.TextDocumentDidOpenName)]
        public async void TextDocumentDidOpenName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- TextDocumentDidOpen");
                System.Console.Error.WriteLine(arg.ToString());
            }
            var request = arg.ToObject<DidOpenTextDocumentParams>();
            var document = _workspace.FindDocument(request.TextDocument.Uri.AbsolutePath);
            if (document == null)
            {
                document = new Workspaces.Document(request.TextDocument.Uri.AbsolutePath,
                    request.TextDocument.Uri.AbsolutePath);
                document.Code = request.TextDocument.Text;
                var project = _workspace.FindProject("Misc");
                if (project == null)
                {
                    project = new Project("Misc", "Misc", "Misc");
                    _workspace.AddChild(project);
                }
                project.AddDocument(document);
            }
            server.SendDiagnostics(request.TextDocument.Uri.AbsoluteUri, "");
        }

        [JsonRpcMethod(Methods.TextDocumentDidChangeName)]
        public async void TextDocumentDidChangeName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- TextDocumentDidChange");
                System.Console.Error.WriteLine(arg.ToString());
            }
            //var parameter = arg.ToObject<DidChangeTextDocumentParams>();
            //server.SendDiagnostics(parameter.TextDocument.Uri.ToString(), parameter.ContentChanges[0].Text);
        }

        [JsonRpcMethod(Methods.TextDocumentWillSaveName)]
        public async void TextDocumentWillSaveName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- TextDocumentWillSave");
                System.Console.Error.WriteLine(arg.ToString());
            }
        }

        [JsonRpcMethod(Methods.TextDocumentWillSaveWaitUntilName)]
        public async System.Threading.Tasks.Task<JToken> TextDocumentWillSaveWaitUntilName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- TextDocumentWillSaveWaitUntil");
                System.Console.Error.WriteLine(arg.ToString());
            }
            return null;
        }

        [JsonRpcMethod(Methods.TextDocumentDidSaveName)]
        public async void TextDocumentDidSaveName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- TextDocumentDidSave");
                System.Console.Error.WriteLine(arg.ToString());
            }
        }

        [JsonRpcMethod(Methods.TextDocumentDidCloseName)]
        public async void TextDocumentDidCloseName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- TextDocumentDidClose");
                System.Console.Error.WriteLine(arg.ToString());
            }
        }

        // ======= DIAGNOSTICS ========

        [JsonRpcMethod(Methods.TextDocumentPublishDiagnosticsName)]
        public async void TextDocumentPublishDiagnosticsName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- TextDocumentPublishDiagnostics");
                System.Console.Error.WriteLine(arg.ToString());
            }
        }

        // ======= LANGUAGE FEATURES ========

        [JsonRpcMethod(Methods.TextDocumentCompletionName)]
        public async System.Threading.Tasks.Task<JToken> TextDocumentCompletionName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- TextDocumentCompletion");
                System.Console.Error.WriteLine(arg.ToString());
            }
            //List<CompletionItem> items = new List<CompletionItem>();
            //for (int i = 0; i < 10; i++)
            //{
            //    var item = new CompletionItem();
            //    item.Label = "Item " + i;
            //    item.InsertText = "Item" + i;
            //    item.Kind = (CompletionItemKind)(i % (Enum.GetNames(typeof(CompletionItemKind)).Length) + 1);
            //    items.Add(item);
            //}
            //return items.ToArray();
            return null;
        }

        [JsonRpcMethod(Methods.TextDocumentCompletionResolveName)]
        public async System.Threading.Tasks.Task<JToken> TextDocumentCompletionResolveName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- TextDocumentCompletionResolve");
                System.Console.Error.WriteLine(arg.ToString());
            }
            return null;
        }

        [JsonRpcMethod(Methods.TextDocumentHoverName)]
        public async System.Threading.Tasks.Task<JToken> TextDocumentHoverName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- TextDocumentHover");
                System.Console.Error.WriteLine(arg.ToString());
            }
            return null;
        }

        [JsonRpcMethod(Methods.TextDocumentSignatureHelpName)]
        public async System.Threading.Tasks.Task<JToken> TextDocumentSignatureHelpName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- TextDocumentSignatureHelp");
                System.Console.Error.WriteLine(arg.ToString());
            }
            return null;
        }

        [JsonRpcMethod(Methods.TextDocumentDefinitionName)]
        public async System.Threading.Tasks.Task<JToken> TextDocumentDefinitionName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- TextDocumentDefinition");
                System.Console.Error.WriteLine(arg.ToString());
            }
            return null;
        }

        [JsonRpcMethod(Methods.TextDocumentTypeDefinitionName)]
        public async System.Threading.Tasks.Task<JToken> TextDocumentTypeDefinitionName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- TextDocumentTypeDefinition");
                System.Console.Error.WriteLine(arg.ToString());
            }
            return null;
        }

        [JsonRpcMethod(Methods.TextDocumentImplementationName)]
        public async System.Threading.Tasks.Task<JToken> TextDocumentImplementationName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- TextDocumentImplementation");
                System.Console.Error.WriteLine(arg.ToString());
            }
            return null;
        }

        [JsonRpcMethod(Methods.TextDocumentReferencesName)]
        public async System.Threading.Tasks.Task<JToken> TextDocumentReferencesName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- TextDocumentReferences");
                System.Console.Error.WriteLine(arg.ToString());
            }
            return null;
        }

        [JsonRpcMethod(Methods.TextDocumentDocumentHighlightName)]
        public async System.Threading.Tasks.Task<JToken> TextDocumentDocumentHighlightName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- TextDocumentDocumentHighlight");
                System.Console.Error.WriteLine(arg.ToString());
            }
            return null;
        }

        [JsonRpcMethod(Methods.TextDocumentDocumentSymbolName)]
        public async System.Threading.Tasks.Task<object[]> TextDocumentDocumentSymbolName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- TextDocumentDocumentSymbol");
                System.Console.Error.WriteLine(arg.ToString());
            }
            var request = arg.ToObject<DocumentSymbolParams>();
            var document = _workspace.FindDocument(request.TextDocument.Uri.AbsolutePath);
            var r = LanguageServer.Module.Get(document);
            var symbols = new List<object>();
            foreach (var s in r)
            {
                var si = new SymbolInformation();
                si.Name = s.name;
                si.Kind = (SymbolKind) si.Kind;
                si.Location = new Microsoft.VisualStudio.LanguageServer.Protocol.Location();
                si.Location.Uri = request.TextDocument.Uri;
                var lcs = LanguageServer.Module.GetLineColumn(s.range.Start.Value, document);
                var lce = LanguageServer.Module.GetLineColumn(s.range.End.Value, document);
                si.Location.Range = new Microsoft.VisualStudio.LanguageServer.Protocol.Range();
                si.Location.Range.Start.Line = lcs.Item1;
                si.Location.Range.Start.Character = lcs.Item2;
                si.Location.Range.End.Line = lce.Item1;
                si.Location.Range.End.Character = lce.Item2;
                symbols.Add(si);
            }
            var result = symbols.ToArray();
            return result;
        }

        [JsonRpcMethod(Methods.TextDocumentCodeActionName)]
        public async System.Threading.Tasks.Task<JToken> TextDocumentCodeActionName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- TextDocumentCodeAction");
                System.Console.Error.WriteLine(arg.ToString());
            }
            return null;
        }

        [JsonRpcMethod(Methods.TextDocumentCodeLensName)]
        public async System.Threading.Tasks.Task<JToken> TextDocumentCodeLensName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- TextDocumentCodeLens");
                System.Console.Error.WriteLine(arg.ToString());
            }
            return null;
        }

        [JsonRpcMethod(Methods.CodeLensResolveName)]
        public async System.Threading.Tasks.Task<JToken> CodeLensResolveName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- CodeLensResolve");
                System.Console.Error.WriteLine(arg.ToString());
            }
            return null;
        }

        [JsonRpcMethod(Methods.TextDocumentDocumentLinkName)]
        public async System.Threading.Tasks.Task<JToken> TextDocumentDocumentLinkName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- TextDocumentDocumentLink");
                System.Console.Error.WriteLine(arg.ToString());
            }
            return null;
        }

        [JsonRpcMethod(Methods.DocumentLinkResolveName)]
        public async System.Threading.Tasks.Task<JToken> DocumentLinkResolveName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- DocumentLinkResolve");
                System.Console.Error.WriteLine(arg.ToString());
            }
            return null;
        }

        [JsonRpcMethod(Methods.TextDocumentFormattingName)]
        public async System.Threading.Tasks.Task<JToken> TextDocumentFormattingName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- TextDocumentFormatting");
                System.Console.Error.WriteLine(arg.ToString());
            }
            return null;
        }

        [JsonRpcMethod(Methods.TextDocumentRangeFormattingName)]
        public async System.Threading.Tasks.Task<JToken> TextDocumentRangeFormattingName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- TextDocumentRangeFormatting");
                System.Console.Error.WriteLine(arg.ToString());
            }
            return null;
        }

        [JsonRpcMethod(Methods.TextDocumentOnTypeFormattingName)]
        public async System.Threading.Tasks.Task<JToken> TextDocumentOnTypeFormattingName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- TextDocumentOnTypeFormatting");
                System.Console.Error.WriteLine(arg.ToString());
            }
            return null;
        }

        [JsonRpcMethod(Methods.TextDocumentRenameName)]
        public async System.Threading.Tasks.Task<JToken> TextDocumentRenameName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- TextDocumentRename");
                System.Console.Error.WriteLine(arg.ToString());
            }
            return null;
        }

        [JsonRpcMethod(Methods.TextDocumentFoldingRangeName)]
        public async System.Threading.Tasks.Task<JToken> TextDocumentFoldingRangeName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- TextDocumentFoldingRange");
                System.Console.Error.WriteLine(arg.ToString());
            }
            return null;
        }


        public string GetText()
        {
            return string.IsNullOrWhiteSpace(this.server.CustomText) ? "custom text from language server target" : this.server.CustomText;
        }
    }
}
