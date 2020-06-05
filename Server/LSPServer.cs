namespace Server
{
    using LanguageServer;
    using Microsoft.VisualStudio.LanguageServer.Protocol;
    using Newtonsoft.Json.Linq;
    using StreamJsonRpc;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;

    public class LSPServer : INotifyPropertyChanged, IDisposable
    {
        private int maxProblems = -1;
        private readonly JsonRpc rpc;
        private readonly LanguageServerTarget target;
        private readonly ManualResetEvent disconnectEvent = new ManualResetEvent(false);
        private Dictionary<string, DiagnosticSeverity> diagnostics;
        private TextDocumentItem textDocument = null;
        private bool isDisposed;
        private int counter = 100;

        public LSPServer(Stream sender, Stream reader)
        {
            target = new LanguageServerTarget(this);
            rpc = JsonRpc.Attach(sender, reader, target);
            rpc.Disconnected += OnRpcDisconnected;
            diagnostics = new Dictionary<string, DiagnosticSeverity>();
            diagnostics.Add("hithere", DiagnosticSeverity.Error);
        }

        public string CustomText
        {
            get;
            set;
        }

        public string CurrentSettings
        {
            get; private set;
        }

        public event EventHandler Disconnected;
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnInitialized(object sender, EventArgs e)
        {
            Timer timer = new Timer(LogMessage, null, 0, 5 * 1000);
        }

        public void OnTextDocumentOpened(DidOpenTextDocumentParams messageParams)
        {
        }

        public void SetDiagnostics(Dictionary<string, DiagnosticSeverity> diagnostics)
        {
            this.diagnostics = diagnostics;
        }

        public void SendDiagnostics(string str, List<DiagnosticInfo> list)
        {
            List<Diagnostic> diagnostics = new List<Diagnostic>();
            foreach (var info in list)
            {
                DiagnosticSeverity severity = default;
                switch (info.Severify)
                {
                    case DiagnosticInfo.Severity.Info:
                        severity = DiagnosticSeverity.Information;
                        break;
                    case DiagnosticInfo.Severity.Warning:
                        severity = DiagnosticSeverity.Warning;
                        break;
                    case DiagnosticInfo.Severity.Error:
                        severity = DiagnosticSeverity.Error;
                        break;
                }
                var document = LanguageServerTarget.CheckDoc(new Uri(info.Document));
                (int, int) bs = LanguageServer.Module.GetLineColumn(info.Start, document);
                (int, int) be = LanguageServer.Module.GetLineColumn(info.End, document);
                Diagnostic diagnostic = new Diagnostic
                {
                    Message = info.Message,
                    Severity = severity,
                    Range = new Microsoft.VisualStudio.LanguageServer.Protocol.Range
                    {
                        Start = new Position(bs.Item1, bs.Item2),
                        End = new Position(be.Item1, be.Item2)
                    },
                    Code = "Test" + Enum.GetName(typeof(DiagnosticSeverity), severity)
                };
                diagnostics.Add(diagnostic);
            }

            PublishDiagnosticParams parameter = new PublishDiagnosticParams
            {
                Uri = new Uri(str),
                Diagnostics = diagnostics.ToArray()
            };

            if (maxProblems > -1)
            {
                parameter.Diagnostics = parameter.Diagnostics.Take(maxProblems).ToArray();
            }

#pragma warning disable VSTHRD110
            rpc.NotifyWithParameterObjectAsync(Methods.TextDocumentPublishDiagnosticsName, parameter);
#pragma warning restore VSTHRD110
        }

        public void LogMessage(object arg)
        {
            LogMessage(MessageType.Info);
        }

        public void LogMessage(MessageType messageType)
        {
            LogMessage("testing " + counter++, messageType);
        }

        public void LogMessage(string message, MessageType messageType)
        {
            LogMessageParams parameter = new LogMessageParams
            {
                Message = message,
                MessageType = messageType
            };
#pragma warning disable VSTHRD110
            rpc.NotifyWithParameterObjectAsync(Methods.WindowLogMessageName, parameter);
#pragma warning restore VSTHRD110
        }

        public bool ApplyEdit(string transaction_name, Dictionary<string, Microsoft.VisualStudio.LanguageServer.Protocol.TextEdit[]> changes)
        {
            WorkspaceEdit edit = new WorkspaceEdit()
            {
                Changes = changes
            };
            ApplyWorkspaceEditParams parameter = new ApplyWorkspaceEditParams
            {
                Label = transaction_name,
                Edit = edit
            };
            _ = rpc.InvokeAsync<ApplyWorkspaceEditResponse>(Methods.WorkspaceApplyEditName, parameter);
            return true;
        }

        public void ShowMessage(string message, MessageType messageType)
        {
            ShowMessageParams parameter = new ShowMessageParams
            {
                Message = message,
                MessageType = messageType
            };
#pragma warning disable VSTHRD110
            rpc.NotifyWithParameterObjectAsync(Methods.WindowShowMessageName, parameter);
#pragma warning restore VSTHRD110
        }

        public async Task<MessageActionItem> ShowMessageRequestAsync(string message, MessageType messageType, string[] actionItems)
        {
            ShowMessageRequestParams parameter = new ShowMessageRequestParams
            {
                Message = message,
                MessageType = messageType,
                Actions = actionItems.Select(a => new MessageActionItem { Title = a }).ToArray()
            };
            JToken response = await rpc.InvokeWithParameterObjectAsync<JToken>(Methods.WindowShowMessageRequestName, parameter).ConfigureAwait(false); ;
            return response.ToObject<MessageActionItem>();
        }

        public void SendSettings(DidChangeConfigurationParams parameter)
        {
            CurrentSettings = parameter.Settings.ToString();
            NotifyPropertyChanged(nameof(CurrentSettings));

            JToken parsedSettings = JToken.Parse(CurrentSettings);
            int newMaxProblems = parsedSettings.Children().First().Values<int>("maxNumberOfProblems").First();
            if (maxProblems != newMaxProblems)
            {
                maxProblems = newMaxProblems;
            }
        }

        public void WaitForExit()
        {
            disconnectEvent.WaitOne();
        }

        public void Exit()
        {
            disconnectEvent.Set();
            Disconnected?.Invoke(this, new EventArgs());
            System.Environment.Exit(0);
        }

        private Diagnostic GetDiagnostic(string line, int lineOffset, ref int characterOffset, string wordToMatch, DiagnosticSeverity severity)
        {
            if (line.Contains(wordToMatch))
            {
                Diagnostic diagnostic = new Diagnostic
                {
                    Message = "This is an " + Enum.GetName(typeof(DiagnosticSeverity), severity),
                    Severity = severity,
                    Range = new Microsoft.VisualStudio.LanguageServer.Protocol.Range
                    {
                        Start = new Position(0, 0),
                        End = new Position(0, 10)
                    },
                    Code = "Test" + Enum.GetName(typeof(DiagnosticSeverity), severity)
                };
                characterOffset += 100;
                return diagnostic;
            }
            return null;
        }

        private void OnRpcDisconnected(object sender, JsonRpcDisconnectedEventArgs e)
        {
            Exit();
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed) return;
            if (disposing)
            {
                // free managed resources
                disconnectEvent.Dispose();
            }
            isDisposed = true;
        }

        ~LSPServer()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }
    }
}
