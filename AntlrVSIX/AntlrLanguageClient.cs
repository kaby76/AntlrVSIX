﻿using Microsoft.VisualStudio.LanguageServer.Protocol;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.VisualStudio.LanguageServer.Client;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Threading;
using Microsoft.VisualStudio.Utilities;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LanguageServer;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Newtonsoft.Json.Linq;
using ServiceProvider = Microsoft.VisualStudio.Shell.ServiceProvider;


namespace LspAntlr
{
    [ContentType("Antlr")]
    [Export(typeof(ILanguageClient))]
    public class AntlrLanguageClient : ILanguageClient, ILanguageClientCustomMessage2
    {
        public static MemoryStream _log_to_server = new MemoryStream();
        public static MemoryStream _log_from_server = new MemoryStream();
        private JsonRpc _rpc;
        public static AntlrLanguageClient Instance { get; set; }
        public string Name => "Antlr language extension";

        public IEnumerable<string> ConfigurationSections => null;

        public object InitializationOptions => null;

        public IEnumerable<string> FilesToWatch => null;

        public event AsyncEventHandler<EventArgs> StartAsync;
        public event AsyncEventHandler<EventArgs> StopAsync;

        public AntlrLanguageClient()
        {
            CustomMessageTarget = new CustomTarget();
            Instance = this;
        }

        public async Task<Connection> ActivateAsync(CancellationToken token)
        {
            await Task.Yield();
            try
            {
                string cache_location = System.IO.Path.GetTempPath();
                var t = typeof(AntlrLanguageClient);
                var a = t.Assembly;
                var f = System.IO.Path.GetFullPath(a.Location);
                var p = System.IO.Path.GetDirectoryName(f);
                var antlr_executable = p + System.IO.Path.DirectorySeparatorChar
                                         + @"Server\net472\LanguageServer.Exec.exe";
                //var w2 = new SetWorkspace(cache_location, antlr_executable);
                //w2.ShowDialog();
                //w2.Close();
                //var workspace_path = w2.workspace_path.Text;
                var workspace_path = cache_location;

                if (workspace_path == null || workspace_path == "")
                    workspace_path = cache_location;

                //antlr_executable = w2.lspserver.Text;

                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = antlr_executable;
                info.WorkingDirectory = workspace_path;
                info.Arguments = workspace_path;
                info.RedirectStandardInput = true;
                info.RedirectStandardOutput = true;
                info.UseShellExecute = false;
                info.CreateNoWindow = false;
                Process process = new Process();
                process.StartInfo = info;
                if (process.Start())
                {
                    bool debug = false;
                    var @out = process.StandardOutput.BaseStream;
                    var eout = debug
                        ? new LspTools.LspHelpers.EchoStream(@out, _log_from_server,
                            LspTools.LspHelpers.EchoStream.StreamOwnership.OwnNone)
                        : @out;
                    var @in = process.StandardInput.BaseStream;
                    var ein = debug
                        ? new LspTools.LspHelpers.EchoStream(@in, _log_to_server,
                            LspTools.LspHelpers.EchoStream.StreamOwnership.OwnNone)
                        : @in;

                    return new Connection(eout, ein);
                }
            }
            catch (Exception e)
            {
            }

            return null;
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

        public async Task AttachForCustomMessageAsync(JsonRpc rpc)
        {
            await Task.Yield();
            this._rpc = rpc;
        }

        public object[] SendServerCustomMessage(int start, int end, string ffn)
        {
            if (this._rpc == null) return null;
            DocumentSymbolSpansParams p = new DocumentSymbolSpansParams();
            var uri = new Uri(ffn);
            p.TextDocument = uri;
            p.Start = start;
            p.End = end;
            var result = this._rpc.InvokeAsync<object[]>("KenCustomMessage", p).Result;
            return result;
        }

        public object MiddleLayer => null;

        public object CustomMessageTarget { get; set; }


        public class CustomTarget
        {
            public void OnCustomNotification(JToken arg)
            {
                // Provide logic on what happens OnCustomNotification is called from the language server
            }

            public string OnCustomRequest(string test)
            {
                // Provide logic on what happens OnCustomRequest is called from the language server
                return "";
            }
        }
    }
}