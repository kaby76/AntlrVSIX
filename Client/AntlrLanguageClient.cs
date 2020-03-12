namespace LspAntlr
{
    using LanguageServer;
    using Microsoft.VisualStudio.ComponentModelHost;
    using Microsoft.VisualStudio.Editor;
    using Microsoft.VisualStudio.LanguageServer.Client;
    using Microsoft.VisualStudio.LanguageServer.Protocol;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;
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
        public static IVsEditorAdaptersFactoryService AdaptersFactory = null;
        public const string PackageGuidString = "49bf9144-398a-467c-9b87-ac26d1e62737";
        public static MemoryStream _log_from_server = new MemoryStream();
        public static MemoryStream _log_to_server = new MemoryStream();
        private static JsonRpc _rpc;
        public static Microsoft.VisualStudio.OLE.Interop.IServiceProvider XXX;
        private readonly JsonRpcMethodAttribute junk;

        public AntlrLanguageClient()
        {
            Instance = this;
            IComponentModel componentModel = Package.GetGlobalService(typeof(SComponentModel)) as IComponentModel;
            AdaptersFactory = componentModel.GetService<IVsEditorAdaptersFactoryService>();
            object dte2 = Package.GetGlobalService(typeof(SDTE));
            XXX = (Microsoft.VisualStudio.OLE.Interop.IServiceProvider)dte2;
            ServiceProvider sp = new ServiceProvider(XXX);
            //AdaptersFactory = this.GetService(typeof(IVsEditorAdaptersFactoryService)) as IVsEditorAdaptersFactoryService;
            OptionsCommand.Initialize(this);
            AboutCommand.Initialize(this);
            NextSymCommand.Initialize(this);
            GoToVisitorCommand.Initialize(this);
            ReplaceLiteral.Initialize(this);
            RemoveUselessParserProductions.Initialize(this);
            MoveStartRuleToTop.Initialize(this);
            Reorder.Initialize(this);
        }

        public event AsyncEventHandler<EventArgs> StartAsync;

        public event AsyncEventHandler<EventArgs> StopAsync;

        public static AntlrLanguageClient Instance { get; set; }
        public IEnumerable<string> ConfigurationSections => null;
        public object CustomMessageTarget => null;
        public IEnumerable<string> FilesToWatch => null;
        public object InitializationOptions => null;
        public object MiddleLayer => null;
        public string Name => "Antlr language extension";
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
                if (workspace_path == null || workspace_path == "")
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
            catch (Exception)
            {
            }

            return null;
        }

        public async Task AttachForCustomMessageAsync(JsonRpc rpc)
        {
            await Task.Yield();
            _rpc = rpc;
        }

        public async Task OnLoadedAsync()
        {
            await StartAsync.InvokeAsync(this, EventArgs.Empty);
        }

        public Task OnServerInitializedAsync()
        {
            return Task.CompletedTask;
        }

        public Task OnServerInitializeFailedAsync(Exception e)
        {
            return Task.CompletedTask;
        }

        public SymbolInformation[] CMGetClassifiersSendServer(int start, int end, string ffn)
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
                SymbolInformation[] result = _rpc.InvokeAsync<SymbolInformation[]>("CMGetClassifiers", p).Result;
                return result;
            }
            catch (Exception)
            {
            }
            return null;
        }

        public int CMNextSymbolSendServer(string ffn, int pos, bool forward)
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
                int result = _rpc.InvokeAsync<int>("CMNextSymbol", p).Result;
                return result;
            }
            catch (Exception)
            {
            }
            return -1;
        }

        public CMGotoResult CMGotoVisitorSendServer(string ffn, int pos)
        {
            try
            {
                if (_rpc == null)
                {
                    return null;
                }

                CMGotoParams p = new CMGotoParams();
                Uri uri = new Uri(ffn);
                p.TextDocument = uri;
                p.Pos = pos;
                CMGotoResult result = _rpc.InvokeAsync<CMGotoResult>("CMGotoVisitor", p).Result;
                return result;
            }
            catch (Exception)
            {
            }
            return null;
        }

        public CMGotoResult CMGotoListenerSendServer(string ffn, bool is_enter, int pos)
        {
            try
            {
                if (_rpc == null)
                {
                    return null;
                }

                CMGotoParams p = new CMGotoParams();
                Uri uri = new Uri(ffn);
                p.TextDocument = uri;
                p.Pos = pos;
                p.IsEnter = is_enter;
                CMGotoResult result = _rpc.InvokeAsync<CMGotoResult>("CMGotoListener", p).Result;
                return result;
            }
            catch (Exception)
            {
            }
            return null;
        }

        public Dictionary<string, string> CMReplaceLiteralsServer(string ffn, int pos)
        {
            try
            {
                if (_rpc == null)
                {
                    return null;
                }

                CMReplaceLiteralsParams p = new CMReplaceLiteralsParams();
                Uri uri = new Uri(ffn);
                p.TextDocument = uri;
                p.Pos = pos;
                Dictionary<string, string> result = _rpc.InvokeAsync<Dictionary<string, string>>("CMReplaceLiterals", p).Result;
                return result;
            }
            catch (Exception)
            {
            }
            return null;
        }

        public Dictionary<string, string> CMRemoveUselessParserProductionsServer(string ffn, int pos)
        {
            try
            {
                if (_rpc == null)
                {
                    return null;
                }

                CMRemoveUselessParserProductionsParams p = new CMRemoveUselessParserProductionsParams();
                Uri uri = new Uri(ffn);
                p.TextDocument = uri;
                p.Pos = pos;
                Dictionary<string, string> result = _rpc.InvokeAsync<Dictionary<string, string>>("CMRemoveUselessParserProductions", p).Result;
                return result;
            }
            catch (Exception)
            {
            }
            return null;
        }

        public Dictionary<string, string> CMMoveStartRuleToTopServer(string ffn, int pos)
        {
            try
            {
                if (_rpc == null)
                {
                    return null;
                }

                CMMoveStartRuleToTopParams p = new CMMoveStartRuleToTopParams();
                Uri uri = new Uri(ffn);
                p.TextDocument = uri;
                p.Pos = pos;
                Dictionary<string, string> result = _rpc.InvokeAsync<Dictionary<string, string>>("CMMoveStartRuleToTop", p).Result;
                return result;
            }
            catch (Exception)
            {
            }
            return null;
        }

        public Dictionary<string, string> CMReorderParserRulesServer(string ffn, int pos, ReorderType reorder_type)
        {
            try
            {
                if (_rpc == null)
                {
                    return null;
                }

                CMReorderParserRulesParams p = new CMReorderParserRulesParams();
                Uri uri = new Uri(ffn);
                p.TextDocument = uri;
                p.Pos = pos;
                p.Type = reorder_type;

                Dictionary<string, string> result = _rpc.InvokeAsync<Dictionary<string, string>>("CMReorderParserRules", p).Result;
                return result;
            }
            catch (Exception)
            {
            }
            return null;
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
