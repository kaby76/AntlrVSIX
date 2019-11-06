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

        public static bool GetBoolean(string option, bool default_value)
        {
            Initialize();
            return persistent_settings.GetBoolean("AntlrVSIX", option, default_value);
        }

        public static string GetString(string option, string default_value)
        {
            Initialize();
            return persistent_settings.GetString("AntlrVSIX", option, default_value);
        }

        public static int GetInt32(string option, int default_value)
        {
            Initialize();
            return persistent_settings.GetInt32("AntlrVSIX", option, default_value);
        }

        public static bool EverInitialized()
        {
            return persistent_store_initialized;
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
            initialized = true;
        }
    }
}
