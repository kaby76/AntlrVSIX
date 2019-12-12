using Microsoft.VisualStudio.LanguageServer.Client;
using Microsoft.VisualStudio.Threading;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Covidimus.IO;


namespace LspClangd
{
    [ContentType("clangd")]
    [Export(typeof(ILanguageClient))]
    public class LspClangdClient : ILanguageClient
    {
        public static MemoryStream _log_to_server = new MemoryStream();
        public static MemoryStream _log_from_server = new MemoryStream();

        string ILanguageClient.Name => "Clangd language extension";

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
                var w2 = new SetWorkspace();
                w2.ShowDialog();
                w2.Close();
                var workspace_path = w2.workspace_path.Text;
                if (workspace_path == null || workspace_path == "")
                    workspace_path = cache_location;
                var clangd_executable = w2._clangd_executable;
                clangd_executable = @"E:\build9\Debug\bin\clangd.exe";
                var executable = @"C:\Users\kenne\source\repos\ProxyLsp\ProxyLsp\bin\Debug\netcoreapp3.0\ProxyLsp.exe";
                //executable = @"E:\build9\Debug\bin\clangd.exe";
                //executable = @"C:\Program Files\LLVM\bin\clangd.exe";
                workspace_path = "C:/Users/kenne/source/repos/ConsoleApplication2/ConsoleApplication2";
                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = executable;
                info.WorkingDirectory = workspace_path;
                info.Arguments = "" 
                                 + clangd_executable
                                 // + " --compile-commands-dir=\""
                                 //  + @"C:\Users\kenne\source\repos\ConsoleApplication2\ConsoleApplication2"
                                 //   + workspace_path //+ "/compile_commands.json"
                                 + " --log=verbose"
                    //+ " --index-file=\"C:/Users/kenne/source/repos/ConsoleApplication2/ConsoleApplication2/clangd.dex\""
                    ;
                //  + " --compile_args_from=filesystem";
                info.RedirectStandardInput = true;
                info.RedirectStandardOutput = true;
                info.UseShellExecute = false;
                info.CreateNoWindow = false;
                Process process = new Process();
                process.StartInfo = info;
                if (process.Start())
                {
                    var @out = process.StandardOutput.BaseStream;
                    var eout = new Covidimus.IO.EchoStream(@out, _log_from_server, EchoStream.StreamOwnership.OwnNone);

                    var @in = process.StandardInput.BaseStream;
                    var ein = new Covidimus.IO.EchoStream(@in, _log_to_server, EchoStream.StreamOwnership.OwnNone);
                    
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
