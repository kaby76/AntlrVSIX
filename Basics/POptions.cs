namespace Basics
{
    using Microsoft.VisualStudio.Settings;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;
    using Microsoft.VisualStudio.Shell.Settings;
    using System;
    using System.Collections.Generic;
    using IServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

    public class POptions
    {
        static WritableSettingsStore persistent_settings;
        static bool initialized = false;
        static bool persistent_store_initialized = false;

        public static void SetBoolean(string option, bool value)
        {
            Initialize();
            IEnumerable<string> collection = persistent_settings.GetSubCollectionNames("AntlrVSIX");
            persistent_settings.SetBoolean("AntlrVSIX", option, value);
        }

        public static void SetString(string option, string value)
        {
            Initialize();
            IEnumerable<string> collection = persistent_settings.GetSubCollectionNames("AntlrVSIX");
            persistent_settings.SetString("AntlrVSIX", option, value);
        }

        public static void SetInt32(string option, int value)
        {
            Initialize();
            IEnumerable<string> collection = persistent_settings.GetSubCollectionNames("AntlrVSIX");
            persistent_settings.SetInt32("AntlrVSIX", option, value);
        }

        public static bool GetBoolean(string option)
        {
            Initialize();
            bool default_value = false;
            defaults.TryGetValue(option, out object value);
            if (value == null)
                default_value = false;
            else if (value is bool)
            {
                default_value = (bool)value;
            }
            return persistent_settings.GetBoolean("AntlrVSIX", option, default_value);
        }

        public static string GetString(string option)
        {
            Initialize();
            string default_value = "";
            defaults.TryGetValue(option, out object value);
            if (value == null)
                default_value = "";
            else if (value is string)
            {
                default_value = (string)value;
            }
            return persistent_settings.GetString("AntlrVSIX", option, default_value);
        }

        private static string s = System.Environment.GetEnvironmentVariable("CORPUS_LOCATION");
        private static string CorpusLocation = s == null ? "" : s;

        private static Dictionary<string, object> defaults = new Dictionary<string, object>()
        {
            {"IncrementalReformat", true },
            {"NonInteractiveParse", false },
            {"RestrictedDirectory", true },
            {"GenerateVisitorListener", false },
            {"OverrideAntlrPluggins", true },
            {"OverrideJavaPluggins", true },
            {"OverridePythonPluggins", true },
            {"OverrideRustPluggins", true },
            {"OptInLogging", 0 },
            {"CorpusLocation", CorpusLocation },
        };



        public static int GetInt32(string option)
        {
            Initialize();
            int default_value = 0;
            defaults.TryGetValue(option, out object value);
            if (value == null)
                default_value = 0;
            else if (value is Int32)
            {
                default_value = (int)value;
            }
            return persistent_settings.GetInt32("AntlrVSIX", option, default_value);
        }

        private static void Initialize()
        {
            if (initialized) return;
            var dte2 = Package.GetGlobalService(typeof(SDTE));
            IServiceProvider isp = (IServiceProvider)dte2;
            ServiceProvider sp = new ServiceProvider(isp);
            var settingsManager = new ShellSettingsManager(sp);
            persistent_settings = settingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);
            try
            {
                IEnumerable<string> collection = persistent_settings.GetSubCollectionNames("AntlrVSIX");
                persistent_store_initialized = true;
            }
            catch (Exception)
            {
                persistent_settings.CreateCollection("AntlrVSIX");
                persistent_store_initialized = false;
            }
            if (!persistent_store_initialized)
            {
                defaults.TryGetValue("RestrictedDirectory", out object RestrictedDirectory);
                POptions.SetBoolean("RestrictedDirectory", (bool)RestrictedDirectory);

                defaults.TryGetValue("NonInteractiveParse", out object NonInteractiveParse);
                POptions.SetBoolean("NonInteractiveParse", (bool)NonInteractiveParse);

                defaults.TryGetValue("GenerateVisitorListener", out object GenerateVisitorListener);
                POptions.SetBoolean("GenerateVisitorListener", (bool)GenerateVisitorListener);

                defaults.TryGetValue("CorpusLocation", out object CorpusLocation);
                POptions.SetString("CorpusLocation", (string)CorpusLocation);

                defaults.TryGetValue("IncrementalReformat", out object IncrementalReformat);
                POptions.SetBoolean("IncrementalReformat", (bool)IncrementalReformat);

                defaults.TryGetValue("OverrideAntlrPluggins", out object OverrideAntlrPluggins);
                POptions.SetBoolean("OverrideAntlrPluggins", (bool)OverrideAntlrPluggins);

                defaults.TryGetValue("OverrideJavaPluggins", out object OverrideJavaPluggins);
                POptions.SetBoolean("OverrideJavaPluggins", (bool)OverrideJavaPluggins);

                defaults.TryGetValue("OverridePythonPluggins", out object OverridePythonPluggins);
                POptions.SetBoolean("OverridePythonPluggins", (bool)OverridePythonPluggins);

                defaults.TryGetValue("OverrideRustPluggins", out object OverrideRustPluggins);
                POptions.SetBoolean("OverrideRustPluggins", (bool)OverrideRustPluggins);

                defaults.TryGetValue("OptInLogging", out object OptInLogging);
                POptions.SetInt32("OptInLogging", (int)OptInLogging);
            }
            initialized = true;
        }
    }
}
