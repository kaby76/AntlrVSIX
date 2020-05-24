namespace LspAntlr
{
    using LanguageServer;
    using Microsoft.VisualStudio.ComponentModelHost;
    using Microsoft.VisualStudio.Editor;
    using Microsoft.VisualStudio.LanguageServer.Client;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Threading;
    using Microsoft.VisualStudio.Utilities;
    using Options;
    using StreamJsonRpc;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Threading.Tasks;
    using Task = System.Threading.Tasks.Task;

    [ContentType("Antlr")]
    [Export(typeof(ILanguageClient))]
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(AntlrLanguageClient.PackageGuidString)]
    public class AntlrLanguageClient : AsyncPackage, ILanguageClient, ILanguageClientCustomMessage2
    {
        [Import]
        internal IClassificationFormatMapService ClassificationFormatMapService = null;

        private static IVsEditorAdaptersFactoryService adaptersFactory = null;
        public const string PackageGuidString = "49bf9144-398a-467c-9b87-ac26d1e62737";
        private static readonly MemoryStream _log_from_server = new MemoryStream();
        private static readonly MemoryStream _log_to_server = new MemoryStream();
        private static JsonRpc _rpc;

        public AntlrLanguageClient()
        {
            Logger.Log.CleanUpLogFile();
            Instance = this;
            IComponentModel componentModel = Package.GetGlobalService(typeof(SComponentModel)) as IComponentModel;
            AdaptersFactory = componentModel.GetService<IVsEditorAdaptersFactoryService>();
            //AdaptersFactory = this.GetService(typeof(IVsEditorAdaptersFactoryService)) as IVsEditorAdaptersFactoryService;
            Import.Initialize(this);
            OptionsCommand.Initialize(this);
            AboutCommand.Initialize(this);
            NextSymCommand.Initialize(this);
            GoToVisitorCommand.Initialize(this);
            ReplaceLiteral.Initialize(this);
            RemoveUselessParserProductions.Initialize(this);
            MoveStartRuleToTop.Initialize(this);
            Reorder.Initialize(this);
            SplitCombineGrammars.Initialize(this);
            EliminateDirectLeftRecursion.Initialize(this);
            EliminateIndirectLeftRecursion.Initialize(this);
            EliminateAntlrKeywordsInRules.Initialize(this);
            AddLexerRulesForStringLiterals.Initialize(this);
            SortModes.Initialize(this);
            ConvertRecursionToKleeneOperator.Initialize(this);
            Unfold.Initialize(this);
            Fold.Initialize(this);
            RemoveUselessParentheses.Initialize(this);
            ShowCycles.Initialize(this);
        }

        public event AsyncEventHandler<EventArgs> StartAsync;

#pragma warning disable 0067
        public event AsyncEventHandler<EventArgs> StopAsync;
#pragma warning restore 0067

        public static AntlrLanguageClient Instance { get; set; }
        public IEnumerable<string> ConfigurationSections => null;
        public object CustomMessageTarget => null;
        public IEnumerable<string> FilesToWatch => null;
        public object InitializationOptions => null;
        public object MiddleLayer => null;
        public string Name => "Antlr language extension";

        public static IVsEditorAdaptersFactoryService AdaptersFactory { get => adaptersFactory; set => adaptersFactory = value; }

        public async Task<Connection> ActivateAsync(CancellationToken token)
        {
            await Task.Yield();
            try
            {
                string cache_location = System.IO.Path.GetTempPath();
                Type t = typeof(AntlrLanguageClient);
                System.Reflection.Assembly a = t.Assembly;
                string f = System.IO.Path.GetFullPath(a.Location);
                string p = System.IO.Path.GetDirectoryName(f);
                string antlr_executable = p + System.IO.Path.DirectorySeparatorChar
                                         + @"Server\net472\Server.exe";
                string workspace_path = cache_location;
                if (string.IsNullOrEmpty(workspace_path))
                {
                    workspace_path = cache_location;
                }

                ProcessStartInfo info = new ProcessStartInfo
                {
                    FileName = antlr_executable,
                    WorkingDirectory = workspace_path,
                    Arguments = workspace_path,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = !Option.GetBoolean("VisibleServerWindow")
                };
                Process process = new Process
                {
                    StartInfo = info
                };
                if (process.Start())
                {
                    bool debug = false;
                    Stream @out = process.StandardOutput.BaseStream;
                    Stream eout = debug
                        ? new LspTools.LspHelpers.EchoStream(@out, _log_from_server,
                            LspTools.LspHelpers.EchoStream.StreamOwnership.OwnNone)
                        : @out;
                    Stream @in = process.StandardInput.BaseStream;
                    Stream ein = debug
                        ? new LspTools.LspHelpers.EchoStream(@in, _log_to_server,
                            LspTools.LspHelpers.EchoStream.StreamOwnership.OwnNone)
                        : @in;

                    return new Connection(eout, ein);
                }
            }
            catch (Exception eeks)
            {
                Logger.Log.Notify(eeks.ToString());
            }

            return null;
        }

        public async Task AttachForCustomMessageAsync(JsonRpc rpc)
        {
            await Task.Yield();
            _rpc = rpc;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task OnLoadedAsync()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            _ = StartAsync.InvokeAsync(this, EventArgs.Empty);
        }

        public Task OnServerInitializedAsync()
        {
            return Task.CompletedTask;
        }

        public Task OnServerInitializeFailedAsync(Exception e)
        {
            return Task.CompletedTask;
        }

        public static CMClassifierInformation[] CMGetClassifiers(int start, int end, string ffn)
        {
            try
            {
                if (_rpc == null)
                {
                    return null;
                }
                CMGetClassifiersParams p = new CMGetClassifiersParams();
                Uri uri = new Uri(ffn);
                p.TextDocument = uri;
                p.Start = start;
                p.End = end;
                //CMClassifierInformation[] result;
                var context = ThreadHelper.JoinableTaskContext;
                var jtf = new JoinableTaskFactory(context);
                var result = jtf.Run(() => _rpc.InvokeAsync<CMClassifierInformation[]>("CMGetClassifiers", p));
                return result;
            }
            catch (Exception)
            {
            }
            return null;
        }

        public static int CMNextSymbol(string ffn, int pos, bool forward)
        {
            try
            {
                if (_rpc == null)
                {
                    return -1;
                }

                CMNextSymbolParams p = new CMNextSymbolParams();
                Uri uri = new Uri(ffn);
                p.TextDocument = uri;
                p.Pos = pos;
                p.Forward = forward;
                var context = ThreadHelper.JoinableTaskContext;
                var jtf = new JoinableTaskFactory(context);
                var result = jtf.Run(() => _rpc.InvokeAsync<int>("CMNextSymbol", p));
                return result;
            }
            catch (Exception)
            {
            }
            return -1;
        }

        public static CMGotoResult CMGotoVisitor(string ffn, int pos)
        {
            try
            {
                if (_rpc == null) return null;
                CMGotoParams p = new CMGotoParams();
                Uri uri = new Uri(ffn);
                p.TextDocument = uri;
                p.Pos = pos;
                var context = ThreadHelper.JoinableTaskContext;
                var jtf = new JoinableTaskFactory(context);
                var result = jtf.Run(() => _rpc.InvokeAsync<CMGotoResult>("CMGotoVisitor", p));
                return result;
            }
            catch (Exception)
            {
            }
            return null;
        }

        public static CMGotoResult CMGotoListener(string ffn, bool is_enter, int pos)
        {
            try
            {
                if (_rpc == null) return null;
                CMGotoParams p = new CMGotoParams();
                Uri uri = new Uri(ffn);
                p.TextDocument = uri;
                p.Pos = pos;
                p.IsEnter = is_enter;
                var context = ThreadHelper.JoinableTaskContext;
                var jtf = new JoinableTaskFactory(context);
                var result = jtf.Run(() => _rpc.InvokeAsync<CMGotoResult>("CMGotoListener", p));
                return result;
            }
            catch (Exception)
            {
            }
            return null;
        }

        public static void CMReplaceLiterals(string ffn, int pos)
        {
            try
            {
                if (_rpc == null) return;
                CMReplaceLiteralsParams p = new CMReplaceLiteralsParams();
                Uri uri = new Uri(ffn);
                p.TextDocument = uri;
                p.Pos = pos;
                _ = _rpc.InvokeAsync("CMReplaceLiterals", p);
            }
            catch (Exception)
            {
            }
        }

        public static void CMRemoveUselessParserProductions(string ffn, int pos)
        {
            try
            {
                if (_rpc == null) return;
                CMRemoveUselessParserProductionsParams p = new CMRemoveUselessParserProductionsParams();
                Uri uri = new Uri(ffn);
                p.TextDocument = uri;
                p.Pos = pos;
                _ = _rpc.InvokeAsync("CMRemoveUselessParserProductions", p);
            }
            catch (Exception)
            {
            }
        }

        public static void CMMoveStartRuleToTop(string ffn, int pos)
        {
            try
            {
                if (_rpc == null) return;
                CMMoveStartRuleToTopParams p = new CMMoveStartRuleToTopParams();
                Uri uri = new Uri(ffn);
                p.TextDocument = uri;
                p.Pos = pos;
                _ = _rpc.InvokeAsync("CMMoveStartRuleToTop", p);
            }
            catch (Exception)
            {
            }
        }

        public static void CMReorderParserRules(string ffn, int pos, ReorderType reorder_type)
        {
            try
            {
                if (_rpc == null) return;
                CMReorderParserRulesParams p = new CMReorderParserRulesParams();
                Uri uri = new Uri(ffn);
                p.TextDocument = uri;
                p.Type = reorder_type;
                _ = _rpc.InvokeAsync("CMReorderParserRules", p);
            }
            catch (Exception)
            {
            }
        }

        public static Dictionary<string, string> CMSplitCombineGrammars(string ffn, int pos, bool split)
        {
            try
            {
                if (_rpc == null) return null;
                CMSplitCombineGrammarsParams p = new CMSplitCombineGrammarsParams();
                Uri uri = new Uri(ffn);
                p.TextDocument = uri;
                p.Pos = pos;
                p.Split = split;

                var context = ThreadHelper.JoinableTaskContext;
                var jtf = new JoinableTaskFactory(context);
                var result = jtf.Run(() => _rpc.InvokeAsync<Dictionary<string, string>>("CMSplitCombineGrammars", p));
                return result;
            }
            catch (Exception)
            {
            }
            return null;
        }

        public static Dictionary<string, string> CMImportGrammars(List<string> ffn)
        {
            try
            {
                if (_rpc == null) return null;
                List<string> p = ffn;
                var context = ThreadHelper.JoinableTaskContext;
                var jtf = new JoinableTaskFactory(context);
                var result = jtf.Run(() => _rpc.InvokeAsync<Dictionary<string, string>>("CMImportGrammars", p));
                return result;
            }
            catch (Exception)
            {
            }
            return null;
        }

        public static void CMEliminateDirectLeftRecursion(string ffn, int pos)
        {
            try
            {
                if (_rpc == null) return;
                CMEliminateDirectLeftRecursionParams p = new CMEliminateDirectLeftRecursionParams();
                Uri uri = new Uri(ffn);
                p.TextDocument = uri;
                p.Pos = pos;
                _ = _rpc.InvokeAsync("CMEliminateDirectLeftRecursion", p);
            }
            catch (Exception)
            {
            }
        }

        public static void CMEliminateIndirectLeftRecursion(string ffn, int pos)
        {
            try
            {
                if (_rpc == null) return;
                CMEliminateDirectLeftRecursionParams p = new CMEliminateDirectLeftRecursionParams();
                Uri uri = new Uri(ffn);
                p.TextDocument = uri;
                p.Pos = pos;
                _ = _rpc.InvokeAsync("CMEliminateIndirectLeftRecursion", p);
            }
            catch (Exception)
            {
            }
        }
        
        public static void CMConvertRecursionToKleeneOperator(string ffn, int pos)
        {
            try
            {
                if (_rpc == null) return;
                CMEliminateDirectLeftRecursionParams p = new CMEliminateDirectLeftRecursionParams();
                Uri uri = new Uri(ffn);
                p.TextDocument = uri;
                p.Pos = pos;
                _ = _rpc.InvokeAsync("CMConvertRecursionToKleeneOperator", p);
            }
            catch (Exception)
            {
            }
        }

        public static void CMEliminateAntlrKeywordsInRules(string ffn)
        {
            try
            {
                if (_rpc == null) return;
                Uri uri = new Uri(ffn);
                var p = uri;
                _ = _rpc.InvokeAsync("CMEliminateAntlrKeywordsInRules", p);
            }
            catch (Exception)
            {
            }
        }

        public static void CMAddLexerRulesForStringLiterals(string ffn)
        {
            try
            {
                if (_rpc == null) return;
                Uri uri = new Uri(ffn);
                var p = uri;
                _ = _rpc.InvokeAsync("CMAddLexerRulesForStringLiterals", p);
            }
            catch (Exception)
            {
            }
        }

        public static void CMSortModes(string ffn)
        {
            try
            {
                if (_rpc == null) return;
                Uri uri = new Uri(ffn);
                var p = uri;
                _ = _rpc.InvokeAsync("CMSortModes", p);
            }
            catch (Exception)
            {
            }
        }

        public static void CMUnfold(string ffn, int pos)
        {
            try
            {
                if (_rpc == null) return;
                _ = _rpc.InvokeAsync("CMUnfold", ffn, pos);
            }
            catch (Exception)
            {
            }
        }

        public static void CMFold(string ffn, int start, int end)
        {
            try
            {
                if (_rpc == null) return;
                _ = _rpc.InvokeAsync("CMFold", ffn, start, end);
            }
            catch (Exception)
            {
            }
        }

        public static void CMRemoveUselessParentheses(string ffn, int start, int end)
        {
            try
            {
                if (_rpc == null) return;
                _ = _rpc.InvokeAsync("CMRemoveUselessParentheses", ffn, start, end);
            }
            catch (Exception)
            {
            }
        }

        public static void CMShowCycles(string ffn, int start)
        {
            try
            {
                if (_rpc == null) return;
                _ = _rpc.InvokeAsync("CMShowCycles", ffn, start);
            }
            catch (Exception)
            {
            }
        }

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            // When initialized asynchronously, the current thread may be a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            //await Command1.InitializeAsync(this);
        }
    }
}
