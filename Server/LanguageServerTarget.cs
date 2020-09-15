namespace Server
{
    using LanguageServer;
    using LoggerNs;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Protocol;
    using StreamJsonRpc;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using Workspaces;
    using DocumentSymbol = LanguageServer.DocumentSymbol;

    public class LanguageServerTarget
    {
        private readonly LSPServer server;
        private readonly bool trace = true;
        private readonly Workspaces.Workspace _workspace;
        private static readonly object _object = new object();
        private readonly Dictionary<string, bool> ignore_next_change = new Dictionary<string, bool>();
        private int current_version;

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
            var a = new Dictionary<string, Protocol.TextEdit[]>();
            foreach (var pair in ch)
            {
                var fn = pair.Key;
                var new_code = pair.Value;
                Document document = CheckDoc(fn);
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

                List<Protocol.TextEdit> new_list = new List<Protocol.TextEdit>();
                int count = 0;
                foreach (LanguageServer.TextEdit delta in changes)
                {
                    var new_edit = new Protocol.TextEdit
                    {
                        Range = new Protocol.Range()
                    };
                    (int, int) lcs = new LanguageServer.Module().GetLineColumn(delta.range.Start.Value, document);
                    (int, int) lce = new LanguageServer.Module().GetLineColumn(delta.range.End.Value, document);
                    new_edit.Range.Start = new Position(lcs.Item1, lcs.Item2);
                    new_edit.Range.End = new Position(lce.Item1, lce.Item2);
                    new_edit.NewText = delta.NewText;
                    new_list.Add(new_edit);
                    count++;
                }
                var result = new_list.ToArray();
                a[fn] = result;
                ignore_next_change[fn] = true;
                // This must be done after computing changes since offsets/line/column computations
                // depend on what is currently the source.
                document.Code = new_code;
            }
            // Recompile only after every single change everywhere is in.
            _ = new LanguageServer.Module().Compile();
            server.ApplyEdit(transaction_name, a);
        }

        [JsonRpcMethod(Methods.InitializeName)]
        public object Initialize(JToken arg)
        {
            lock (_object)
            {
                if (trace)
                {
                    Logger.Log.WriteLine("<-- Initialize");
                    Logger.Log.WriteLine(arg.ToString());
                }
                MyServerCapabilities capabilities = new MyServerCapabilities
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

                    SemanticTokensProvider = new SemanticTokensOptions()
                    {
                        full = true,
                        range = false,
                        legend = new SemanticTokensLegend()
                        {
                            tokenTypes = new string[] {
                                "class",
                                "variable",
                                "enum",
                                "comment",
                                "string",
                                "keyword",
                            },
                            tokenModifiers = new string[] {
                                "declaration",
                                "documentation",
                            }
                        }
                    },
                };

                MyInitializeResult result = new MyInitializeResult
                {
                    Capabilities = capabilities
                };
                string json = JsonConvert.SerializeObject(result);
                if (trace)
                {
                    Logger.Log.WriteLine("--> " + json);
                }
                return result;
            }
        }

        [JsonRpcMethod(Methods.InitializedName)]
        public void InitializedName(JToken arg)
        {
            lock (_object)
            {
                try
                {
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- Initialized");
                        Logger.Log.WriteLine(arg.ToString());
                    }
                }
                catch (Exception)
                { }
            }
        }

        [JsonRpcMethod(Methods.ShutdownName)]
        public JToken ShutdownName()
        {
            lock (_object)
            {
                try
                {
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- Shutdown");
                    }
                }
                catch (Exception)
                { }
                return null;
            }
        }

        [JsonRpcMethod(Methods.ExitName)]
        public void ExitName()
        {
            lock (_object)
            {
                try
                {
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- Exit");
                    }
                    server.Exit();
                }
                catch (Exception)
                { }
            }
        }

        // ======= WINDOW ========

        [JsonRpcMethod(Methods.WorkspaceDidChangeConfigurationName)]
        public void WorkspaceDidChangeConfigurationName(JToken arg)
        {
            lock (_object)
            {
                try
                {
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- WorkspaceDidChangeConfiguration");
                        Logger.Log.WriteLine(arg.ToString());
                    }
                    //var parameter = arg.ToObject<DidChangeConfigurationParams>();
                    //this.server.SendSettings(parameter);
                }
                catch (Exception)
                { }
            }
        }

        [JsonRpcMethod(Methods.WorkspaceDidChangeWatchedFilesName)]
        public void WorkspaceDidChangeWatchedFilesName(JToken arg)
        {
            lock (_object)
            {
                try
                {
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- WorkspaceDidChangeWatchedFiles");
                        Logger.Log.WriteLine(arg.ToString());
                    }
                }
                catch (Exception)
                { }
            }
        }

        [JsonRpcMethod(Methods.WorkspaceSymbolName)]
        public JToken WorkspaceSymbolName(JToken arg)
        {
            lock (_object)
            {
                try
                {
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- WorkspaceSymbol");
                        Logger.Log.WriteLine(arg.ToString());
                    }
                }
                catch (Exception)
                { }
                return null;
            }
        }

        [JsonRpcMethod(Methods.WorkspaceExecuteCommandName)]
        public JToken WorkspaceExecuteCommandName(JToken arg)
        {
            lock (_object)
            {
                try
                {
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- WorkspaceExecuteCommand");
                        Logger.Log.WriteLine(arg.ToString());
                    }
                }
                catch (Exception)
                { }
                return null;
            }
        }

        [JsonRpcMethod(Methods.WorkspaceApplyEditName)]
        public JToken WorkspaceApplyEditName(JToken arg)
        {
            lock (_object)
            {
                if (trace)
                {
                    Logger.Log.WriteLine("<-- WorkspaceApplyEdit");
                    Logger.Log.WriteLine(arg.ToString());
                }
                return null;
            }
        }

        // ======= TEXT SYNCHRONIZATION ========

        [JsonRpcMethod(Methods.TextDocumentDidOpenName)]
        public void TextDocumentDidOpenName(JToken arg)
        {
            lock (_object)
            {
                if (trace)
                {
                    Logger.Log.WriteLine("<-- TextDocumentDidOpen");
                    Logger.Log.WriteLine(arg.ToString());
                }
                DidOpenTextDocumentParams request = arg.ToObject<DidOpenTextDocumentParams>();
                var document = CheckDoc(request.TextDocument.Uri);
                document.Code = request.TextDocument.Text;
                List<ParsingResults> to_do = new LanguageServer.Module().Compile();
            }
        }

        [JsonRpcMethod(Methods.TextDocumentDidChangeName)]
        public void TextDocumentDidChangeName(JToken arg)
        {
            lock (_object)
            {
                if (trace)
                {
                    Logger.Log.WriteLine("<-- TextDocumentDidChange");
                    Logger.Log.WriteLine(arg.ToString());
                }
                DidChangeTextDocumentParams request = arg.ToObject<DidChangeTextDocumentParams>();
                Document document = CheckDoc(request.TextDocument.Uri);

                // Create a thread to handle this update in the correct order.
                //Thread thread = new Thread(() =>
                {
                    try
                    {
                        //       if (request.TextDocument?.Version != null)
                        {
                            int version = (int)request.TextDocument.Version;
                            // spin until this version is current_version.
                            // for (; ; )
                            {
                                //  if (first_change || version == current_version)
                                {
                                    if (!ignore_next_change.ContainsKey(document.FullPath))
                                    {
                                        ParsingResults pd = ParsingResultsFactory.Create(document);
                                        string code = pd.Code;
                                        if (trace)
                                        {
                                            Logger.Log.WriteLine("making change, code before");
                                            Logger.Log.WriteLine(code);
                                            Logger.Log.WriteLine("----------------------");
                                        }
                                        int start_index = 0;
                                        int end_index = 0;
                                        foreach (TextDocumentContentChangeEvent change in request.ContentChanges)
                                        {
                                            var range = change.Range;
                                            int length = change.RangeLength; // Why? range encodes start and end => length!
                                            string text = change.Text;
                                            {
                                                int line = range.Start.Line;
                                                int character = range.Start.Character;
                                                start_index = new LanguageServer.Module().GetIndex(line, character, document);
                                            }
                                            {
                                                int line = range.End.Line;
                                                int character = range.End.Character;
                                                end_index = new LanguageServer.Module().GetIndex(line, character, document);
                                            }
                                            (int, int) bs = new LanguageServer.Module().GetLineColumn(start_index, document);
                                            (int, int) be = new LanguageServer.Module().GetLineColumn(end_index, document);
                                            string original = code.Substring(start_index, end_index - start_index);
                                            string n = code.Substring(0, start_index)
                                                    + text
                                                    + code.Substring(0 + start_index + end_index - start_index);
                                            code = n;
                                        }
                                        if (trace)
                                        {
                                            Logger.Log.WriteLine("making change, code after");
                                            Logger.Log.WriteLine(code);
                                            Logger.Log.WriteLine("----------------------");
                                        }
                                        document.Code = code;
                                        List<ParsingResults> to_do = new LanguageServer.Module().Compile();
                                    }
                                    else
                                    {
                                        ignore_next_change.Remove(document.FullPath);
                                    }
                                    current_version = version + 1;
                                    //first_change = false;

                                    //          break;
                                }
                                //        Thread.Sleep(100);
                            }
                        }
                    }
                    catch (ThreadAbortException)
                    {
                        Thread.ResetAbort();
                    }
                    // })
                    //      {
                    //          IsBackground = true
                    //      };

                    //       thread.Start();
                }
            }
        }

        [JsonRpcMethod(Methods.TextDocumentWillSaveName)]
        public void TextDocumentWillSaveName(JToken arg)
        {
            lock (_object)
            {
                if (trace)
                {
                    Logger.Log.WriteLine("<-- TextDocumentWillSave");
                    Logger.Log.WriteLine(arg.ToString());
                }
                // Nothing to do--who cares because the server does not perform a save.
            }
        }

        [JsonRpcMethod(Methods.TextDocumentWillSaveWaitUntilName)]
        public JToken TextDocumentWillSaveWaitUntilName(JToken arg)
        {
            lock (_object)
            {
                if (trace)
                {
                    Logger.Log.WriteLine("<-- TextDocumentWillSaveWaitUntil");
                    Logger.Log.WriteLine(arg.ToString());
                }
                // Nothing to do--who cares because the server does not perform a save, and
                // the server doesn't manufacture edit requests out of thin air.
                return null;
            }
        }

        [JsonRpcMethod(Methods.TextDocumentDidSaveName)]
        public void TextDocumentDidSaveName(JToken arg)
        {
            lock (_object)
            {
                if (trace)
                {
                    Logger.Log.WriteLine("<-- TextDocumentDidSave");
                    Logger.Log.WriteLine(arg.ToString());
                }
                // Nothing to do--who cares because the server does not perform a save.
            }
        }

        [JsonRpcMethod(Methods.TextDocumentDidCloseName)]
        public void TextDocumentDidCloseName(JToken arg)
        {
            lock (_object)
            {
                if (trace)
                {
                    Logger.Log.WriteLine("<-- TextDocumentDidClose");
                    Logger.Log.WriteLine(arg.ToString());
                }
                // Nothing to do--who cares.
            }
        }

        // ======= DIAGNOSTICS ========

        [JsonRpcMethod(Methods.TextDocumentPublishDiagnosticsName)]
        public void TextDocumentPublishDiagnosticsName(JToken arg)
        {
            lock (_object)
            {
                if (trace)
                {
                    Logger.Log.WriteLine("<-- TextDocumentPublishDiagnostics");
                    Logger.Log.WriteLine(arg.ToString());
                }
            }
        }

        // ======= LANGUAGE FEATURES ========

        [JsonRpcMethod(Methods.TextDocumentCompletionName)]
        public object[] TextDocumentCompletionName(JToken arg)
        {
            lock (_object)
            {
                try
                {
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- TextDocumentCompletion");
                        Logger.Log.WriteLine(arg.ToString());
                    }
                    CompletionParams request = arg.ToObject<CompletionParams>();
                    Document document = CheckDoc(request.TextDocument.Uri);
                    CompletionContext context = request.Context;
                    Position position = request.Position;
                    int line = position.Line;
                    int character = position.Character;
                    int char_index = new LanguageServer.Module().GetIndex(line, character, document);
                    if (trace)
                    {
                        Logger.Log.WriteLine("position index = " + char_index);
                        (int, int) back = new LanguageServer.Module().GetLineColumn(char_index, document);
                        Logger.Log.WriteLine("back to l,c = " + back.Item1 + "," + back.Item2);
                    }
                    List<string> res = new LanguageServer.Module().Completion(char_index, document);
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
        }

        [JsonRpcMethod(Methods.TextDocumentCompletionResolveName)]
        public JToken TextDocumentCompletionResolveName(JToken arg)
        {
            lock (_object)
            {
                if (trace)
                {
                    Logger.Log.WriteLine("<-- TextDocumentCompletionResolve");
                    Logger.Log.WriteLine(arg.ToString());
                }
                return null;
            }
        }

        public static Document CheckDoc(string uri)
        {
            string file_name = uri.Replace("file:///", "");
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
                _ = ParsingResultsFactory.Create(document);
                _ = new LanguageServer.Module().Compile();
            }
            return document;
        }

        [JsonRpcMethod(Methods.TextDocumentHoverName)]
        public object TextDocumentHoverName(JToken arg)
        {
            lock (_object)
            {
                Hover hover = null;
                try
                {
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- TextDocumentHover");
                        Logger.Log.WriteLine(arg.ToString());
                    }
                    TextDocumentPositionParams request = arg.ToObject<TextDocumentPositionParams>();
                    Document document = CheckDoc(request.TextDocument.Uri);
                    Position position = request.Position;
                    int line = position.Line;
                    int character = position.Character;
                    int index = new LanguageServer.Module().GetIndex(line, character, document);
                    if (trace)
                    {
                        Logger.Log.WriteLine("position index = " + index);
                        (int, int) back = new LanguageServer.Module().GetLineColumn(index, document);
                        Logger.Log.WriteLine("back to l,c = " + back.Item1 + "," + back.Item2);
                    }
                    QuickInfo quick_info = new LanguageServer.Module().GetQuickInfo(index, document);
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
                    (int, int) lcs = new LanguageServer.Module().GetLineColumn(index_start, document);
                    (int, int) lce = new LanguageServer.Module().GetLineColumn(index_end, document);
                    hover.Range = new Protocol.Range
                    {
                        Start = new Position(lcs.Item1, lcs.Item2),
                        End = new Position(lce.Item1, lce.Item2)
                    };
                    Logger.Log.WriteLine("returning " + quick_info.Display);
                }
                catch (Exception)
                { }
                return hover;
            }
        }

        [JsonRpcMethod(Methods.TextDocumentSignatureHelpName)]
        public JToken TextDocumentSignatureHelpName(JToken arg)
        {
            lock (_object)
            {
                try
                {
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- TextDocumentSignatureHelp");
                        Logger.Log.WriteLine(arg.ToString());
                    }
                }
                catch (Exception)
                { }
                return null;
            }
        }

        // TextDocumentDeclaration does not exist in Microsoft.VisualStudio.LanguageServer.Protocol 16.3.57
        // but does in version 3.14 of LSP.

        [JsonRpcMethod(Methods.TextDocumentDefinitionName)]
        public object[] TextDocumentDefinitionName(JToken arg)
        {
            lock (_object)
            {
                object[] result = null;
                try
                {
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- TextDocumentDefinition");
                        Logger.Log.WriteLine(arg.ToString());
                    }
                    TextDocumentPositionParams request = arg.ToObject<TextDocumentPositionParams>();
                    Document document = CheckDoc(request.TextDocument.Uri);
                    Position position = request.Position;
                    int line = position.Line;
                    int character = position.Character;
                    int index = new LanguageServer.Module().GetIndex(line, character, document);
                    if (trace)
                    {
                        Logger.Log.WriteLine("position index = " + index);
                        (int, int) back = new LanguageServer.Module().GetLineColumn(index, document);
                        Logger.Log.WriteLine("back to l,c = " + back.Item1 + "," + back.Item2);
                    }
                    var found = new LanguageServer.Module().FindDefs(index, document);
                    List<object> locations = new List<object>();
                    foreach (var f in found)
                    {
                        Protocol.Location location = new Protocol.Location
                        {
                            Uri = f.Uri.FullPath
                        };
                        Document def_document = _workspace.FindDocument(f.Uri.FullPath);
                        location.Range = new Protocol.Range();
                        (int, int) lcs = new LanguageServer.Module().GetLineColumn(f.Range.Start.Value, def_document);
                        (int, int) lce = new LanguageServer.Module().GetLineColumn(f.Range.End.Value, def_document);
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
        }

        [JsonRpcMethod(Methods.TextDocumentTypeDefinitionName)]
        public object[] TextDocumentTypeDefinitionName(JToken arg)
        {
            lock (_object)
            {
                object[] result = null;
                try
                {
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- TextDocumentTypeDefinitionName");
                        Logger.Log.WriteLine(arg.ToString());
                    }
                    TextDocumentPositionParams request = arg.ToObject<TextDocumentPositionParams>();
                    Document document = CheckDoc(request.TextDocument.Uri);
                    Position position = request.Position;
                    int line = position.Line;
                    int character = position.Character;
                    int index = new LanguageServer.Module().GetIndex(line, character, document);
                    if (trace)
                    {
                        Logger.Log.WriteLine("position index = " + index);
                        (int, int) back = new LanguageServer.Module().GetLineColumn(index, document);
                        Logger.Log.WriteLine("back to l,c = " + back.Item1 + "," + back.Item2);
                    }
                    var found = new LanguageServer.Module().FindDefs(index, document);
                    List<object> locations = new List<object>();
                    foreach (var f in found)
                    {
                        Protocol.Location location = new Protocol.Location
                        {
                            Uri = f.Uri.FullPath
                        };
                        Document def_document = _workspace.FindDocument(f.Uri.FullPath);
                        location.Range = new Protocol.Range();
                        (int, int) lcs = new LanguageServer.Module().GetLineColumn(f.Range.Start.Value, def_document);
                        (int, int) lce = new LanguageServer.Module().GetLineColumn(f.Range.End.Value, def_document);
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
        }

        [JsonRpcMethod(Methods.TextDocumentImplementationName)]
        public object[] TextDocumentImplementationName(JToken arg)
        {
            lock (_object)
            {
                object[] result = null;
                try
                {
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- TextDocumentImplementation");
                        Logger.Log.WriteLine(arg.ToString());
                    }
                    TextDocumentPositionParams request = arg.ToObject<TextDocumentPositionParams>();
                    Document document = CheckDoc(request.TextDocument.Uri);
                    Position position = request.Position;
                    int line = position.Line;
                    int character = position.Character;
                    int index = new LanguageServer.Module().GetIndex(line, character, document);
                    if (trace)
                    {
                        Logger.Log.WriteLine("position index = " + index);
                        (int, int) back = new LanguageServer.Module().GetLineColumn(index, document);
                        Logger.Log.WriteLine("back to l,c = " + back.Item1 + "," + back.Item2);
                    }
                    var found = new LanguageServer.Module().FindDefs(index, document);
                    List<object> locations = new List<object>();
                    foreach (var f in found)
                    {
                        var location = new Protocol.Location
                        {
                            Uri = f.Uri.FullPath
                        };
                        Document def_document = _workspace.FindDocument(f.Uri.FullPath);
                        location.Range = new Protocol.Range();
                        (int, int) lcs = new LanguageServer.Module().GetLineColumn(f.Range.Start.Value, def_document);
                        (int, int) lce = new LanguageServer.Module().GetLineColumn(f.Range.End.Value, def_document);
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
        }

        [JsonRpcMethod(Methods.TextDocumentReferencesName)]
        public object[] TextDocumentReferencesName(JToken arg)
        {
            lock (_object)
            {
                object[] result = null;
                try
                {
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- TextDocumentReferences");
                        Logger.Log.WriteLine(arg.ToString());
                    }
                    TextDocumentPositionParams request = arg.ToObject<TextDocumentPositionParams>();
                    Document document = CheckDoc(request.TextDocument.Uri);
                    Position position = request.Position;
                    int line = position.Line;
                    int character = position.Character;
                    int index = new LanguageServer.Module().GetIndex(line, character, document);
                    if (trace)
                    {
                        Logger.Log.WriteLine("position index = " + index);
                        (int, int) back = new LanguageServer.Module().GetLineColumn(index, document);
                        Logger.Log.WriteLine("back to l,c = " + back.Item1 + "," + back.Item2);
                    }
                    var found = new LanguageServer.Module().FindRefsAndDefs(index, document);
                    List<object> locations = new List<object>();
                    foreach (var f in found)
                    {
                        var location = new Protocol.Location
                        {
                            Uri = f.Uri.FullPath
                        };
                        Document def_document = _workspace.FindDocument(f.Uri.FullPath);
                        location.Range = new Protocol.Range();
                        (int, int) lcs = new LanguageServer.Module().GetLineColumn(f.Range.Start.Value, def_document);
                        (int, int) lce = new LanguageServer.Module().GetLineColumn(f.Range.End.Value + 1, def_document);
                        location.Range.Start = new Position(lcs.Item1, lcs.Item2);
                        location.Range.End = new Position(lce.Item1, lce.Item2);
                        locations.Add(location);
                    }
                    result = locations.ToArray();
                    if (trace)
                    {
                        Logger.Log.Write("returning ");
                        Logger.Log.WriteLine(string.Join(
                            System.Environment.NewLine, result.Select(s =>
                        {
                            var v = (Protocol.Location)s;
                            var dd = CheckDoc(v.Uri);
                            return "<" + v.Uri +
                                ",[" + new LanguageServer.Module().GetIndex(
                                    v.Range.Start.Line,
                                    v.Range.Start.Character,
                                    dd)
                                + ".."
                                + new LanguageServer.Module().GetIndex(
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
                    Logger.Log.WriteLine("Exception: " + eeks.ToString());
                }
                return result;
            }
        }

        [JsonRpcMethod(Methods.TextDocumentDocumentHighlightName)]
        public object[] TextDocumentDocumentHighlightName(JToken arg)
        {
            lock (_object)
            {
                object[] result = null;
                try
                {
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- TextDocumentDocumentHighlight");
                        Logger.Log.WriteLine(arg.ToString());
                    }
                    TextDocumentPositionParams request = arg.ToObject<TextDocumentPositionParams>();
                    Document document = CheckDoc(request.TextDocument.Uri);
                    Position position = request.Position;
                    int line = position.Line;
                    int character = position.Character;
                    int index = new LanguageServer.Module().GetIndex(line, character, document);
                    if (trace)
                    {
                        Logger.Log.WriteLine("position index = " + index);
                        (int, int) back = new LanguageServer.Module().GetLineColumn(index, document);
                        Logger.Log.WriteLine("back to l,c = " + back.Item1 + "," + back.Item2);
                    }
                    var found = new LanguageServer.Module().FindRefsAndDefs(index, document);
                    List<object> locations = new List<object>();
                    foreach (var f in found)
                    {
                        if (f.Uri.FullPath != document.FullPath)
                        {
                            continue;
                        }
                        DocumentHighlight location = new DocumentHighlight();
                        Document def_document = _workspace.FindDocument(f.Uri.FullPath);
                        location.Range = new Protocol.Range();
                        (int, int) lcs = new LanguageServer.Module().GetLineColumn(f.Range.Start.Value, def_document);
                        (int, int) lce = new LanguageServer.Module().GetLineColumn(f.Range.End.Value + 1, def_document);
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
        }

        [JsonRpcMethod(Methods.TextDocumentDocumentSymbolName)]
        public object[] TextDocumentDocumentSymbolName(JToken arg)
        {
            lock (_object)
            {
                object[] result = null;
                try
                {
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- TextDocumentDocumentSymbol");
                        Logger.Log.WriteLine(arg.ToString());
                    }
                    DocumentSymbolParams request = arg.ToObject<DocumentSymbolParams>();
                    Document document = CheckDoc(request.TextDocument.Uri);
                    IEnumerable<DocumentSymbol> r = new LanguageServer.Module().GetSymbols(document);
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
                        si.Location = new Protocol.Location
                        {
                            Uri = request.TextDocument.Uri
                        };
                        (int, int) lcs = new LanguageServer.Module().GetLineColumn(s.range.Start.Value, document);
                        (int, int) lce = new LanguageServer.Module().GetLineColumn(s.range.End.Value, document);
                        si.Location.Range = new Protocol.Range
                        {
                            Start = new Position(lcs.Item1, lcs.Item2),
                            End = new Position(lce.Item1, lce.Item2)
                        };
                        symbols.Add(si);
                    }
                    if (trace)
                    {
                        Logger.Log.Write("returning ");
                        Logger.Log.WriteLine(string.Join(" ", symbols.Select(s =>
                        {
                            SymbolInformation v = (SymbolInformation)s;
                            return "<" + v.Name + "," + v.Kind
                                + ",[" + new LanguageServer.Module().GetIndex(
                                    v.Location.Range.Start.Line,
                                    v.Location.Range.Start.Character,
                                    document)
                                + ".."
                                + new LanguageServer.Module().GetIndex(
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
        }

        [JsonRpcMethod(Methods.TextDocumentCodeActionName)]
        public JToken TextDocumentCodeActionName(JToken arg)
        {
            lock (_object)
            {
                try
                {
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- TextDocumentCodeAction");
                        Logger.Log.WriteLine(arg.ToString());
                    }
                }
                catch (Exception)
                { }
                return null;
            }
        }

        [JsonRpcMethod(Methods.TextDocumentCodeLensName)]
        public JToken TextDocumentCodeLensName(JToken arg)
        {
            lock (_object)
            {
                try
                {
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- TextDocumentCodeLens");
                        Logger.Log.WriteLine(arg.ToString());
                    }
                }
                catch (Exception)
                { }
                return null;
            }
        }

        [JsonRpcMethod(Methods.CodeLensResolveName)]
        public JToken CodeLensResolveName(JToken arg)
        {
            lock (_object)
            {
                try
                {
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- CodeLensResolve");
                        Logger.Log.WriteLine(arg.ToString());
                    }
                }
                catch (Exception)
                { }
                return null;
            }
        }

        [JsonRpcMethod(Methods.TextDocumentDocumentLinkName)]
        public JToken TextDocumentDocumentLinkName(JToken arg)
        {
            lock (_object)
            {
                try
                {
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- TextDocumentDocumentLink");
                        Logger.Log.WriteLine(arg.ToString());
                    }
                }
                catch (Exception)
                { }
                return null;
            }
        }

        [JsonRpcMethod(Methods.DocumentLinkResolveName)]
        public JToken DocumentLinkResolveName(JToken arg)
        {
            lock (_object)
            {
                try
                {
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- DocumentLinkResolve");
                        Logger.Log.WriteLine(arg.ToString());
                    }
                }
                catch (Exception)
                { }
                return null;
            }
        }

        [JsonRpcMethod(Methods.TextDocumentFormattingName)]
        public object[] TextDocumentFormattingName(JToken arg)
        {
            lock (_object)
            {
                object[] result = null;
                try
                {
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- TextDocumentFormatting");
                        Logger.Log.WriteLine(arg.ToString());
                    }
                    DocumentFormattingParams request = arg.ToObject<DocumentFormattingParams>();
                    Document document = CheckDoc(request.TextDocument.Uri);
                    var new_list = new List<Protocol.TextEdit>();
                    LanguageServer.TextEdit[] changes = new LanguageServer.Module().Reformat(document);
                    int count = 0;
                    foreach (LanguageServer.TextEdit delta in changes)
                    {
                        var new_edit = new Protocol.TextEdit
                        {
                            Range = new Protocol.Range()
                        };
                        (int, int) lcs = new LanguageServer.Module().GetLineColumn(delta.range.Start.Value, document);
                        (int, int) lce = new LanguageServer.Module().GetLineColumn(delta.range.End.Value, document);
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
        }

        [JsonRpcMethod(Methods.TextDocumentRangeFormattingName)]
        public JToken TextDocumentRangeFormattingName(JToken arg)
        {
            lock (_object)
            {
                try
                {
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- TextDocumentRangeFormatting");
                        Logger.Log.WriteLine(arg.ToString());
                    }
                }
                catch (Exception)
                { }
                return null;
            }
        }

        [JsonRpcMethod(Methods.TextDocumentOnTypeFormattingName)]
        public JToken TextDocumentOnTypeFormattingName(JToken arg)
        {
            lock (_object)
            {
                try
                {
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- TextDocumentOnTypeFormatting");
                        Logger.Log.WriteLine(arg.ToString());
                    }
                }
                catch (Exception)
                { }
                return null;
            }
        }

        [JsonRpcMethod(Methods.TextDocumentRenameName)]
        public WorkspaceEdit TextDocumentRenameName(JToken arg)
        {
            lock (_object)
            {
                if (trace)
                {
                    Logger.Log.WriteLine("<-- TextDocumentRename");
                    Logger.Log.WriteLine(arg.ToString());
                }
                WorkspaceEdit edit = null;
                try
                {
                    RenameParams request = arg.ToObject<RenameParams>();
                    Position position = request.Position;
                    int line = position.Line;
                    int character = position.Character;
                    Document document = CheckDoc(request.TextDocument.Uri);
                    if (!ignore_next_change.ContainsKey(document.FullPath))
                    {
                        int index = new LanguageServer.Module().GetIndex(line, character, document);
                        if (trace)
                        {
                            Logger.Log.WriteLine("position index = " + index);
                            (int, int) back = new LanguageServer.Module().GetLineColumn(index, document);
                            Logger.Log.WriteLine("back to l,c = " + back.Item1 + "," + back.Item2);
                        }
                        string new_name = request.NewName;
                        Dictionary<string, LanguageServer.TextEdit[]> changes = new LanguageServer.Module().Rename(index, new_name, document);
                        edit = new WorkspaceEdit();
                        int count = 0;
                        var edit_changes_array = new Dictionary<string, Protocol.TextEdit[]>();
                        foreach (KeyValuePair<string, LanguageServer.TextEdit[]> pair in changes)
                        {
                            string doc = pair.Key;
                            Uri uri = new Uri(doc);
                            LanguageServer.TextEdit[] val = pair.Value;
                            var new_list = new List<Protocol.TextEdit>();
                            foreach (LanguageServer.TextEdit v in val)
                            {
                                var new_edit = new Protocol.TextEdit
                                {
                                    Range = new Protocol.Range()
                                };
                                (int, int) lcs = new LanguageServer.Module().GetLineColumn(v.range.Start.Value, document);
                                (int, int) lce = new LanguageServer.Module().GetLineColumn(v.range.End.Value, document);
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

                return edit;
            }
        }

        [JsonRpcMethod(Methods.TextDocumentFoldingRangeName)]
        public JToken TextDocumentFoldingRangeName(JToken arg)
        {
            lock (_object)
            {
                try
                {
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- TextDocumentFoldingRange");
                        Logger.Log.WriteLine(arg.ToString());
                    }
                }
                catch (Exception)
                { }
                return null;
            }
        }


        [JsonRpcMethod("CMGetClassifiers")]
        public CMClassifierInformation[] CMGetClassifiers(JToken arg1, JToken arg2, JToken arg3)
        {
            lock (_object)
            {
                CMClassifierInformation[] result = null;
                try
                {
                    var a1 = arg1.ToObject<string>();
                    var a2 = arg2.ToObject<int>();
                    var a3 = arg3.ToObject<int>();
                    Document document = CheckDoc(a1);
                    var start = a2;
                    var end = a3;
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- CMGetClassifiers");
                        Logger.Log.WriteLine(start.ToString());
                        (int, int) bs = new LanguageServer.Module().GetLineColumn(start, document);
                        Logger.Log.WriteLine(end.ToString());
                        (int, int) be = new LanguageServer.Module().GetLineColumn(start, document);
                    }
                    IEnumerable<Module.Info> r = new LanguageServer.Module().Get(start, end, document);
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
                        Logger.Log.Write("returning ");
                        Logger.Log.WriteLine(string.Join(" ", symbols.Select(s =>
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
        }

        [JsonRpcMethod("CMNextSymbol")]
        public int CMNextSymbol(JToken arg1, JToken arg2, JToken arg3)
        {
            lock (_object)
            {
                var ffn = arg1.ToObject<string>();
                var pos = arg2.ToObject<int>();
                var forward = arg3.ToObject<bool>();
                int next_sym = forward ? int.MaxValue : -1;
                try
                {
                    Document document = CheckDoc(ffn);
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- CMNextSymbol");
                        Logger.Log.WriteLine(pos.ToString());
                        (int, int) bs = new LanguageServer.Module().GetLineColumn(pos, document);
                        Logger.Log.WriteLine("");
                    }
                    var r = new LanguageServer.Module().GetDefs(document);
                    List<object> symbols = new List<object>();
                    foreach (var s in r)
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
                        Logger.Log.Write("returning ");
                        Logger.Log.WriteLine(next_sym.ToString());
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
        }

        [JsonRpcMethod("CMGotoVisitor")]
        public CMGotoResult CMGotoVisitor(JToken arg1, JToken arg2, JToken arg3)
        {
            lock (_object)
            {
                CMGotoResult s = null;
                try
                {
                    string a1 = arg1.ToObject<string>();
                    Document document = CheckDoc(a1);
                    int a2 = arg2.ToObject<int>();
                    bool a3 = arg3.ToObject<bool>();
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- CMGotoVisitor");
                        Logger.Log.WriteLine(a1.ToString());
                        (int, int) bs = new LanguageServer.Module().GetLineColumn(a2, document);
                        Logger.Log.WriteLine("");
                    }
                    s = Goto.main(true, a3, document, a2);
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
        }

        [JsonRpcMethod("CMGotoListener")]
        public CMGotoResult CMGotoListener(JToken arg1, JToken arg2, JToken arg3)
        {
            lock (_object)
            {
                CMGotoResult s = null;
                try
                {
                    string a1 = arg1.ToObject<string>();
                    Document document = CheckDoc(a1);
                    int a2 = arg2.ToObject<int>();
                    bool a3 = arg3.ToObject<bool>();
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- CMGotoListener");
                        Logger.Log.WriteLine(a1.ToString());
                        (int, int) bs = new LanguageServer.Module().GetLineColumn(a2, document);
                        Logger.Log.WriteLine("");
                    }
                    s = Goto.main(false, a3, document, a2);
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
        }

        [JsonRpcMethod("CMReplaceLiterals")]
        public void CMReplaceLiterals(JToken arg1, JToken arg2, JToken arg3)
        {
            lock (_object)
            {
                try
                {
                    string a1 = arg1.ToObject<string>();
                    int a2 = arg2.ToObject<int>();
                    int a3 = arg3.ToObject<int>();
                    Document document = CheckDoc(a1);
                    int start = a2;
                    int end = a3;
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- CMReplaceLiterals");
                        Logger.Log.WriteLine(a1);
                        (int, int) bs = new LanguageServer.Module().GetLineColumn(start, document);
                        Logger.Log.WriteLine("line " + bs.Item1 + " col " + bs.Item2);
                        (int, int) be = new LanguageServer.Module().GetLineColumn(end, document);
                        Logger.Log.WriteLine("line " + be.Item1 + " col " + be.Item2);
                    }
                    var s = LanguageServer.Transform.ReplaceLiterals(start, end, document);
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
        }

        [JsonRpcMethod("CMRemoveUselessParserProductions")]
        public void CMRemoveUselessParserProductions(JToken arg1, JToken arg2, JToken arg3)
        {
            lock (_object)
            {
                try
                {
                    string a1 = arg1.ToObject<string>();
                    int a2 = arg2.ToObject<int>();
                    int a3 = arg3.ToObject<int>();
                    Document document = CheckDoc(a1);
                    int start = a2;
                    int end = a3;
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- CMRemoveUselessParserProductions");
                        Logger.Log.WriteLine(start.ToString());
                        (int, int) bs = new LanguageServer.Module().GetLineColumn(start, document);
                        Logger.Log.WriteLine("");
                    }
                    var s = LanguageServer.Transform.RemoveUselessParserProductions(start, end, document);
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
        }

        [JsonRpcMethod("CMMoveStartRuleToTop")]
        public void CMMoveStartRuleToTop(JToken arg1)
        {
            lock (_object)
            {
                try
                {
                    string a1 = arg1.ToObject<string>();
                    Document document = CheckDoc(a1);
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- CMMoveStartRuleToTop");
                        Logger.Log.WriteLine(a1);
                        Logger.Log.WriteLine("");
                    }
                    var s = LanguageServer.Transform.MoveStartRuleToTop(document);
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
        }

        [JsonRpcMethod("CMReorderParserRules")]
        public void CMReorderParserRules(JToken arg1, JToken arg2)
        {
            lock (_object)
            {
                try
                {
                    var a1 = arg1.ToObject<string>();
                    Document document = CheckDoc(a1);
                    var type = arg2.ToObject<LspAntlr.ReorderType>();
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- CMReorderParserRules");
                        Logger.Log.WriteLine(a1);
                        Logger.Log.WriteLine(type.ToString());
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
        }

        [JsonRpcMethod("CMSplitCombineGrammars")]
        public Dictionary<string, string> CMSplitCombineGrammars(JToken arg1, JToken arg2)
        {
            lock (_object)
            {
                Dictionary<string, string> changes = null;
                try
                {
                    var a1 = arg1.ToObject<string>();
                    Document document = CheckDoc(a1);
                    var type = arg2.ToObject<LspAntlr.ReorderType>();
                    var split = arg1.ToObject<bool>();
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- CMSplitCombineGrammars");
                        Logger.Log.WriteLine(a1);
                        Logger.Log.WriteLine(split.ToString());
                        Logger.Log.WriteLine("");
                    }
                    changes = LanguageServer.Transform.SplitCombineGrammars(document, split);
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
        }

        [JsonRpcMethod("CMImportGrammars")]
        public Dictionary<string, string> CMImportGrammars(JToken arg)
        {
            lock (_object)
            {
                Dictionary<string, string> changes = null;
                try
                {
                    List<string> request = arg.ToObject<List<string>>();
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- CMImportGrammars");
                        Logger.Log.WriteLine(arg.ToString());
                    }
                    //       changes = LanguageServer.BisonImport.ImportGrammars(request);
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
        }

        [JsonRpcMethod("CMConvertRecursionToKleeneOperator")]
        public void CMConvertRecursionToKleeneOperator(JToken arg1, JToken arg2, JToken arg3)
        {
            lock (_object)
            {
                try
                {
                    string a1 = arg1.ToObject<string>();
                    int a2 = arg2.ToObject<int>();
                    int a3 = arg3.ToObject<int>();
                    Document document = CheckDoc(a1);
                    int start = a2;
                    int end = a3;
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- CMConvertRecursionToKleeneOperator");
                        Logger.Log.WriteLine(a1);
                        (int, int) bs = new LanguageServer.Module().GetLineColumn(start, document);
                        Logger.Log.WriteLine("line " + bs.Item1 + " col " + bs.Item2);
                        (int, int) be = new LanguageServer.Module().GetLineColumn(end, document);
                        Logger.Log.WriteLine("line " + be.Item1 + " col " + be.Item2);
                    }
                    var s = Transform.ConvertRecursionToKleeneOperator(start, end, document);
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
        }


        [JsonRpcMethod("CMEliminateDirectLeftRecursion")]
        public void CMEliminateDirectLeftRecursion(JToken arg1, JToken arg2, JToken arg3)
        {
            lock (_object)
            {
                try
                {
                    string a1 = arg1.ToObject<string>();
                    int a2 = arg2.ToObject<int>();
                    int a3 = arg3.ToObject<int>();
                    Document document = CheckDoc(a1);
                    int start = a2;
                    int end = a3;
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- CMEliminateDirectLeftRecursion");
                        Logger.Log.WriteLine(a1);
                        (int, int) bs = new LanguageServer.Module().GetLineColumn(start, document);
                        Logger.Log.WriteLine("line " + bs.Item1 + " col " + bs.Item2);
                        (int, int) be = new LanguageServer.Module().GetLineColumn(end, document);
                        Logger.Log.WriteLine("line " + be.Item1 + " col " + be.Item2);
                    }
                    var s = Transform.EliminateDirectLeftRecursion(start, end, document);
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
        }

        [JsonRpcMethod("CMEliminateIndirectLeftRecursion")]
        public void CMEliminateIndirectLeftRecursion(JToken arg1, JToken arg2, JToken arg3)
        {
            lock (_object)
            {
                try
                {
                    string a1 = arg1.ToObject<string>();
                    int a2 = arg2.ToObject<int>();
                    int a3 = arg3.ToObject<int>();
                    Document document = CheckDoc(a1);
                    int start = a2;
                    int end = a3;
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- CMEliminateIndirectLeftRecursion");
                        Logger.Log.WriteLine(a1);
                        (int, int) bs = new LanguageServer.Module().GetLineColumn(start, document);
                        Logger.Log.WriteLine("line " + bs.Item1 + " col " + bs.Item2);
                        (int, int) be = new LanguageServer.Module().GetLineColumn(end, document);
                        Logger.Log.WriteLine("line " + be.Item1 + " col " + be.Item2);
                    }
                    var s = Transform.EliminateIndirectLeftRecursion(start, end, document);
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
        }

        [JsonRpcMethod("CMEliminateAntlrKeywordsInRules")]
        public void CMEliminateAntlrKeywordsInRules(JToken arg)
        {
            lock (_object)
            {
                try
                {
                    var request = arg.ToObject<string>();
                    Document document = CheckDoc(request);
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- CMEliminateAntlrKeywordsInRules");
                        Logger.Log.WriteLine(arg.ToString());
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
        }


        [JsonRpcMethod("CMAddLexerRulesForStringLiterals")]
        public void CMAddLexerRulesForStringLiterals(JToken arg1, JToken arg2, JToken arg3)
        {
            lock (_object)
            {
                try
                {
                    string a1 = arg1.ToObject<string>();
                    int a2 = arg2.ToObject<int>();
                    int a3 = arg3.ToObject<int>();
                    Document document = CheckDoc(a1);
                    int start = a2;
                    int end = a3;
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- CMAddLexerRulesForStringLiterals");
                        Logger.Log.WriteLine(a1);
                        (int, int) bs = new LanguageServer.Module().GetLineColumn(start, document);
                        Logger.Log.WriteLine("line " + bs.Item1 + " col " + bs.Item2);
                        (int, int) be = new LanguageServer.Module().GetLineColumn(end, document);
                        Logger.Log.WriteLine("line " + be.Item1 + " col " + be.Item2);
                    }
                    var s = Transform.AddLexerRulesForStringLiterals(start, end, document);
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
        }

        [JsonRpcMethod("CMSortModes")]
        public void CMSortModes(JToken arg)
        {
            lock (_object)
            {
                try
                {
                    string request = arg.ToObject<string>();
                    Document document = CheckDoc(request);
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- CMSortModes");
                        Logger.Log.WriteLine(arg.ToString());
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
        }

        [JsonRpcMethod("CMUnfold")]
        public void CMUnfold(JToken arg1, JToken arg2, JToken arg3)
        {
            lock (_object)
            {
                try
                {
                    string a1 = arg1.ToObject<string>();
                    int a2 = arg2.ToObject<int>();
                    int a3 = arg3.ToObject<int>();
                    Document document = CheckDoc(a1);
                    int start = a2;
                    int end = a3;
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- CMUnfold");
                        Logger.Log.WriteLine(a1);
                        (int, int) bs = new LanguageServer.Module().GetLineColumn(start, document);
                        Logger.Log.WriteLine("line " + bs.Item1 + " col " + bs.Item2);
                        (int, int) be = new LanguageServer.Module().GetLineColumn(end, document);
                        Logger.Log.WriteLine("line " + be.Item1 + " col " + be.Item2);
                    }
                    var s = Transform.Unfold(start, end, document);
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
        }

        [JsonRpcMethod("CMFold")]
        public void CMFold(JToken arg1, JToken arg2, JToken arg3)
        {
            lock (_object)
            {
                try
                {
                    string a1 = arg1.ToObject<string>();
                    int a2 = arg2.ToObject<int>();
                    int a3 = arg3.ToObject<int>();
                    Document document = CheckDoc(a1);
                    int start = a2;
                    int end = a3;
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- CMFold");
                        Logger.Log.WriteLine(a1);
                        (int, int) bs = new LanguageServer.Module().GetLineColumn(start, document);
                        Logger.Log.WriteLine("line " + bs.Item1 + " col " + bs.Item2);
                        (int, int) be = new LanguageServer.Module().GetLineColumn(end, document);
                        Logger.Log.WriteLine("line " + be.Item1 + " col " + be.Item2);
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
        }

        [JsonRpcMethod("CMRemoveUselessParentheses")]
        public void CMRemoveUselessParentheses(JToken arg1, JToken arg2, JToken arg3)
        {
            lock (_object)
            {
                try
                {
                    string a1 = arg1.ToObject<string>();
                    int a2 = arg2.ToObject<int>();
                    int a3 = arg3.ToObject<int>();
                    Document document = CheckDoc(a1);
                    int start = a2;
                    int end = a3;
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- CMRemoveUselessParentheses");
                        Logger.Log.WriteLine(a1);
                        (int, int) bs = new LanguageServer.Module().GetLineColumn(start, document);
                        Logger.Log.WriteLine("line " + bs.Item1 + " col " + bs.Item2);
                        (int, int) be = new LanguageServer.Module().GetLineColumn(end, document);
                        Logger.Log.WriteLine("line " + be.Item1 + " col " + be.Item2);
                    }
                    var s = Transform.RemoveUselessParentheses(document);
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
        }

        [JsonRpcMethod("CMPerformAnalysis")]
        public void CMPerformAnalysis(JToken arg1)
        {
            lock (_object)
            {
                try
                {
                    string a1 = arg1.ToObject<string>();
                    Document document = CheckDoc(a1);
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- CMPerformAnalysis");
                        Logger.Log.WriteLine(a1);
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
        }

        [JsonRpcMethod("CMReplacePriorization")]
        public void CMReplacePriorization(JToken arg1, JToken arg2, JToken arg3)
        {
            lock (_object)
            {
                try
                {
                    string a1 = arg1.ToObject<string>();
                    int a2 = arg2.ToObject<int>();
                    int a3 = arg3.ToObject<int>();
                    Document document = CheckDoc(a1);
                    int start = a2;
                    int end = a3;
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- CMReplacePriorization");
                        Logger.Log.WriteLine(a1);
                        (int, int) bs = new LanguageServer.Module().GetLineColumn(start, document);
                        Logger.Log.WriteLine("line " + bs.Item1 + " col " + bs.Item2);
                        (int, int) be = new LanguageServer.Module().GetLineColumn(end, document);
                        Logger.Log.WriteLine("line " + be.Item1 + " col " + be.Item2);
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
        }

        [JsonRpcMethod("CMUpperLowerCaseLiteral")]
        public void CMUpperLowerCaseLiteral(JToken arg1, JToken arg2, JToken arg3)
        {
            lock (_object)
            {
                try
                {
                    string a1 = arg1.ToObject<string>();
                    int a2 = arg2.ToObject<int>();
                    int a3 = arg3.ToObject<int>();
                    Document document = CheckDoc(a1);
                    int start = a2;
                    int end = a3;
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- CMUpperLowerCaseLiteral");
                        Logger.Log.WriteLine(a1);
                        (int, int) bs = new LanguageServer.Module().GetLineColumn(start, document);
                        Logger.Log.WriteLine("line " + bs.Item1 + " col " + bs.Item2);
                        (int, int) be = new LanguageServer.Module().GetLineColumn(end, document);
                        Logger.Log.WriteLine("line " + be.Item1 + " col " + be.Item2);
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
        }

        [JsonRpcMethod("CMVersion")]
        public string CMVersion()
        {
            lock (_object)
            {
                try
                {
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- CMVersion");
                    }
                    var s = new LanguageServer.Module().AntlrVersion();
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

        [JsonRpcMethod(Methods.TextDocumentSemanticTokensFull)]
        public SemanticTokens SemanticTokens(JToken arg)
        {
            lock (_object)
            {
                SemanticTokens result = null;
                try
                {
                    if (trace)
                    {
                        Logger.Log.WriteLine("<-- SemanticTokens");
                        Logger.Log.WriteLine(arg.ToString());
                    }
                    DocumentSymbolParams request = arg.ToObject<DocumentSymbolParams>();
                    Document document = CheckDoc(request.TextDocument.Uri);
                    var r = new LanguageServer.Module().Get(document);
                    List<object> symbols = new List<object>();
                    List<int> data = new List<int>();
                    // Let us fill up temp values to figure out.
                    int start = 0;
                    var new_r = r.ToList();
                    foreach (var s in new_r)
                    {
                        int kind;
                        if (s.kind == 0)
                        {
                            // Parser symbol
                            kind = 0;
                        }
                        else if (s.kind == 1)
                        {
                            // Lexer symbol
                            kind = 1;
                        }
                        else if (s.kind == 2)
                        {
                            // Comment
                            kind = 3;
                        }
                        else if (s.kind == 3)
                        {
                            // Keyword
                            kind = 5;
                        }
                        else if (s.kind == 4)
                        {
                            // Literal
                            kind = 4;
                        }
                        else
                        {
                            continue;
                        }

                        (int, int) lc_start = new LanguageServer.Module().GetLineColumn(start, document);
                        (int, int) lcs = new LanguageServer.Module().GetLineColumn(s.start, document);
                        (int, int) lce = new LanguageServer.Module().GetLineColumn(s.end, document);

                        var diff_l = lcs.Item1 - lc_start.Item1;
                        var diff_c = diff_l != 0 ? lcs.Item2 : lcs.Item2 - lc_start.Item2;
                        // line
                        data.Add(diff_l);
                        // startChar
                        data.Add(diff_c);
                        // length
                        data.Add(s.end - s.start + 1);
                        // tokenType
                        data.Add(kind);
                        // tokenModifiers
                        data.Add(0);

                        start = s.start;
                    }
                    result = new SemanticTokens();
                    result.data = data.ToArray();
                    if (trace)
                    {
                        Logger.Log.Write("returning semantictokens");
                        Logger.Log.WriteLine(string.Join(" ", data));
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
                return result;
            }
        }
    }
}
