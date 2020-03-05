namespace Server
{
    using Microsoft.VisualStudio.LanguageServer.Protocol;
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;

    internal class Program
    {
        private string logMessage;
        private readonly ObservableCollection<DiagnosticTag> tags = new ObservableCollection<DiagnosticTag>();
        private LSPServer languageServer;
        private string responseText;
        private string currentSettings;
        private MessageType messageType;

        public static void Main(string[] args)
        {
            TimeSpan delay = new TimeSpan(0, 0, 0, 20);
            //Console.Error.WriteLine("Waiting " + delay + " seconds...");
            //Thread.Sleep((int)delay.TotalMilliseconds);
            Console.Error.WriteLine("Starting");
            Program program = new Program();
            program.MainAsync(args).GetAwaiter().GetResult();
        }

        private async Task MainAsync(string[] args)
        {
            System.IO.Stream stdin = Console.OpenStandardInput();
            System.IO.Stream stdout = Console.OpenStandardOutput();
            languageServer = new LSPServer(stdout, stdin);
            languageServer.Disconnected += OnDisconnected;
            languageServer.PropertyChanged += OnLanguageServerPropertyChanged;
            Tags.Add(new DiagnosticTag());
            LogMessage = string.Empty;
            ResponseText = string.Empty;
            await Task.Delay(-1);
        }

        private void OnLanguageServerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("CurrentSettings"))
            {
                CurrentSettings = languageServer.CurrentSettings;
            }
        }

        internal void SendLogMessage()
        {
            languageServer.LogMessage(arg: null, message: LogMessage, messageType: MessageType);
        }

        internal void SendMessage()
        {
            languageServer.ShowMessage(message: LogMessage, messageType: MessageType);
        }

        internal void SendMessageRequest()
        {
            Task.Run(async () =>
            {
                MessageActionItem selectedAction = await languageServer.ShowMessageRequestAsync(message: LogMessage, messageType: MessageType, actionItems: new string[] { "option 1", "option 2", "option 3" });
                ResponseText = $"The user selected: {selectedAction?.Title ?? "cancelled"}";
            });
        }

        private void OnDisconnected(object sender, System.EventArgs e)
        {
            //MediaTypeNames.Application.Current.Dispatcher.BeginInvokeShutdown(DispatcherPriority.Normal);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<DiagnosticTag> Tags => tags;

        public string LogMessage
        {
            get => logMessage;
            set
            {
                logMessage = value;
                NotifyPropertyChanged(nameof(LogMessage));
            }
        }

        public string ResponseText
        {
            get => responseText;
            set
            {
                responseText = value;
                NotifyPropertyChanged(nameof(ResponseText));
            }
        }

        public string CustomText
        {
            get => languageServer.CustomText;
            set
            {
                languageServer.CustomText = value;
                NotifyPropertyChanged(nameof(CustomText));
            }
        }

        public MessageType MessageType
        {
            get => messageType;
            set
            {
                messageType = value;
                NotifyPropertyChanged(nameof(MessageType));
            }
        }

        public string CurrentSettings
        {
            get => currentSettings;
            set
            {
                currentSettings = value;
                NotifyPropertyChanged(nameof(CurrentSettings));
            }
        }

        public void SendDiagnostics()
        {
            System.Collections.Generic.Dictionary<string, DiagnosticSeverity> diagnosticTags = Tags.ToDictionary((d) => d.Text, (d) => d.Severity);
            languageServer.SetDiagnostics(diagnosticTags);
            languageServer.SendDiagnostics();
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class DiagnosticTag : INotifyPropertyChanged
    {
        private string text;
        private DiagnosticSeverity severity;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Text
        {
            get => text;
            set
            {
                text = value;
                OnPropertyChanged(nameof(Text));
            }
        }

        public DiagnosticSeverity Severity
        {
            get => severity;
            set
            {
                severity = value;
                OnPropertyChanged(nameof(Severity));
            }
        }

        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
