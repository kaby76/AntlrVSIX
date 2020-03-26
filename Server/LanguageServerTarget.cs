namespace Server
{
    using LanguageServer;
    using Microsoft.VisualStudio.LanguageServer.Protocol;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using StreamJsonRpc;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Workspaces;
    using DocumentSymbol = LanguageServer.DocumentSymbol;
    using Location = LanguageServer.Location;

    public class LanguageServerTarget
    {
        private readonly LSPServer server;
        private readonly bool trace = true;
        private readonly Workspaces.Workspace _workspace;
        private static readonly object _object = new object();

        public LanguageServerTarget(LSPServer server)
        {
            this.server = server;
            _workspace = Workspaces.Workspace.Instance;
        }

        [JsonRpcMethod(Methods.InitializeName)]
        public object Initialize(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- Initialize");
                System.Console.Error.WriteLine(arg.ToString());
            }
            ServerCapabilities capabilities = new ServerCapabilities
            {
                TextDocumentSync = new TextDocumentSyncOptions
                {
                    OpenClose = true,
                    Change = TextDocumentSyncKind.Incremental,
                    Save = new SaveOptions
                    {
                        IncludeText = true
                    }
                },

                HoverProvider = true,

                CompletionProvider = new CompletionOptions
                {
                    ResolveProvider = true,
                    TriggerCharacters = new string[] { ",", "." }
                },

                ReferencesProvider = true,

                DefinitionProvider = true,

                TypeDefinitionProvider = false, // Does not make sense for Antlr.

                ImplementationProvider = false, // Does not make sense for Antlr.

                DocumentHighlightProvider = true,

                DocumentSymbolProvider = true,

                WorkspaceSymbolProvider = false,

                DocumentFormattingProvider = true,

                DocumentRangeFormattingProvider = false,

                RenameProvider = true
            };

            InitializeResult result = new InitializeResult
            {
                Capabilities = capabilities
            };
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
            try
            {
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- Initialized");
                    System.Console.Error.WriteLine(arg.ToString());
                }
            }
            catch (Exception)
            { }
        }

        [JsonRpcMethod(Methods.ShutdownName)]
        public async System.Threading.Tasks.Task<JToken> ShutdownName()
        {
            try
            {
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- Shutdown");
                }
            }
            catch (Exception)
            { }
            return null;
        }

        [JsonRpcMethod(Methods.ExitName)]
        public async void ExitName()
        {
            try
            {
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- Exit");
                }
                server.Exit();
            }
            catch (Exception)
            { }
        }

        // ======= WINDOW ========

        [JsonRpcMethod(Methods.WorkspaceDidChangeConfigurationName)]
        public async void WorkspaceDidChangeConfigurationName(JToken arg)
        {
            try
            {
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- WorkspaceDidChangeConfiguration");
                    System.Console.Error.WriteLine(arg.ToString());
                }
                //var parameter = arg.ToObject<DidChangeConfigurationParams>();
                //this.server.SendSettings(parameter);
            }
            catch (Exception)
            { }
        }

        [JsonRpcMethod(Methods.WorkspaceDidChangeWatchedFilesName)]
        public async void WorkspaceDidChangeWatchedFilesName(JToken arg)
        {
            try
            {
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- WorkspaceDidChangeWatchedFiles");
                    System.Console.Error.WriteLine(arg.ToString());
                }
            }
            catch (Exception)
            { }
        }

        [JsonRpcMethod(Methods.WorkspaceSymbolName)]
        public async System.Threading.Tasks.Task<JToken> WorkspaceSymbolName(JToken arg)
        {
            try
            {
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- WorkspaceSymbol");
                    System.Console.Error.WriteLine(arg.ToString());
                }
            }
            catch (Exception)
            { }
            return null;
        }

        [JsonRpcMethod(Methods.WorkspaceExecuteCommandName)]
        public async System.Threading.Tasks.Task<JToken> WorkspaceExecuteCommandName(JToken arg)
        {
            try
            {
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- WorkspaceExecuteCommand");
                    System.Console.Error.WriteLine(arg.ToString());
                }
            }
            catch (Exception)
            { }
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
            DidOpenTextDocumentParams request = arg.ToObject<DidOpenTextDocumentParams>();
            Document document = CheckDoc(request.TextDocument.Uri);
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
            DidChangeTextDocumentParams request = arg.ToObject<DidChangeTextDocumentParams>();
            int? version = request.TextDocument.Version;
            Document document = CheckDoc(request.TextDocument.Uri);
            lock (_object)
            {
                ParserDetails pd = ParserDetailsFactory.Create(document);
                string code = pd.Code;
                int start_index = 0;
                int end_index = 0;
                foreach (TextDocumentContentChangeEvent change in request.ContentChanges)
                {
                    Microsoft.VisualStudio.LanguageServer.Protocol.Range range = change.Range;
                    int length = change.RangeLength; // Why? range encodes start and end => length!
                    string text = change.Text;
                    {
                        int line = range.Start.Line;
                        int character = range.Start.Character;
                        start_index = LanguageServer.Module.GetIndex(line, character, document);
                    }
                    {
                        int line = range.End.Line;
                        int character = range.End.Character;
                        end_index = LanguageServer.Module.GetIndex(line, character, document);
                    }
                    (int, int) bs = LanguageServer.Module.GetLineColumn(start_index, document);
                    (int, int) be = LanguageServer.Module.GetLineColumn(end_index, document);
                    string original = code.Substring(start_index, end_index - start_index);
                    string n = code.Substring(0, start_index)
                            + text
                            + code.Substring(0 + start_index + end_index - start_index);
                    code = n;
                }
                document.Code = code;
                List<ParserDetails> to_do = LanguageServer.Module.Compile();
            }
        }

        [JsonRpcMethod(Methods.TextDocumentWillSaveName)]
        public async void TextDocumentWillSaveName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- TextDocumentWillSave");
                System.Console.Error.WriteLine(arg.ToString());
            }
            // Nothing to do--who cares because the server does not perform a save.
        }

        [JsonRpcMethod(Methods.TextDocumentWillSaveWaitUntilName)]
        public async System.Threading.Tasks.Task<JToken> TextDocumentWillSaveWaitUntilName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- TextDocumentWillSaveWaitUntil");
                System.Console.Error.WriteLine(arg.ToString());
            }
            // Nothing to do--who cares because the server does not perform a save, and
            // the server doesn't manufacture edit requests out of thin air.
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
            // Nothing to do--who cares because the server does not perform a save.
        }

        [JsonRpcMethod(Methods.TextDocumentDidCloseName)]
        public async void TextDocumentDidCloseName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- TextDocumentDidClose");
                System.Console.Error.WriteLine(arg.ToString());
            }
            // Nothing to do--who cares.
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
        public async System.Threading.Tasks.Task<object[]> TextDocumentCompletionName(JToken arg)
        {
            try
            {
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- TextDocumentCompletion");
                    System.Console.Error.WriteLine(arg.ToString());
                }
                CompletionParams request = arg.ToObject<CompletionParams>();
                Document document = CheckDoc(request.TextDocument.Uri);
                CompletionContext context = request.Context;
                Position position = request.Position;
                int line = position.Line;
                int character = position.Character;
                int char_index = LanguageServer.Module.GetIndex(line, character, document);
                if (trace)
                {
                    System.Console.Error.WriteLine("position index = " + char_index);
                    (int, int) back = LanguageServer.Module.GetLineColumn(char_index, document);
                    System.Console.Error.WriteLine("back to l,c = " + back.Item1 + "," + back.Item2);
                }
                List<string> res = LanguageServer.Module.Completion(char_index, document);
                List<CompletionItem> items = new List<CompletionItem>();
                foreach (string r in res)
                {
                    CompletionItem item = new CompletionItem
                    {
                        Label = r,
                        InsertText = r,
                        Kind = CompletionItemKind.Variable
                    };
                    items.Add(item);
                }
                return items.ToArray();
            }
            catch (Exception)
            {
            }
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

        private Document CheckDoc(System.Uri uri)
        {
            string decoded = System.Web.HttpUtility.UrlDecode(uri.AbsoluteUri);
            string file_name = new Uri(decoded).LocalPath;
            Document document = _workspace.FindDocument(file_name);
            if (document == null)
            {
                document = new Workspaces.Document(file_name);
                try
                {   // Open the text file using a stream reader.
                    using (StreamReader sr = new StreamReader(file_name))
                    {
                        // Read the stream to a string, and write the string to the console.
                        string str = sr.ReadToEnd();
                        document.Code = str;
                    }
                }
                catch (IOException)
                {
                }
                Project project = _workspace.FindProject("Misc");
                if (project == null)
                {
                    project = new Project("Misc", "Misc", "Misc");
                    _workspace.AddChild(project);
                }
                project.AddDocument(document);
                document.Changed = true;
                ParserDetails pd = ParserDetailsFactory.Create(document);
                List<ParserDetails> to_do = LanguageServer.Module.Compile();
            }
            return document;
        }

        [JsonRpcMethod(Methods.TextDocumentHoverName)]
        public async System.Threading.Tasks.Task<object> TextDocumentHoverName(JToken arg)
        {
            Hover hover = null;
            try
            {
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- TextDocumentHover");
                    System.Console.Error.WriteLine(arg.ToString());
                }
                TextDocumentPositionParams request = arg.ToObject<TextDocumentPositionParams>();
                Document document = CheckDoc(request.TextDocument.Uri);
                Position position = request.Position;
                int line = position.Line;
                int character = position.Character;
                int index = LanguageServer.Module.GetIndex(line, character, document);
                if (trace)
                {
                    System.Console.Error.WriteLine("position index = " + index);
                    (int, int) back = LanguageServer.Module.GetLineColumn(index, document);
                    System.Console.Error.WriteLine("back to l,c = " + back.Item1 + "," + back.Item2);
                }
                QuickInfo quick_info = LanguageServer.Module.GetQuickInfo(index, document);
                if (quick_info == null)
                {
                    return null;
                }

                hover = new Hover
                {
                    Contents = new MarkupContent
                    {
                        Kind = MarkupKind.PlainText,
                        Value = quick_info.Display
                    }
                };
                int index_start = quick_info.Range.Start.Value;
                int index_end = quick_info.Range.End.Value;
                (int, int) lcs = LanguageServer.Module.GetLineColumn(index_start, document);
                (int, int) lce = LanguageServer.Module.GetLineColumn(index_end, document);
                hover.Range = new Microsoft.VisualStudio.LanguageServer.Protocol.Range
                {
                    Start = new Position(lcs.Item1, lcs.Item2),
                    End = new Position(lce.Item1, lce.Item2)
                };
                System.Console.Error.WriteLine("returning " + quick_info.Display.ToString());
            }
            catch (Exception)
            { }
            return hover;
        }

        [JsonRpcMethod(Methods.TextDocumentSignatureHelpName)]
        public async System.Threading.Tasks.Task<JToken> TextDocumentSignatureHelpName(JToken arg)
        {
            try
            {
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- TextDocumentSignatureHelp");
                    System.Console.Error.WriteLine(arg.ToString());
                }
            }
            catch (Exception)
            { }
            return null;
        }

        // TextDocumentDeclaration does not exist in Microsoft.VisualStudio.LanguageServer.Protocol 16.3.57
        // but does in version 3.14 of LSP.

        [JsonRpcMethod(Methods.TextDocumentDefinitionName)]
        public async System.Threading.Tasks.Task<object[]> TextDocumentDefinitionName(JToken arg)
        {
            object[] result = null;
            try
            {
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- TextDocumentDefinition");
                    System.Console.Error.WriteLine(arg.ToString());
                }
                TextDocumentPositionParams request = arg.ToObject<TextDocumentPositionParams>();
                Document document = CheckDoc(request.TextDocument.Uri);
                Position position = request.Position;
                int line = position.Line;
                int character = position.Character;
                int index = LanguageServer.Module.GetIndex(line, character, document);
                if (trace)
                {
                    System.Console.Error.WriteLine("position index = " + index);
                    (int, int) back = LanguageServer.Module.GetLineColumn(index, document);
                    System.Console.Error.WriteLine("back to l,c = " + back.Item1 + "," + back.Item2);
                }
                IList<Location> found = LanguageServer.Module.FindDef(index, document);
                List<object> locations = new List<object>();
                foreach (Location f in found)
                {
                    Microsoft.VisualStudio.LanguageServer.Protocol.Location location = new Microsoft.VisualStudio.LanguageServer.Protocol.Location
                    {
                        Uri = new Uri(f.Uri.FullPath)
                    };
                    Document def_document = _workspace.FindDocument(f.Uri.FullPath);
                    location.Range = new Microsoft.VisualStudio.LanguageServer.Protocol.Range();
                    (int, int) lcs = LanguageServer.Module.GetLineColumn(f.Range.Start.Value, def_document);
                    (int, int) lce = LanguageServer.Module.GetLineColumn(f.Range.End.Value, def_document);
                    location.Range.Start = new Position(lcs.Item1, lcs.Item2);
                    location.Range.End = new Position(lce.Item1, lce.Item2);
                    locations.Add(location);
                }
                result = locations.ToArray();
            }
            catch (Exception)
            { }
            return result;
        }

        [JsonRpcMethod(Methods.TextDocumentTypeDefinitionName)]
        public async System.Threading.Tasks.Task<object[]> TextDocumentTypeDefinitionName(JToken arg)
        {
            object[] result = null;
            try
            {
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- TextDocumentTypeDefinitionName");
                    System.Console.Error.WriteLine(arg.ToString());
                }
                TextDocumentPositionParams request = arg.ToObject<TextDocumentPositionParams>();
                Document document = CheckDoc(request.TextDocument.Uri);
                Position position = request.Position;
                int line = position.Line;
                int character = position.Character;
                int index = LanguageServer.Module.GetIndex(line, character, document);
                if (trace)
                {
                    System.Console.Error.WriteLine("position index = " + index);
                    (int, int) back = LanguageServer.Module.GetLineColumn(index, document);
                    System.Console.Error.WriteLine("back to l,c = " + back.Item1 + "," + back.Item2);
                }
                IList<Location> found = LanguageServer.Module.FindDef(index, document);
                List<object> locations = new List<object>();
                foreach (Location f in found)
                {
                    Microsoft.VisualStudio.LanguageServer.Protocol.Location location = new Microsoft.VisualStudio.LanguageServer.Protocol.Location
                    {
                        Uri = new Uri(f.Uri.FullPath)
                    };
                    Document def_document = _workspace.FindDocument(f.Uri.FullPath);
                    location.Range = new Microsoft.VisualStudio.LanguageServer.Protocol.Range();
                    (int, int) lcs = LanguageServer.Module.GetLineColumn(f.Range.Start.Value, def_document);
                    (int, int) lce = LanguageServer.Module.GetLineColumn(f.Range.End.Value, def_document);
                    location.Range.Start = new Position(lcs.Item1, lcs.Item2);
                    location.Range.End = new Position(lce.Item1, lce.Item2);
                    locations.Add(location);
                }
                result = locations.ToArray();
            }
            catch (Exception)
            { }
            return result;
        }

        [JsonRpcMethod(Methods.TextDocumentImplementationName)]
        public async System.Threading.Tasks.Task<object[]> TextDocumentImplementationName(JToken arg)
        {
            object[] result = null;
            try
            {
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- TextDocumentImplementation");
                    System.Console.Error.WriteLine(arg.ToString());
                }
                TextDocumentPositionParams request = arg.ToObject<TextDocumentPositionParams>();
                Document document = CheckDoc(request.TextDocument.Uri);
                Position position = request.Position;
                int line = position.Line;
                int character = position.Character;
                int index = LanguageServer.Module.GetIndex(line, character, document);
                if (trace)
                {
                    System.Console.Error.WriteLine("position index = " + index);
                    (int, int) back = LanguageServer.Module.GetLineColumn(index, document);
                    System.Console.Error.WriteLine("back to l,c = " + back.Item1 + "," + back.Item2);
                }
                IList<Location> found = LanguageServer.Module.FindDef(index, document);
                List<object> locations = new List<object>();
                foreach (Location f in found)
                {
                    Microsoft.VisualStudio.LanguageServer.Protocol.Location location = new Microsoft.VisualStudio.LanguageServer.Protocol.Location
                    {
                        Uri = new Uri(f.Uri.FullPath)
                    };
                    Document def_document = _workspace.FindDocument(f.Uri.FullPath);
                    location.Range = new Microsoft.VisualStudio.LanguageServer.Protocol.Range();
                    (int, int) lcs = LanguageServer.Module.GetLineColumn(f.Range.Start.Value, def_document);
                    (int, int) lce = LanguageServer.Module.GetLineColumn(f.Range.End.Value, def_document);
                    location.Range.Start = new Position(lcs.Item1, lcs.Item2);
                    location.Range.End = new Position(lce.Item1, lce.Item2);
                    locations.Add(location);
                }
                result = locations.ToArray();
            }
            catch (Exception)
            { }
            return result;
        }

        [JsonRpcMethod(Methods.TextDocumentReferencesName)]
        public async System.Threading.Tasks.Task<object[]> TextDocumentReferencesName(JToken arg)
        {
            object[] result = null;
            try
            {
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- TextDocumentReferences");
                    System.Console.Error.WriteLine(arg.ToString());
                }
                TextDocumentPositionParams request = arg.ToObject<TextDocumentPositionParams>();
                Document document = CheckDoc(request.TextDocument.Uri);
                Position position = request.Position;
                int line = position.Line;
                int character = position.Character;
                int index = LanguageServer.Module.GetIndex(line, character, document);
                if (trace)
                {
                    System.Console.Error.WriteLine("position index = " + index);
                    (int, int) back = LanguageServer.Module.GetLineColumn(index, document);
                    System.Console.Error.WriteLine("back to l,c = " + back.Item1 + "," + back.Item2);
                }
                IEnumerable<Location> found = LanguageServer.Module.FindRefsAndDefs(index, document);
                List<object> locations = new List<object>();
                foreach (Location f in found)
                {
                    Microsoft.VisualStudio.LanguageServer.Protocol.Location location = new Microsoft.VisualStudio.LanguageServer.Protocol.Location
                    {
                        Uri = new Uri(f.Uri.FullPath)
                    };
                    Document def_document = _workspace.FindDocument(f.Uri.FullPath);
                    location.Range = new Microsoft.VisualStudio.LanguageServer.Protocol.Range();
                    (int, int) lcs = LanguageServer.Module.GetLineColumn(f.Range.Start.Value, def_document);
                    (int, int) lce = LanguageServer.Module.GetLineColumn(f.Range.End.Value + 1, def_document);
                    location.Range.Start = new Position(lcs.Item1, lcs.Item2);
                    location.Range.End = new Position(lce.Item1, lce.Item2);
                    locations.Add(location);
                }
                result = locations.ToArray();
            }
            catch (Exception)
            { }
            return result;
        }

        [JsonRpcMethod(Methods.TextDocumentDocumentHighlightName)]
        public async System.Threading.Tasks.Task<object[]> TextDocumentDocumentHighlightName(JToken arg)
        {
            object[] result = null;
            try
            {
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- TextDocumentDocumentHighlight");
                    System.Console.Error.WriteLine(arg.ToString());
                }
                TextDocumentPositionParams request = arg.ToObject<TextDocumentPositionParams>();
                Document document = CheckDoc(request.TextDocument.Uri);
                Position position = request.Position;
                int line = position.Line;
                int character = position.Character;
                int index = LanguageServer.Module.GetIndex(line, character, document);
                if (trace)
                {
                    System.Console.Error.WriteLine("position index = " + index);
                    (int, int) back = LanguageServer.Module.GetLineColumn(index, document);
                    System.Console.Error.WriteLine("back to l,c = " + back.Item1 + "," + back.Item2);
                }
                IEnumerable<Location> found = LanguageServer.Module.FindRefsAndDefs(index, document);
                List<object> locations = new List<object>();
                foreach (Location f in found)
                {
                    Microsoft.VisualStudio.LanguageServer.Protocol.Location location = new Microsoft.VisualStudio.LanguageServer.Protocol.Location
                    {
                        Uri = new Uri(f.Uri.FullPath)
                    };
                    Document def_document = _workspace.FindDocument(f.Uri.FullPath);
                    location.Range = new Microsoft.VisualStudio.LanguageServer.Protocol.Range();
                    (int, int) lcs = LanguageServer.Module.GetLineColumn(f.Range.Start.Value, def_document);
                    (int, int) lce = LanguageServer.Module.GetLineColumn(f.Range.End.Value + 1, def_document);
                    location.Range.Start = new Position(lcs.Item1, lcs.Item2);
                    location.Range.End = new Position(lce.Item1, lce.Item2);
                    locations.Add(location);
                }
                result = locations.ToArray();
            }
            catch (Exception)
            { }
            return result;
        }

        [JsonRpcMethod(Methods.TextDocumentDocumentSymbolName)]
        public async System.Threading.Tasks.Task<object[]> TextDocumentDocumentSymbolName(JToken arg)
        {
            object[] result = null;
            try
            {
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- TextDocumentDocumentSymbol");
                    System.Console.Error.WriteLine(arg.ToString());
                }
                DocumentSymbolParams request = arg.ToObject<DocumentSymbolParams>();
                Document document = CheckDoc(request.TextDocument.Uri);
                IEnumerable<DocumentSymbol> r = LanguageServer.Module.Get(document);
                List<object> symbols = new List<object>();
                foreach (DocumentSymbol s in r)
                {
                    SymbolInformation si = new SymbolInformation();
                    if (s.kind == 0)
                    {
                        si.Kind = SymbolKind.Variable; // Nonterminal
                    }
                    else if (s.kind == 1)
                    {
                        si.Kind = SymbolKind.Enum; // Terminal
                    }
                    else if (s.kind == 2)
                    {
                        //si.Kind = 0; // Comment
                        continue;
                    }
                    else if (s.kind == 3)
                    {
                        // si.Kind = 0; // Keyword
                        continue;
                    }
                    else if (s.kind == 4)
                    {
                        // si.Kind = SymbolKind.Number; // Literal
                        continue;
                    }
                    else if (s.kind == 5)
                    {
                        // si.Kind = 0; // Mode
                        continue;
                    }
                    else if (s.kind == 6)
                    {
                        // si.Kind = SymbolKind.Enum; // Channel
                        continue;
                    }
                    else
                    {
                        // si.Kind = 0; // Default.
                        continue;
                    }

                    si.Name = s.name;
                    si.Location = new Microsoft.VisualStudio.LanguageServer.Protocol.Location
                    {
                        Uri = request.TextDocument.Uri
                    };
                    (int, int) lcs = LanguageServer.Module.GetLineColumn(s.range.Start.Value, document);
                    (int, int) lce = LanguageServer.Module.GetLineColumn(s.range.End.Value, document);
                    si.Location.Range = new Microsoft.VisualStudio.LanguageServer.Protocol.Range
                    {
                        Start = new Position(lcs.Item1, lcs.Item2),
                        End = new Position(lce.Item1, lce.Item2)
                    };
                    symbols.Add(si);
                }
                if (trace)
                {
                    System.Console.Error.Write("returning ");
                    System.Console.Error.WriteLine(string.Join(" ", symbols.Select(s =>
                    {
                        SymbolInformation v = (SymbolInformation)s;
                        return "<" + v.Name + "," + v.Kind + ">";
                    })));
                }
                result = symbols.ToArray();
            }
            catch (Exception)
            { }
            return result;
        }

        [JsonRpcMethod(Methods.TextDocumentCodeActionName)]
        public async System.Threading.Tasks.Task<JToken> TextDocumentCodeActionName(JToken arg)
        {
            try
            {
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- TextDocumentCodeAction");
                    System.Console.Error.WriteLine(arg.ToString());
                }
            }
            catch (Exception)
            { }
            return null;
        }

        [JsonRpcMethod(Methods.TextDocumentCodeLensName)]
        public async System.Threading.Tasks.Task<JToken> TextDocumentCodeLensName(JToken arg)
        {
            try
            {
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- TextDocumentCodeLens");
                    System.Console.Error.WriteLine(arg.ToString());
                }
            }
            catch (Exception)
            { }
            return null;
        }

        [JsonRpcMethod(Methods.CodeLensResolveName)]
        public async System.Threading.Tasks.Task<JToken> CodeLensResolveName(JToken arg)
        {
            try
            {
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- CodeLensResolve");
                    System.Console.Error.WriteLine(arg.ToString());
                }
            }
            catch (Exception)
            { }
            return null;
        }

        [JsonRpcMethod(Methods.TextDocumentDocumentLinkName)]
        public async System.Threading.Tasks.Task<JToken> TextDocumentDocumentLinkName(JToken arg)
        {
            try
            {
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- TextDocumentDocumentLink");
                    System.Console.Error.WriteLine(arg.ToString());
                }
            }
            catch (Exception)
            { }
            return null;
        }

        [JsonRpcMethod(Methods.DocumentLinkResolveName)]
        public async System.Threading.Tasks.Task<JToken> DocumentLinkResolveName(JToken arg)
        {
            try
            {
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- DocumentLinkResolve");
                    System.Console.Error.WriteLine(arg.ToString());
                }
            }
            catch (Exception)
            { }
            return null;
        }

        [JsonRpcMethod(Methods.TextDocumentFormattingName)]
        public async System.Threading.Tasks.Task<object[]> TextDocumentFormattingName(JToken arg)
        {
            object[] result = null;
            try
            {
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- TextDocumentFormatting");
                    System.Console.Error.WriteLine(arg.ToString());
                }
                DocumentFormattingParams request = arg.ToObject<DocumentFormattingParams>();
                Document document = CheckDoc(request.TextDocument.Uri);
                List<Microsoft.VisualStudio.LanguageServer.Protocol.TextEdit> new_list = new List<Microsoft.VisualStudio.LanguageServer.Protocol.TextEdit>();
                LanguageServer.TextEdit[] changes = LanguageServer.Module.Reformat(document);
                int count = 0;
                foreach (LanguageServer.TextEdit delta in changes)
                {
                    Microsoft.VisualStudio.LanguageServer.Protocol.TextEdit new_edit = new Microsoft.VisualStudio.LanguageServer.Protocol.TextEdit
                    {
                        Range = new Microsoft.VisualStudio.LanguageServer.Protocol.Range()
                    };
                    (int, int) lcs = LanguageServer.Module.GetLineColumn(delta.range.Start.Value, document);
                    (int, int) lce = LanguageServer.Module.GetLineColumn(delta.range.End.Value, document);
                    new_edit.Range.Start = new Position(lcs.Item1, lcs.Item2);
                    new_edit.Range.End = new Position(lce.Item1, lce.Item2);
                    new_edit.NewText = delta.NewText;
                    new_list.Add(new_edit);
                    count++;
                }
                result = new_list.ToArray();
            }
            catch (Exception)
            { }
            return result;
        }

        [JsonRpcMethod(Methods.TextDocumentRangeFormattingName)]
        public async System.Threading.Tasks.Task<JToken> TextDocumentRangeFormattingName(JToken arg)
        {
            try
            {
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- TextDocumentRangeFormatting");
                    System.Console.Error.WriteLine(arg.ToString());
                }
            }
            catch (Exception)
            { }
            return null;
        }

        [JsonRpcMethod(Methods.TextDocumentOnTypeFormattingName)]
        public async System.Threading.Tasks.Task<JToken> TextDocumentOnTypeFormattingName(JToken arg)
        {
            try
            {
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- TextDocumentOnTypeFormatting");
                    System.Console.Error.WriteLine(arg.ToString());
                }
            }
            catch (Exception)
            { }
            return null;
        }

        [JsonRpcMethod(Methods.TextDocumentRenameName)]
        public async System.Threading.Tasks.Task<WorkspaceEdit> TextDocumentRenameName(JToken arg)
        {
            WorkspaceEdit edit = null;
            try
            {
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- TextDocumentRename");
                    System.Console.Error.WriteLine(arg.ToString());
                }
                RenameParams request = arg.ToObject<RenameParams>();
                Document document = CheckDoc(request.TextDocument.Uri);
                Position position = request.Position;
                int line = position.Line;
                int character = position.Character;
                int index = LanguageServer.Module.GetIndex(line, character, document);
                if (trace)
                {
                    System.Console.Error.WriteLine("position index = " + index);
                    (int, int) back = LanguageServer.Module.GetLineColumn(index, document);
                    System.Console.Error.WriteLine("back to l,c = " + back.Item1 + "," + back.Item2);
                }
                string new_name = request.NewName;
                Dictionary<string, LanguageServer.TextEdit[]> changes = LanguageServer.Module.Rename(index, new_name, document);
                edit = new WorkspaceEdit();
                int count = 0;
                Dictionary<string, Microsoft.VisualStudio.LanguageServer.Protocol.TextEdit[]> edit_changes_array = new Dictionary<string, Microsoft.VisualStudio.LanguageServer.Protocol.TextEdit[]>();
                foreach (KeyValuePair<string, LanguageServer.TextEdit[]> pair in changes)
                {
                    string doc = pair.Key;
                    Uri uri = new Uri(doc);
                    LanguageServer.TextEdit[] val = pair.Value;
                    List<Microsoft.VisualStudio.LanguageServer.Protocol.TextEdit> new_list = new List<Microsoft.VisualStudio.LanguageServer.Protocol.TextEdit>();
                    foreach (LanguageServer.TextEdit v in val)
                    {
                        Microsoft.VisualStudio.LanguageServer.Protocol.TextEdit new_edit = new Microsoft.VisualStudio.LanguageServer.Protocol.TextEdit
                        {
                            Range = new Microsoft.VisualStudio.LanguageServer.Protocol.Range()
                        };
                        (int, int) lcs = LanguageServer.Module.GetLineColumn(v.range.Start.Value, document);
                        (int, int) lce = LanguageServer.Module.GetLineColumn(v.range.End.Value, document);
                        new_edit.Range.Start = new Position(lcs.Item1, lcs.Item2);
                        new_edit.Range.End = new Position(lce.Item1, lce.Item2);
                        new_edit.NewText = v.NewText;
                        new_list.Add(new_edit);
                        count++;
                    }
                    edit_changes_array.Add(uri.ToString(), new_list.ToArray());
                }
                edit.Changes = edit_changes_array;
            }
            catch (Exception)
            { }
            return edit;
        }

        [JsonRpcMethod(Methods.TextDocumentFoldingRangeName)]
        public async System.Threading.Tasks.Task<JToken> TextDocumentFoldingRangeName(JToken arg)
        {
            try
            {
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- TextDocumentFoldingRange");
                    System.Console.Error.WriteLine(arg.ToString());
                }
            }
            catch (Exception)
            { }
            return null;
        }


        [JsonRpcMethod("CMGetClassifiers")]
        public async System.Threading.Tasks.Task<SymbolInformation[]> CMGetClassifiers(JToken arg)
        {
            SymbolInformation[] result = null;
            try
            {
                CMGetClassifiersParams request = arg.ToObject<CMGetClassifiersParams>();
                Document document = CheckDoc(request.TextDocument);
                int start = request.Start;
                int end = request.End;
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- CMGetClassifiers");
                    System.Console.Error.WriteLine(arg.ToString());
                    (int, int) bs = LanguageServer.Module.GetLineColumn(start, document);
                    System.Console.Error.WriteLine("");
                }
                IEnumerable<DocumentSymbol> r = LanguageServer.Module.Get(document);
                List<SymbolInformation> symbols = new List<SymbolInformation>();
                foreach (DocumentSymbol s in r)
                {
                    if (!(start <= s.range.Start.Value && s.range.Start.Value <= end))
                    {
                        continue;
                    }

                    SymbolInformation si = new SymbolInformation();
                    if (s.kind == 0)
                    {
                        si.Kind = SymbolKind.Variable; // Nonterminal
                    }
                    else if (s.kind == 1)
                    {
                        si.Kind = SymbolKind.Enum; // Terminal
                    }
                    else if (s.kind == 2)
                    {
                        //si.Kind = 0; // Comment
                        continue;
                    }
                    else if (s.kind == 3)
                    {
                        // si.Kind = 0; // Keyword
                        continue;
                    }
                    else if (s.kind == 4)
                    {
                        // si.Kind = SymbolKind.Number; // Literal
                        continue;
                    }
                    else if (s.kind == 5)
                    {
                        // si.Kind = 0; // Mode
                        continue;
                    }
                    else if (s.kind == 6)
                    {
                        // si.Kind = SymbolKind.Enum; // Channel
                        continue;
                    }
                    else
                    {
                        // si.Kind = 0; // Default.
                        continue;
                    }

                    si.Name = s.name;
                    si.Location = new Microsoft.VisualStudio.LanguageServer.Protocol.Location
                    {
                        Uri = request.TextDocument
                    };
                    (int, int) lcs = LanguageServer.Module.GetLineColumn(s.range.Start.Value, document);
                    (int, int) lce = LanguageServer.Module.GetLineColumn(s.range.End.Value, document);
                    si.Location.Range = new Microsoft.VisualStudio.LanguageServer.Protocol.Range
                    {
                        Start = new Position(lcs.Item1, lcs.Item2),
                        End = new Position(lce.Item1, lce.Item2)
                    };
                    symbols.Add(si);
                }
                if (trace)
                {
                    System.Console.Error.Write("returning ");
                    System.Console.Error.WriteLine(string.Join(" ", symbols.Select(s =>
                    {
                        SymbolInformation v = s;
                        return "<" + v.Name + "," + v.Kind + ">";
                    })));
                }
                result = symbols.ToArray();
            }
            catch (Exception)
            { }
            return result;
        }

        [JsonRpcMethod("CMNextSymbol")]
        public async System.Threading.Tasks.Task<int> CMNextSymbol(JToken arg)
        {
            CMNextSymbolParams request = arg.ToObject<CMNextSymbolParams>();
            int pos = request.Pos;
            bool forward = request.Forward;
            int next_sym = forward ? int.MaxValue : -1;
            try
            {
                Document document = CheckDoc(request.TextDocument);
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- CMNextSymbol");
                    System.Console.Error.WriteLine(arg.ToString());
                    (int, int) bs = LanguageServer.Module.GetLineColumn(pos, document);
                    System.Console.Error.WriteLine("");
                }
                IEnumerable<Location> r = LanguageServer.Module.GetDefs(document);
                List<object> symbols = new List<object>();
                foreach (Location s in r)
                {
                    if (forward)
                    {
                        if (s.Range.Start.Value > pos && s.Range.Start.Value < next_sym)
                        {
                            next_sym = s.Range.Start.Value;
                        }
                    }
                    else
                    {
                        if (s.Range.Start.Value < pos && s.Range.Start.Value > next_sym)
                        {
                            next_sym = s.Range.Start.Value;
                        }
                    }
                }
                if (next_sym == int.MaxValue)
                {
                    next_sym = -1;
                }
                if (trace)
                {
                    System.Console.Error.Write("returning ");
                    System.Console.Error.WriteLine(next_sym);
                }
            }
            catch (Exception)
            { }
            return next_sym;
        }

        [JsonRpcMethod("CMGotoVisitor")]
        public async System.Threading.Tasks.Task<CMGotoResult> CMGotoVisitor(JToken arg)
        {
            CMGotoResult s = null;
            try
            {
                CMGotoParams request = arg.ToObject<CMGotoParams>();
                Document document = CheckDoc(request.TextDocument);
                int pos = request.Pos;
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- CMGotoVisitor");
                    System.Console.Error.WriteLine(arg.ToString());
                    (int, int) bs = LanguageServer.Module.GetLineColumn(pos, document);
                    System.Console.Error.WriteLine("");
                }
                bool key_state = request.IsEnter;
                s = Goto.main(true, key_state, document, pos);
            }
            catch (Exception)
            { }
            return s;
        }

        [JsonRpcMethod("CMGotoListener")]
        public async System.Threading.Tasks.Task<CMGotoResult> CMGotoListener(JToken arg)
        {
            CMGotoResult s = null;
            try
            {
                CMGotoParams request = arg.ToObject<CMGotoParams>();
                Document document = CheckDoc(request.TextDocument);
                int pos = request.Pos;
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- CMGotoListener");
                    System.Console.Error.WriteLine(arg.ToString());
                    (int, int) bs = LanguageServer.Module.GetLineColumn(pos, document);
                    System.Console.Error.WriteLine("");
                }
                bool is_enter = request.IsEnter;
                s = Goto.main(false, is_enter, document, pos);
            }
            catch (Exception)
            { }
            return s;
        }

        [JsonRpcMethod("CMReplaceLiterals")]
        public async System.Threading.Tasks.Task<Dictionary<string, string>> CMReplaceLiterals(JToken arg)
        {
            Dictionary<string, string> changes = null;
            try
            {
                CMReplaceLiteralsParams request = arg.ToObject<CMReplaceLiteralsParams>();
                Document document = CheckDoc(request.TextDocument);
                int pos = request.Pos;
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- CMReplaceLiterals");
                    System.Console.Error.WriteLine(arg.ToString());
                    (int, int) bs = LanguageServer.Module.GetLineColumn(pos, document);
                    System.Console.Error.WriteLine("");
                }
                changes = LanguageServer.Transform.ReplaceLiterals(pos, document);
            }
            catch (Exception)
            { }
            return changes;
        }

        [JsonRpcMethod("CMRemoveUselessParserProductions")]
        public async System.Threading.Tasks.Task<Dictionary<string, string>> CMRemoveUselessParserProductions(JToken arg)
        {
            Dictionary<string, string> changes = null;
            try
            {
                CMReplaceLiteralsParams request = arg.ToObject<CMReplaceLiteralsParams>();
                Document document = CheckDoc(request.TextDocument);
                int pos = request.Pos;
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- CMRemoveUselessParserProductions");
                    System.Console.Error.WriteLine(arg.ToString());
                    (int, int) bs = LanguageServer.Module.GetLineColumn(pos, document);
                    System.Console.Error.WriteLine("");
                }
                changes = LanguageServer.Transform.RemoveUselessParserProductions(pos, document);
            }
            catch (Exception)
            { }
            return changes;
        }

        [JsonRpcMethod("CMMoveStartRuleToTop")]
        public async System.Threading.Tasks.Task<Dictionary<string, string>> CMMoveStartRuleToTop(JToken arg)
        {
            Dictionary<string, string> changes = null;
            try
            {
                CMReplaceLiteralsParams request = arg.ToObject<CMReplaceLiteralsParams>();
                Document document = CheckDoc(request.TextDocument);
                int pos = request.Pos;
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- CMMoveStartRuleToTop");
                    System.Console.Error.WriteLine(arg.ToString());
                    (int, int) bs = LanguageServer.Module.GetLineColumn(pos, document);
                    System.Console.Error.WriteLine("");
                }
                changes = LanguageServer.Transform.MoveStartRuleToTop(pos, document);
            }
            catch (Exception)
            { }
            return changes;
        }

        [JsonRpcMethod("CMReorderParserRules")]
        public async System.Threading.Tasks.Task<Dictionary<string, string>> CMReorderParserRules(JToken arg)
        {
            Dictionary<string, string> changes = null;
            try
            {
                CMReorderParserRulesParams request = arg.ToObject<CMReorderParserRulesParams>();
                Document document = CheckDoc(request.TextDocument);
                int pos = request.Pos;
                LspAntlr.ReorderType type = request.Type;
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- CMReorderParserRules");
                    System.Console.Error.WriteLine(arg.ToString());
                    (int, int) bs = LanguageServer.Module.GetLineColumn(pos, document);
                    System.Console.Error.WriteLine("");
                }
                changes = LanguageServer.Transform.ReorderParserRules(pos, document, type);
            }
            catch (Exception)
            { }
            return changes;
        }

        [JsonRpcMethod("CMSplitCombineGrammars")]
        public async System.Threading.Tasks.Task<Dictionary<string, string>> CMSplitCombineGrammars(JToken arg)
        {
            Dictionary<string, string> changes = null;
            try
            {
                CMSplitCombineGrammarsParams request = arg.ToObject<CMSplitCombineGrammarsParams>();
                Document document = CheckDoc(request.TextDocument);
                int pos = request.Pos;
                bool split = request.Split;
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- CMSplitCombineGrammars");
                    System.Console.Error.WriteLine(arg.ToString());
                    (int, int) bs = LanguageServer.Module.GetLineColumn(pos, document);
                    System.Console.Error.WriteLine("");
                }
                changes = LanguageServer.Transform.SplitCombineGrammars(pos, document, split);
            }
            catch (Exception)
            { }
            return changes;
        }

        [JsonRpcMethod("CMImportGrammars")]
        public async System.Threading.Tasks.Task<Dictionary<string, string>> CMImportGrammars(JToken arg)
        {
            Dictionary<string, string> changes = null;
            try
            {
                List<string> request = arg.ToObject<List<string>>();
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- CMImportGrammars");
                    System.Console.Error.WriteLine(arg.ToString());
                }
                changes = LanguageServer.BisonImport.ImportGrammars(request);
            }
            catch (Exception)
            { }
            return changes;
        }

        [JsonRpcMethod("CMEliminateLeftRecursion")]
        public async System.Threading.Tasks.Task<Dictionary<string, string>> CMEliminateLeftRecursion(JToken arg)
        {
            Dictionary<string, string> s = null;
            try
            {
                CMEliminateLeftRecursionParams request = arg.ToObject<CMEliminateLeftRecursionParams>();
                Document document = CheckDoc(request.TextDocument);
                int pos = request.Pos;
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- CMEliminateLeftRecursion");
                    System.Console.Error.WriteLine(arg.ToString());
                    (int, int) bs = LanguageServer.Module.GetLineColumn(pos, document);
                    System.Console.Error.WriteLine("");
                }
                s = Transform.EliminateLeftRecursion(pos, document);
            }
            catch (Exception)
            { }
            return s;
        }

        [JsonRpcMethod("CMEliminateAntlrKeywordsInRules")]
        public async System.Threading.Tasks.Task<Dictionary<string, string>> CMEliminateAntlrKeywordsInRules(JToken arg)
        {
            Dictionary<string, string> s = null;
            try
            {
                Uri request = arg.ToObject<Uri>();
                Document document = CheckDoc(request);
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- CMEliminateAntlrKeywordsInRules");
                    System.Console.Error.WriteLine(arg.ToString());
                }
                s = Transform.EliminateAntlrKeywordsInRules(document);
            }
            catch (Exception)
            { }
            return s;
        }


    }
}
