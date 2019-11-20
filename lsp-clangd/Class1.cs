using Microsoft.VisualStudio.LanguageServer.Client;
using Microsoft.VisualStudio.Threading;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;


namespace VSIXProject1
{
    [ContentType("clangd")]
    [Export(typeof(ILanguageClient))]
    public class Class1 : ILanguageClient
    {
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
                workspace_path = "C:/Users/kenne/source/repos/ConsoleApplication2/ConsoleApplication2";

                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = clangd_executable;
                info.WorkingDirectory = workspace_path;
                info.Arguments = " --compile-commands-dir=\""
                    + workspace_path //+ "/compile_commands.json"
                    + "\" --log=verbose";
                info.RedirectStandardInput = true;
                info.RedirectStandardOutput = true;
                info.UseShellExecute = false;
                info.CreateNoWindow = false;
                Process process = new Process();
                process.StartInfo = info;
                if (process.Start())
                {
                    return new Connection(process.StandardOutput.BaseStream, process.StandardInput.BaseStream);
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
