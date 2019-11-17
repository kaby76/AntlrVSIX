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
    [ContentType("java")]
    [Export(typeof(ILanguageClient))]
    public class Class1 : ILanguageClient
    {
        string ILanguageClient.Name => "Java language extension";

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
                // See https://github.com/eclipse/eclipse.jdt.ls for information on the server.
                // Executable in the form of a jar from https://download.eclipse.org/jdtls/snapshots/?d
                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = "c:\\Program Files\\Java\\jdk-11.0.4\\bin\\java.exe";
                info.WorkingDirectory = "c:\\Users\\kenne\\Downloads\\jls";
                var workspace_path = "c:\\Users\\kenne\\Documents\\test";
                info.Arguments = "-Declipse.application=org.eclipse.jdt.ls.core.id1 -Dosgi.bundles.defaultStartLevel=4 -Declipse.product=org.eclipse.jdt.ls.core.product -Dlog.level=ALL -noverify -Xmx1G -jar ./plugins/org.eclipse.equinox.launcher_1.5.600.v20191014-2022.jar -configuration ./config_win -data "
                    + workspace_path
                    + " --add-modules=ALL-SYSTEM --add-opens java.base/java.util=ALL-UNNAMED --add-opens java.base/java.lang=ALL-UNNAMED";
                info.RedirectStandardInput = true;
                info.RedirectStandardOutput = true;
                info.UseShellExecute = false;
                info.CreateNoWindow = true;
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
