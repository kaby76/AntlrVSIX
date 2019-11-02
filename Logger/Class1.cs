namespace Logger
{
    using NLog;
    using NLog.Targets;
    using NLog.Config;
    using System.Windows.Forms;

    public class Log
    {
        private static bool initialized = false;

        public static void Initialize()
        {
            var options = Basics.XOptions.GetProperty("OptInLogging");
            if (options == "0")
            {
                //DialogResult result = MessageBox.Show("AntlrVSIX would like to send a stack trace of an exception. Can we do that? (You can change this in the AntlrVSIX options at any time.)",
                //    "Warning",
                //    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                //if (result == DialogResult.Yes)
                //{
                //    //code for Yes
                //    Basics.XOptions.SetProperty("OptInLogging", "1");
                //}
                //else if (result == DialogResult.No)
                //{
                //    //code for No
                //    Basics.XOptions.SetProperty("OptInLogging", "2");
                //}
            }
            else if (options == "1")
            {
                // Set up database.
            }

            var config = new LoggingConfiguration();

            // Targets where to log to: File and Console
            var logfile = new FileTarget("logfile") { FileName = "c:\\temp\\file.txt" };

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logfile);

            // Apply config           
            LogManager.Configuration = config;
            logger = LogManager.GetLogger("Example");
            initialized = true;
        }

        static NLog.Logger logger;

        public static void Notify(string message)
        {
            if (!initialized) Initialize();
            logger.Error(message);
        }
    }
}
