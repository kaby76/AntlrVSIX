namespace Server
{
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

    public class LSPServer : INotifyPropertyChanged
    {
        private int maxProblems = -1;
        private readonly JsonRpc rpc;
        private readonly LanguageServerTarget target;
        private readonly ManualResetEvent disconnectEvent = new ManualResetEvent(false);
        private Dictionary<string, DiagnosticSeverity> diagnostics;
        private TextDocumentItem textDocument = null;

        private int counter = 100;

        public LSPServer(Stream sender, Stream reader, Dictionary<string, DiagnosticSeverity> initialDiagnostics = null)
        {
            target = new LanguageServerTarget(this);
            rpc = JsonRpc.Attach(sender, reader, target);
            rpc.Disconnected += OnRpcDisconnected;
            diagnostics = initialDiagnostics;
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
            textDocument = messageParams.TextDocument;

            SendDiagnostics();
        }

        public void SetDiagnostics(Dictionary<string, DiagnosticSeverity> diagnostics)
        {
            this.diagnostics = diagnostics;
        }

        public void SendDiagnostics()
        {
            if (textDocument == null || this.diagnostics == null)
            {
                return;
            }

            string[] lines = textDocument.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            List<Diagnostic> diagnostics = new List<Diagnostic>();
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                int j = 0;
                while (j < line.Length)
                {
                    Diagnostic diagnostic = null;
                    foreach (KeyValuePair<string, DiagnosticSeverity> tag in this.diagnostics)
                    {
                        diagnostic = GetDiagnostic(line, i, ref j, tag.Key, tag.Value);

                        if (diagnostic != null)
                        {
                            break;
                        }
                    }

                    if (diagnostic == null)
                    {
                        ++j;
                    }
                    else
                    {
                        diagnostics.Add(diagnostic);
                    }
                }
            }

            PublishDiagnosticParams parameter = new PublishDiagnosticParams
            {
                Uri = textDocument.Uri,
                Diagnostics = diagnostics.ToArray()
            };

            if (maxProblems > -1)
            {
                parameter.Diagnostics = parameter.Diagnostics.Take(maxProblems).ToArray();
            }

            rpc.NotifyWithParameterObjectAsync(Methods.TextDocumentPublishDiagnosticsName, parameter);
        }

        public void SendDiagnostics(string uri, string text)
        {
            string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            List<Diagnostic> diagnostics = new List<Diagnostic>();
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                int j = 0;
                while (j < line.Length)
                {
                    Diagnostic diagnostic = null;
                    foreach (KeyValuePair<string, DiagnosticSeverity> tag in this.diagnostics)
                    {
                        diagnostic = GetDiagnostic(line, i, ref j, tag.Key, tag.Value);

                        if (diagnostic != null)
                        {
                            break;
                        }
                    }

                    if (diagnostic == null)
                    {
                        ++j;
                    }
                    else
                    {
                        diagnostics.Add(diagnostic);
                    }
                }
            }

            PublishDiagnosticParams parameter = new PublishDiagnosticParams
            {
                Uri = new Uri(uri),
                Diagnostics = diagnostics.ToArray()
            };

            if (maxProblems > -1)
            {
                parameter.Diagnostics = parameter.Diagnostics.Take(maxProblems).ToArray();
            }

            rpc.NotifyWithParameterObjectAsync(Methods.TextDocumentPublishDiagnosticsName, parameter);
        }

        public void LogMessage(object arg)
        {
            LogMessage(arg, MessageType.Info);
        }

        public void LogMessage(object arg, MessageType messageType)
        {
            LogMessage(arg, "testing " + counter++, messageType);
        }

        public void LogMessage(object arg, string message, MessageType messageType)
        {
            LogMessageParams parameter = new LogMessageParams
            {
                Message = message,
                MessageType = messageType
            };
            rpc.NotifyWithParameterObjectAsync(Methods.WindowLogMessageName, parameter);
        }

        public bool ApplyEdit(Dictionary<string, TextEdit[]> changes)
        {
            WorkspaceEdit edit = new WorkspaceEdit()
            {
                Changes = changes
            };
            ApplyWorkspaceEditParams parameter = new ApplyWorkspaceEditParams
            {
                Label = "Random change",
                Edit = edit
            };
            var r = rpc.InvokeAsync<ApplyWorkspaceEditResponse>(Methods.WorkspaceApplyEditName, parameter).Result;
            return true;
        }

        public void ShowMessage(string message, MessageType messageType)
        {
            ShowMessageParams parameter = new ShowMessageParams
            {
                Message = message,
                MessageType = messageType
            };
            rpc.NotifyWithParameterObjectAsync(Methods.WindowShowMessageName, parameter);
        }

        public async Task<MessageActionItem> ShowMessageRequestAsync(string message, MessageType messageType, string[] actionItems)
        {
            ShowMessageRequestParams parameter = new ShowMessageRequestParams
            {
                Message = message,
                MessageType = messageType,
                Actions = actionItems.Select(a => new MessageActionItem { Title = a }).ToArray()
            };

            JToken response = await rpc.InvokeWithParameterObjectAsync<JToken>(Methods.WindowShowMessageRequestName, parameter);
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
                SendDiagnostics();
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
            if ((characterOffset + wordToMatch.Length) <= line.Length)
            {
                string subString = line.Substring(characterOffset, wordToMatch.Length);
                if (subString.Equals(wordToMatch, StringComparison.OrdinalIgnoreCase))
                {
                    Diagnostic diagnostic = new Diagnostic
                    {
                        Message = "This is an " + Enum.GetName(typeof(DiagnosticSeverity), severity),
                        Severity = severity,
                        Range = new Microsoft.VisualStudio.LanguageServer.Protocol.Range
                        {
                            Start = new Position(lineOffset, characterOffset),
                            End = new Position(lineOffset, characterOffset + wordToMatch.Length)
                        },
                        Code = "Test" + Enum.GetName(typeof(DiagnosticSeverity), severity)
                    };
                    characterOffset = characterOffset + wordToMatch.Length;

                    return diagnostic;
                }
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
    }
}
