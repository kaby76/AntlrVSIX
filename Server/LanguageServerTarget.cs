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
        private readonly Dictionary<string, bool> ignore_next_change = new Dictionary<string, bool>();

        public LanguageServerTarget(LSPServer server)
        {
            this.server = server;
            _workspace = Workspaces.Workspace.Instance;
        }


        void ApplyChanges(string transaction_name, Dictionary<string, string> ch)
        {
            if (!ch.Any())
            {
                throw new LanguageServerException("No changes were needed, none made.");
            }
            Dictionary<string, Microsoft.VisualStudio.LanguageServer.Protocol.TextEdit[]> a = new Dictionary<string, Microsoft.VisualStudio.LanguageServer.Protocol.TextEdit[]>();
            foreach (var pair in ch)
            {
                var fn = pair.Key;
                var new_code = pair.Value;
                Document document = CheckDoc(new Uri(fn));
                var code = document.Code;
                List<LanguageServer.TextEdit> edits = new List<LanguageServer.TextEdit>();
                Diff_match_patch diff = new Diff_match_patch();
                List<Diff> diffs = diff.Diff_main(code, new_code);
                List<Patch> patch = diff.Patch_make(diffs);
                {
                    // Start edit session.
                    int times = 0;
                    int delta = 0;
                    foreach (Patch p in patch)
                    {
                        times++;
                        int start = p.start1 - delta;

                        int offset = 0;
                        foreach (Diff ed in p.diffs)
                        {
                            if (ed.operation == Operation.EQUAL)
                            {
                                //// Let's verify that.
                                int len = ed.text.Length;
                                //var tokenSpan = new SnapshotSpan(buffer.CurrentSnapshot,
                                //  new Span(start + offset, len));
                                //var tt = tokenSpan.GetText();
                                //if (ed.text != tt)
                                //{ }
                                offset += len;
                            }
                            else if (ed.operation == Operation.DELETE)
                            {
                                int len = ed.text.Length;
                                //var tokenSpan = new SnapshotSpan(buffer.CurrentSnapshot,
                                //  new Span(start + offset, len));
                                //var tt = tokenSpan.GetText();
                                //if (ed.text != tt)
                                //{ }
                                LanguageServer.TextEdit edit = new LanguageServer.TextEdit()
                                {
                                    range = new Workspaces.Range(
                                        new Workspaces.Index(start + offset),
                                        new Workspaces.Index(start + offset + len)),
                                    NewText = ""
                                };
                                offset += len;
                                edits.Add(edit);
                            }
                            else if (ed.operation == Operation.INSERT)
                            {
                                int len = ed.text.Length;
                                LanguageServer.TextEdit edit = new LanguageServer.TextEdit()
                                {
                                    range = new Workspaces.Range(
                                        new Workspaces.Index(start + offset),
                                        new Workspaces.Index(start + offset)),
                                    NewText = ed.text
                                };
                                edits.Add(edit);
                            }
                        }
                        delta += (p.length2 - p.length1);
                    }
                }
                var changes = edits.ToArray();

                List<Microsoft.VisualStudio.LanguageServer.Protocol.TextEdit> new_list = new List<Microsoft.VisualStudio.LanguageServer.Protocol.TextEdit>();
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
                var result = new_list.ToArray();
                a[fn] = result;
                lock (_object)
                {
                    ignore_next_change[fn] = true;
                }
                // This must be done after computing changes since offsets/line/column computations
                // depend on what is currently the source.
                document.Code = new_code;
            }
            // Recompile only after every single change everywhere is in.
            _ = LanguageServer.Module.Compile();
            server.ApplyEdit(transaction_name, a);
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

                CompletionProvider =
                    (Options.Option.GetBoolean("EnableCompletion")
                        ? new CompletionOptions
                        {
                            ResolveProvider = true,
                            TriggerCharacters = new string[] { ",", "." }
                        }
                        : null),

                HoverProvider = true,

                SignatureHelpProvider = null,

                // DeclarationProvider not supported.

                DefinitionProvider = true,

                TypeDefinitionProvider = false, // Does not make sense for Antlr.
                
                ImplementationProvider = false, // Does not make sense for Antlr.

                ReferencesProvider = true,

                DocumentHighlightProvider = true,

                DocumentSymbolProvider = true,

                CodeLensProvider = null,

                DocumentLinkProvider = null,

                // ColorProvider not supported.

                DocumentFormattingProvider = true,

                DocumentRangeFormattingProvider = false,

                RenameProvider = true,

                FoldingRangeProvider = null,

                ExecuteCommandProvider = null,

                // SelectionRangeProvider not supported.

                WorkspaceSymbolProvider = false,

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
        public void InitializedName(JToken arg)
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
        public JToken ShutdownName()
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
        public void ExitName()
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
        public void WorkspaceDidChangeConfigurationName(JToken arg)
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
        public void WorkspaceDidChangeWatchedFilesName(JToken arg)
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
        public JToken WorkspaceSymbolName(JToken arg)
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
        public JToken WorkspaceExecuteCommandName(JToken arg)
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
        public JToken WorkspaceApplyEditName(JToken arg)
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
        public void TextDocumentDidOpenName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- TextDocumentDidOpen");
                System.Console.Error.WriteLine(arg.ToString());
            }
            DidOpenTextDocumentParams request = arg.ToObject<DidOpenTextDocumentParams>();
            _ = CheckDoc(request.TextDocument.Uri);
        }

        [JsonRpcMethod(Methods.TextDocumentDidChangeName)]
        public void TextDocumentDidChangeName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- TextDocumentDidChange");
                System.Console.Error.WriteLine(arg.ToString());
            }
            DidChangeTextDocumentParams request = arg.ToObject<DidChangeTextDocumentParams>();
            Document document = CheckDoc(request.TextDocument.Uri);
            lock (_object)
            {
                if (!ignore_next_change.ContainsKey(document.FullPath))
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
                else
                {
                    ignore_next_change.Remove(document.FullPath);
                }
            }
        }

        [JsonRpcMethod(Methods.TextDocumentWillSaveName)]
        public void TextDocumentWillSaveName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- TextDocumentWillSave");
                System.Console.Error.WriteLine(arg.ToString());
            }
            // Nothing to do--who cares because the server does not perform a save.
        }

        [JsonRpcMethod(Methods.TextDocumentWillSaveWaitUntilName)]
        public JToken TextDocumentWillSaveWaitUntilName(JToken arg)
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
        public void TextDocumentDidSaveName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- TextDocumentDidSave");
                System.Console.Error.WriteLine(arg.ToString());
            }
            // Nothing to do--who cares because the server does not perform a save.
        }

        [JsonRpcMethod(Methods.TextDocumentDidCloseName)]
        public void TextDocumentDidCloseName(JToken arg)
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
        public void TextDocumentPublishDiagnosticsName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- TextDocumentPublishDiagnostics");
                System.Console.Error.WriteLine(arg.ToString());
            }
        }

        // ======= LANGUAGE FEATURES ========

        [JsonRpcMethod(Methods.TextDocumentCompletionName)]
        public object[] TextDocumentCompletionName(JToken arg)
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
        public JToken TextDocumentCompletionResolveName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- TextDocumentCompletionResolve");
                System.Console.Error.WriteLine(arg.ToString());
            }
            return null;
        }

        public static Document CheckDoc(System.Uri uri)
        {
            string decoded = System.Web.HttpUtility.UrlDecode(uri.AbsoluteUri);
            string file_name = new Uri(decoded).LocalPath;
            Document document = Workspaces.Workspace.Instance.FindDocument(file_name);
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
                Project project = Workspaces.Workspace.Instance.FindProject("Misc");
                if (project == null)
                {
                    project = new Project("Misc", "Misc", "Misc");
                    Workspaces.Workspace.Instance.AddChild(project);
                }
                project.AddDocument(document);
                document.Changed = true;
                _ = ParserDetailsFactory.Create(document);
                _ = LanguageServer.Module.Compile();
            }
            return document;
        }

        [JsonRpcMethod(Methods.TextDocumentHoverName)]
        public object TextDocumentHoverName(JToken arg)
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
                System.Console.Error.WriteLine("returning " + quick_info.Display);
            }
            catch (Exception)
            { }
            return hover;
        }

        [JsonRpcMethod(Methods.TextDocumentSignatureHelpName)]
        public JToken TextDocumentSignatureHelpName(JToken arg)
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
        public object[] TextDocumentDefinitionName(JToken arg)
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
                IList<Location> found = LanguageServer.Module.FindDefs(index, document);
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
        public object[] TextDocumentTypeDefinitionName(JToken arg)
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
                IList<Location> found = LanguageServer.Module.FindDefs(index, document);
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
        public object[] TextDocumentImplementationName(JToken arg)
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
                IList<Location> found = LanguageServer.Module.FindDefs(index, document);
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
        public object[] TextDocumentReferencesName(JToken arg)
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
                if (trace)
                {
                    System.Console.Error.Write("returning ");
                    System.Console.Error.WriteLine(string.Join(
                        System.Environment.NewLine, result.Select(s =>
                    {
                        var v = (Microsoft.VisualStudio.LanguageServer.Protocol.Location)s;
                        var dd = CheckDoc(v.Uri);
                        return "<" + v.Uri +
                            ",[" + LanguageServer.Module.GetIndex(
                                v.Range.Start.Line,
                                v.Range.Start.Character,
                                dd)
                            + ".."
                            + LanguageServer.Module.GetIndex(
                                v.Range.End.Line,
                                v.Range.End.Character,
                                dd)
                            + "]>";
                    })));
                }
                server.ShowMessage("" + result.Length + " results.", MessageType.Info);
            }
            catch (Exception eeks)
            {
                System.Console.Error.WriteLine("Exception: " + eeks.ToString());
            }
            return result;
        }

        [JsonRpcMethod(Methods.TextDocumentDocumentHighlightName)]
        public object[] TextDocumentDocumentHighlightName(JToken arg)
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
                    if (f.Uri.FullPath != document.FullPath)
                    {
                        continue;
                    }
                    Microsoft.VisualStudio.LanguageServer.Protocol.DocumentHighlight location = new DocumentHighlight();
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
        public object[] TextDocumentDocumentSymbolName(JToken arg)
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
                        continue;
                        // si.Kind = SymbolKind.String; // Comment
                    }
                    else if (s.kind == 3)
                    {
                        continue;
                        // si.Kind = SymbolKind.Key; // Keyword
                    }
                    else if (s.kind == 4)
                    {
                        continue;
                        // si.Kind = SymbolKind.Constant; // Literal
                    }
                    else if (s.kind == 5)
                    {
                        si.Kind = SymbolKind.Event; // Mode
                    }
                    else if (s.kind == 6)
                    {
                        si.Kind = SymbolKind.Object; // Channel
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
                        return "<" + v.Name + "," + v.Kind
                            + ",[" + LanguageServer.Module.GetIndex(
                                v.Location.Range.Start.Line,
                                v.Location.Range.Start.Character,
                                document)
                            + ".."
                            + LanguageServer.Module.GetIndex(
                                v.Location.Range.End.Line,
                                v.Location.Range.End.Character,
                                document)
                            + "]>";
                    })));
                }
                result = symbols.ToArray();
            }
            catch (Exception)
            { }
            return result;
        }

        [JsonRpcMethod(Methods.TextDocumentCodeActionName)]
        public JToken TextDocumentCodeActionName(JToken arg)
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
        public JToken TextDocumentCodeLensName(JToken arg)
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
        public JToken CodeLensResolveName(JToken arg)
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
        public JToken TextDocumentDocumentLinkName(JToken arg)
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
        public JToken DocumentLinkResolveName(JToken arg)
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
        public object[] TextDocumentFormattingName(JToken arg)
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
        public JToken TextDocumentRangeFormattingName(JToken arg)
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
        public JToken TextDocumentOnTypeFormattingName(JToken arg)
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
        public WorkspaceEdit TextDocumentRenameName(JToken arg)
        {
            if (trace)
            {
                System.Console.Error.WriteLine("<-- TextDocumentRename");
                System.Console.Error.WriteLine(arg.ToString());
            }
            WorkspaceEdit edit = null;
            lock (_object)
            {
                try
                {
                    RenameParams request = arg.ToObject<RenameParams>();
                    Position position = request.Position;
                    int line = position.Line;
                    int character = position.Character;
                    Document document = CheckDoc(request.TextDocument.Uri);
                    if (!ignore_next_change.ContainsKey(document.FullPath))
                    {
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
                    else
                    {
                        ignore_next_change.Remove(document.FullPath);
                    }
                }
                catch (Exception)
                { }
            }
            return edit;
        }

        [JsonRpcMethod(Methods.TextDocumentFoldingRangeName)]
        public JToken TextDocumentFoldingRangeName(JToken arg)
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
        public CMClassifierInformation[] CMGetClassifiers(JToken arg)
        {
            CMClassifierInformation[] result = null;
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
                IEnumerable<Module.Info> r = LanguageServer.Module.Get(start, end, document);
                List<CMClassifierInformation> symbols = new List<CMClassifierInformation>();
                foreach (var p in r)
                {
                    CMClassifierInformation si = new CMClassifierInformation
                    {
                        Kind = p.kind,
                        start = p.start,
                        end = p.end
                    };
                    symbols.Add(si);
                }
                if (trace)
                {
                    System.Console.Error.Write("returning ");
                    System.Console.Error.WriteLine(string.Join(" ", symbols.Select(s =>
                    {
                        var v = s;
                        return "<" + v.start + "," + v.end + "," + v.Kind + ">";
                    })));
                }
                result = symbols.ToArray();
            }
            catch (LanguageServerException e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
            catch (Exception e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
            return result;
        }

        [JsonRpcMethod("CMNextSymbol")]
        public int CMNextSymbol(JToken arg)
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
            catch (LanguageServerException e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
            catch (Exception e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
            return next_sym;
        }

        [JsonRpcMethod("CMGotoVisitor")]
        public CMGotoResult CMGotoVisitor(JToken arg)
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
            catch (LanguageServerException e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
            catch (Exception e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
            return s;
        }

        [JsonRpcMethod("CMGotoListener")]
        public CMGotoResult CMGotoListener(JToken arg)
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
            catch (LanguageServerException e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
            catch (Exception e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
            return s;
        }

        [JsonRpcMethod("CMReplaceLiterals")]
        public void CMReplaceLiterals(JToken arg)
        {
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
                var s = LanguageServer.Transform.ReplaceLiterals(pos, pos, document);
                ApplyChanges("Replace String Literals", s);
            }
            catch (LanguageServerException e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
            catch (Exception e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
        }

        [JsonRpcMethod("CMRemoveUselessParserProductions")]
        public void CMRemoveUselessParserProductions(JToken arg)
        {
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
                var s = LanguageServer.Transform.RemoveUselessParserProductions(pos, document);
                ApplyChanges("Remove Useless Rules", s);
            }
            catch (LanguageServerException e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
            catch (Exception e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
        }

        [JsonRpcMethod("CMMoveStartRuleToTop")]
        public void CMMoveStartRuleToTop(JToken arg)
        {
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
                var s = LanguageServer.Transform.MoveStartRuleToTop(pos, document);
                ApplyChanges("Move Start Rule To Top", s);
            }
            catch (LanguageServerException e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
            catch (Exception e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
        }

        [JsonRpcMethod("CMReorderParserRules")]
        public void CMReorderParserRules(JToken arg)
        {
            try
            {
                CMReorderParserRulesParams request = arg.ToObject<CMReorderParserRulesParams>();
                Document document = CheckDoc(request.TextDocument);
                LspAntlr.ReorderType type = request.Type;
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- CMReorderParserRules");
                    System.Console.Error.WriteLine(arg.ToString());
                }
                var s = LanguageServer.Transform.ReorderParserRules(document, type);
                string na = "";
                switch (type)
                {
                    case LspAntlr.ReorderType.Alphabetically:
                        na = "Alphabetically"; break;
                    case LspAntlr.ReorderType.BFS:
                        na = "BFS"; break;
                    case LspAntlr.ReorderType.DFS:
                        na = "DFS"; break;
                }
                ApplyChanges("Reorder Parser Rules " + na, s);
            }
            catch (LanguageServerException e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
            catch (Exception e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
        }

        [JsonRpcMethod("CMSplitCombineGrammars")]
        public Dictionary<string, string> CMSplitCombineGrammars(JToken arg)
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
            catch (LanguageServerException e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
            catch (Exception e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
            return changes;
        }

        [JsonRpcMethod("CMImportGrammars")]
        public Dictionary<string, string> CMImportGrammars(JToken arg)
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
            catch (LanguageServerException e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
            catch (Exception e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
            return changes;
        }

        [JsonRpcMethod("CMConvertRecursionToKleeneOperator")]
        public void CMConvertRecursionToKleeneOperator(JToken arg)
        {
            try
            {
                CMEliminateDirectLeftRecursionParams request = arg.ToObject<CMEliminateDirectLeftRecursionParams>();
                Document document = CheckDoc(request.TextDocument);
                int pos = request.Pos;
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- CMConvertRecursionToKleeneOperator");
                    System.Console.Error.WriteLine(arg.ToString());
                    (int, int) bs = LanguageServer.Module.GetLineColumn(pos, document);
                    System.Console.Error.WriteLine("");
                }
                var s = Transform.ConvertRecursionToKleeneOperator(pos, document);
                ApplyChanges("Convert to Kleene Syntax", s);
            }
            catch (LanguageServerException e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
            catch (Exception e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
        }


        [JsonRpcMethod("CMEliminateDirectLeftRecursion")]
        public void CMEliminateDirectLeftRecursion(JToken arg)
        {
            try
            {
                CMEliminateDirectLeftRecursionParams request = arg.ToObject<CMEliminateDirectLeftRecursionParams>();
                Document document = CheckDoc(request.TextDocument);
                int pos = request.Pos;
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- CMEliminateDirectLeftRecursion");
                    System.Console.Error.WriteLine(arg.ToString());
                    (int, int) bs = LanguageServer.Module.GetLineColumn(pos, document);
                    System.Console.Error.WriteLine("");
                }
                var s = Transform.EliminateDirectLeftRecursion(pos, document);
                ApplyChanges("Elimiate Direct Left Recursion", s);
            }
            catch (LanguageServerException e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
            catch (Exception e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
        }

        [JsonRpcMethod("CMEliminateIndirectLeftRecursion")]
        public void CMEliminateIndirectLeftRecursion(JToken arg)
        {
            try
            {
                CMEliminateDirectLeftRecursionParams request = arg.ToObject<CMEliminateDirectLeftRecursionParams>();
                Document document = CheckDoc(request.TextDocument);
                int pos = request.Pos;
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- CMEliminateIndirectLeftRecursion");
                    System.Console.Error.WriteLine(arg.ToString());
                    (int, int) bs = LanguageServer.Module.GetLineColumn(pos, document);
                    System.Console.Error.WriteLine("");
                }
                var s = Transform.EliminateIndirectLeftRecursion(pos, document);
                ApplyChanges("Elimiate Indirect Left Recursion", s);
            }
            catch (LanguageServerException e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
            catch (Exception e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
        }

        [JsonRpcMethod("CMEliminateAntlrKeywordsInRules")]
        public void CMEliminateAntlrKeywordsInRules(JToken arg)
        {
            try
            {
                Uri request = arg.ToObject<Uri>();
                Document document = CheckDoc(request);
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- CMEliminateAntlrKeywordsInRules");
                    System.Console.Error.WriteLine(arg.ToString());
                }
                var s = Transform.EliminateAntlrKeywordsInRules(document);
                ApplyChanges("Eliminate Keywords", s);
            }
            catch (LanguageServerException e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
            catch (Exception e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
        }


        [JsonRpcMethod("CMAddLexerRulesForStringLiterals")]
        public void CMAddLexerRulesForStringLiterals(JToken arg)
        {
            try
            {
                Uri request = arg.ToObject<Uri>();
                Document document = CheckDoc(request);
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- CMAddLexerRulesForStringLiterals");
                    System.Console.Error.WriteLine(arg.ToString());
                }
                var s = Transform.AddLexerRulesForStringLiterals(document);
                ApplyChanges("Replace String Literals", s);
            }
            catch (LanguageServerException e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
            catch (Exception e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
        }

        [JsonRpcMethod("CMSortModes")]
        public void CMSortModes(JToken arg)
        {
            try
            {
                Uri request = arg.ToObject<Uri>();
                Document document = CheckDoc(request);
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- CMSortModes");
                    System.Console.Error.WriteLine(arg.ToString());
                }
                var s = Transform.SortModes(document);
                ApplyChanges("Sort Modes", s);
            }
            catch (LanguageServerException e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
            catch (Exception e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
        }

        [JsonRpcMethod("CMUnfold")]
        public void CMUnfold(JToken arg1, JToken arg2)
        {
            try
            {
                string a1 = arg1.ToObject<string>();
                int a2 = arg2.ToObject<int>();
                Document document = CheckDoc(new Uri(a1));
                int pos = a2;
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- CMUnfold");
                    System.Console.Error.WriteLine(a1);
                    (int, int) bs = LanguageServer.Module.GetLineColumn(pos, document);
                    System.Console.Error.WriteLine("line " + bs.Item1 + " col " + bs.Item2);
                }
                var s = Transform.Unfold(pos, document);
                ApplyChanges("Unfold", s);
            }
            catch (LanguageServerException e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
            catch (Exception e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
        }

        [JsonRpcMethod("CMFold")]
        public void CMFold(JToken arg1, JToken arg2, JToken arg3)
        {
            try
            {
                string a1 = arg1.ToObject<string>();
                int a2 = arg2.ToObject<int>();
                int a3 = arg3.ToObject<int>();
                Document document = CheckDoc(new Uri(a1));
                int start = a2;
                int end = a3;
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- CMFold");
                    System.Console.Error.WriteLine(a1);
                    (int, int) bs = LanguageServer.Module.GetLineColumn(start, document);
                    System.Console.Error.WriteLine("line " + bs.Item1 + " col " + bs.Item2);
                }
                var s = Transform.Fold(start, end, document);
                ApplyChanges("Fold", s);
            }
            catch (LanguageServerException e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
            catch (Exception e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
        }

        [JsonRpcMethod("CMRemoveUselessParentheses")]
        public void CMRemoveUselessParentheses(JToken arg1, JToken arg2, JToken arg3)
        {
            try
            {
                string a1 = arg1.ToObject<string>();
                int a2 = arg2.ToObject<int>();
                int a3 = arg3.ToObject<int>();
                Document document = CheckDoc(new Uri(a1));
                int start = a2;
                int end = a3;
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- CMRemoveUselessParentheses");
                    System.Console.Error.WriteLine(a1);
                    (int, int) bs = LanguageServer.Module.GetLineColumn(start, document);
                    System.Console.Error.WriteLine("line " + bs.Item1 + " col " + bs.Item2);
                }
                var s = Transform.RemoveUselessParentheses(start, end, document);
                ApplyChanges("Remove Useless Parentheses", s);
            }
            catch (LanguageServerException e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
            catch (Exception e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
        }

        [JsonRpcMethod("CMPerformAnalysis")]
        public void CMPerformAnalysis(JToken arg1)
        {
            try
            {
                string a1 = arg1.ToObject<string>();
                Document document = CheckDoc(new Uri(a1));
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- CMPerformAnalysis");
                    System.Console.Error.WriteLine(a1);
                }
                List<DiagnosticInfo> results = LanguageServer.Analysis.PerformAnalysis(document);
                server.SendDiagnostics(results);
            }
            catch (LanguageServerException e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
            catch (Exception e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
        }

        [JsonRpcMethod("CMReplacePriorization")]
        public void CMReplacePriorization(JToken arg1, JToken arg2, JToken arg3)
        {
            try
            {
                string a1 = arg1.ToObject<string>();
                int a2 = arg2.ToObject<int>();
                int a3 = arg3.ToObject<int>();
                Document document = CheckDoc(new Uri(a1));
                int start = a2;
                int end = a3;
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- CMReplacePriorization");
                    System.Console.Error.WriteLine(a1);
                    (int, int) bs = LanguageServer.Module.GetLineColumn(start, document);
                    System.Console.Error.WriteLine("line " + bs.Item1 + " col " + bs.Item2);
                }
                var s = Transform.ReplacePriorization(start, end, document);
                ApplyChanges("Replace priorization rules", s);
            }
            catch (LanguageServerException e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
            catch (Exception e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
        }

        [JsonRpcMethod("CMUpperLowerCaseLiteral")]
        public void CMUpperLowerCaseLiteral(JToken arg1, JToken arg2, JToken arg3)
        {
            try
            {
                string a1 = arg1.ToObject<string>();
                int a2 = arg2.ToObject<int>();
                int a3 = arg3.ToObject<int>();
                Document document = CheckDoc(new Uri(a1));
                int start = a2;
                int end = a3;
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- CMUpperLowerCaseLiteral");
                    System.Console.Error.WriteLine(a1);
                    (int, int) bs = LanguageServer.Module.GetLineColumn(start, document);
                    System.Console.Error.WriteLine("line " + bs.Item1 + " col " + bs.Item2);
                }
                var s = Transform.UpperLowerCaseLiteral(start, end, document);
                ApplyChanges("Replace literal with upper and lower case literal", s);
            }
            catch (LanguageServerException e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
            catch (Exception e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
        }

        [JsonRpcMethod("CMVersion")]
        public string CMVersion()
        {
            try
            {
                if (trace)
                {
                    System.Console.Error.WriteLine("<-- CMVersion");
                }
                var s = Module.AntlrVersion();
                return s;
            }
            catch (LanguageServerException e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
            catch (Exception e)
            {
                server.ShowMessage(e.Message, MessageType.Info);
            }
            return null;
        }

    }
}
