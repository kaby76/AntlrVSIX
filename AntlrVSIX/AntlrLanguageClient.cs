using Microsoft.VisualStudio.LanguageServer.Client;
using Microsoft.VisualStudio.Threading;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;


namespace LspAntlr
{
    [ContentType("Antlr")]
    [Export(typeof(ILanguageClient))]
    public class AntlrLanguageClient : ILanguageClient
    {
        public static MemoryStream _log_to_server = new MemoryStream();
        public static MemoryStream _log_from_server = new MemoryStream();

        string ILanguageClient.Name => "Antlr language extension";

        IEnumerable<string> ILanguageClient.ConfigurationSections => null;

        object ILanguageClient.InitializationOptions => null;

        IEnumerable<string> ILanguageClient.FilesToWatch => null;

        public event AsyncEventHandler<EventArgs> StartAsync;
        public event AsyncEventHandler<EventArgs> StopAsync;

        async Task<Connection> ILanguageClient.ActivateAsync(CancellationToken token)
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
                var w2 = new SetWorkspace(cache_location, antlr_executable);
                w2.ShowDialog();
                w2.Close();
                var workspace_path = w2.workspace_path.Text;
                if (workspace_path == null || workspace_path == "")
			        workspace_path = cache_location;
                antlr_executable = w2.lspserver.Text;
		        ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = antlr_executable;
                info.WorkingDirectory = workspace_path;
		        info.Arguments = workspace_path;
                info.RedirectStandardInput = true;
                info.RedirectStandardOutput = true;
                info.UseShellExecute = false;
                info.CreateNoWindow = true;
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

        async Task ILanguageClient.OnLoadedAsync()
        {
            await StartAsync.InvokeAsync(this, EventArgs.Empty);
        }

        Task ILanguageClient.OnServerInitializedAsync()
        {
            return Task.CompletedTask;
        }

        Task ILanguageClient.OnServerInitializeFailedAsync(Exception e)
        {
            return Task.CompletedTask;
        }
    }
}
