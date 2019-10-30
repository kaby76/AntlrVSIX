namespace Logger
{
    using NLog;
    using NLog.Targets;
    using NLog.Config;

    public class Log
    {
        private static bool initialized = false;

        public static void Initialize()
        {
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
