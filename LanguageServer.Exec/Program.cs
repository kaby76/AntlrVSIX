using System;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Pipes;
using System.Linq;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using StreamJsonRpc;


namespace LanguageServer.Exec
{
    class Program
    {
        private string logMessage;
        private ObservableCollection<DiagnosticTag> tags = new ObservableCollection<DiagnosticTag>();
        private LSPServer languageServer;
        private string responseText;
        private string currentSettings;
        private MessageType messageType;

        public static void Main(string[] args)
        {
            TimeSpan delay = new TimeSpan(0, 0, 0, 20);
            Console.Error.WriteLine("Waiting " + delay + " seconds...");
            Thread.Sleep((int)delay.TotalMilliseconds);
            Console.Error.WriteLine("Starting");
            var program = new Program();
            program.MainAsync(args).GetAwaiter().GetResult();
        }

        async Task MainAsync(string[] args)
        {
            var stdin = Console.OpenStandardInput();
            var stdout = Console.OpenStandardOutput();
            this.languageServer = new LSPServer(stdout, stdin);
            this.languageServer.Disconnected += OnDisconnected;
            this.languageServer.PropertyChanged += OnLanguageServerPropertyChanged;
            Tags.Add(new DiagnosticTag());
            this.LogMessage = string.Empty;
            this.ResponseText = string.Empty;
            await Task.Delay(-1);
        }

        private void OnLanguageServerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("CurrentSettings"))
            {
                this.CurrentSettings = this.languageServer.CurrentSettings;
            }
        }

        internal void SendLogMessage()
        {
            this.languageServer.LogMessage(arg: null, message: this.LogMessage, messageType: this.MessageType);
        }

        internal void SendMessage()
        {
            this.languageServer.ShowMessage(message: this.LogMessage, messageType: this.MessageType);
        }

        internal void SendMessageRequest()
        {
            Task.Run(async () =>
            {
                MessageActionItem selectedAction = await this.languageServer.ShowMessageRequestAsync(message: this.LogMessage, messageType: this.MessageType, actionItems: new string[] { "option 1", "option 2", "option 3" });
                this.ResponseText = $"The user selected: {selectedAction?.Title ?? "cancelled"}";
            });
        }

        private void OnDisconnected(object sender, System.EventArgs e)
        {
            //MediaTypeNames.Application.Current.Dispatcher.BeginInvokeShutdown(DispatcherPriority.Normal);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<DiagnosticTag> Tags
        {
            get { return this.tags; }
        }

        public string LogMessage
        {
            get
            {
                return this.logMessage;
            }
            set
            {
                this.logMessage = value;
                this.NotifyPropertyChanged(nameof(LogMessage));
            }
        }

        public string ResponseText
        {
            get
            {
                return this.responseText;
            }
            set
            {
                this.responseText = value;
                this.NotifyPropertyChanged(nameof(ResponseText));
            }
        }

        public string CustomText
        {
            get
            {
                return this.languageServer.CustomText;
            }
            set
            {
                this.languageServer.CustomText = value;
                this.NotifyPropertyChanged(nameof(CustomText));
            }
        }

        public MessageType MessageType
        {
            get
            {
                return this.messageType;
            }
            set
            {
                this.messageType = value;
                this.NotifyPropertyChanged(nameof(MessageType));
            }
        }

        public string CurrentSettings
        {
            get
            {
                return this.currentSettings;
            }
            set
            {
                this.currentSettings = value;
                this.NotifyPropertyChanged(nameof(CurrentSettings));
            }
        }

        public void SendDiagnostics()
        {
            var diagnosticTags = Tags.ToDictionary((d) => d.Text, (d) => d.Severity);
            this.languageServer.SetDiagnostics(diagnosticTags);
            this.languageServer.SendDiagnostics();
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class DiagnosticTag : INotifyPropertyChanged
    {
        private string text;
        private DiagnosticSeverity severity;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Text
        {
            get { return this.text; }
            set
            {
                this.text = value;
                OnPropertyChanged(nameof(Text));
            }
        }

        public DiagnosticSeverity Severity
        {
            get { return this.severity; }
            set
            {
                this.severity = value;
                OnPropertyChanged(nameof(Severity));
            }
        }

        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
