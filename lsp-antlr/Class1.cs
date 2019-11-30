using Microsoft.VisualStudio.LanguageServer.Client;
using Microsoft.VisualStudio.Threading;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;


namespace VSIXProject1
{
    [ContentType("antlr")]
    [Export(typeof(ILanguageClient))]
    public class Class1 : ILanguageClient
    {
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
                var w2 = new SetWorkspace();
                w2.ShowDialog();
                w2.Close();
                var workspace_path = w2.workspace_path.Text;
                if (workspace_path == null || workspace_path == "")
			        workspace_path = cache_location;
		        var antlr_executable = @"c:\Users\kenne\Documents\AntlrVSIX\LanguageServer.Exec\bin\Debug\netcoreapp3.0\LanguageServer.Exec.exe";
		        var stdInPipeName = @"output";
		        var stdOutPipeName = @"input";
		        var pipeAccessRule = new PipeAccessRule("Everyone", PipeAccessRights.ReadWrite, System.Security.AccessControl.AccessControlType.Allow);
		        var pipeSecurity = new PipeSecurity();
		        pipeSecurity.AddAccessRule(pipeAccessRule);
		        var bufferSize = 256;
		        var readerPipe = new NamedPipeServerStream(stdInPipeName, PipeDirection.InOut, 4, PipeTransmissionMode.Message, PipeOptions.Asynchronous, bufferSize, bufferSize, pipeSecurity);
		        var writerPipe = new NamedPipeServerStream(stdOutPipeName, PipeDirection.InOut, 4, PipeTransmissionMode.Message, PipeOptions.Asynchronous, bufferSize, bufferSize, pipeSecurity);            
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
			        await readerPipe.WaitForConnectionAsync(token);
			        await writerPipe.WaitForConnectionAsync(token);
			        return new Connection(readerPipe, writerPipe);
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
