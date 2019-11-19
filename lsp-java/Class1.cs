using Microsoft.VisualStudio.LanguageServer.Client;
using Microsoft.VisualStudio.Threading;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Runtime.InteropServices;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using System.ComponentModel;
using System.Windows.Forms;


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
                string cache_location = System.IO.Path.GetTempPath();
                var w2 = new SetWorkspace();
                w2.ShowDialog();
                w2.Close();
                var workspace_path = w2.workspace_path.Text;
                if (workspace_path == null || workspace_path == "")
                    workspace_path = cache_location;
                var java_home = w2._java_home;
                var java_executable = w2._java_executable;

                // See https://github.com/eclipse/eclipse.jdt.ls for information on the server.
                // Executable in the form of a jar from https://download.eclipse.org/jdtls/snapshots/?d
                string compressed_server_jar_name = w2.eclipse_jdt.Text;
                string compressed_server_jar_full_path = cache_location + compressed_server_jar_name;
                string decompressed_location = cache_location + "jdt-language-server\\";
                if (!System.IO.File.Exists(compressed_server_jar_full_path))
                {
                    GetJar w = new GetJar();
                    w.Download("http://download.eclipse.org/jdtls/snapshots/" + compressed_server_jar_name,
                        compressed_server_jar_full_path,
                        decompressed_location);
                    w.ShowDialog();
                    w.Close();
                }
                // Look for jar file.
                string relative_path_eclipse_jar = null;
                foreach (var file in Directory.GetFiles(decompressed_location + "plugins"))
                {
                    if (file.Contains("equinox.launcher_"))
                    {
                        var f = Path.GetFileName(file);
                        relative_path_eclipse_jar = "./plugins/" + f;
                        break;
                    }
                }
                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = java_executable;
                info.WorkingDirectory = decompressed_location;
                info.Arguments = "-Declipse.application=org.eclipse.jdt.ls.core.id1 -Dosgi.bundles.defaultStartLevel=4 -Declipse.product=org.eclipse.jdt.ls.core.product -Dlog.level=ALL -noverify -Xmx1G -jar "
                    + relative_path_eclipse_jar
                    + " -configuration ./config_win -data "
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
