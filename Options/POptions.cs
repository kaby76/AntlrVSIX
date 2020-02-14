using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Options
{
    public class POptions
    {
        static bool initialized = false;
        static string home = System.Environment.GetEnvironmentVariable("HOMEPATH");
        static string antlr_options_file_name = "";
        private static string s = System.Environment.GetEnvironmentVariable("CORPUS_LOCATION");
        private static string CorpusLocation = s == null ? "" : s;
        private static Dictionary<string, object> defaults = new Dictionary<string, object>()
        {
            {"IncrementalReformat", true },
            {"RestrictedDirectory", true },
            {"GenerateVisitorListener", false },
            {"OverrideAntlrPluggins", true },
            {"OptInLogging", 0 },
            {"CorpusLocation", CorpusLocation },
        };


        public static void SetBoolean(string option, bool value)
        {
            Initialize();
            defaults[option] = value;
            Write();
        }

        public static void SetString(string option, string value)
        {
            Initialize();
            defaults[option] = value;
            Write();
        }

        public static void SetInt32(string option, int value)
        {
            Initialize();
            defaults[option] = value;
            Write();
        }

        public static bool GetBoolean(string option)
        {
            Initialize();
            bool default_value = false;
            defaults.TryGetValue(option, out object value);
            if (value == null)
                default_value = false;
            else
            {
                var options = new JsonSerializerOptions();
                options.Converters.Add(new ObjectToBoolConverter());
                object obj = JsonSerializer.Deserialize<object>(value.ToString().ToLower(), options);
                default_value = (bool)obj;
            }
            return default_value;
        }

        public static string GetString(string option)
        {
            Initialize();
            string default_value = "";
            defaults.TryGetValue(option, out object value);
            if (value == null)
                default_value = "";
            else
            {
                default_value = value.ToString();
            }
            return default_value;
        }

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
            return default_value;
        }


        private static void Initialize()
        {
            if (initialized) return;
            Read();
        }

        private static void Read()
        {
            antlr_options_file_name = home
                                      + Path.DirectorySeparatorChar
                                      + ".antlrvsixrc";
            if (Path.IsPathRooted(antlr_options_file_name))
            {
                if (File.Exists(antlr_options_file_name))
                {
                    var jsonString = File.ReadAllText(antlr_options_file_name);
                    defaults = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonString);
                }
                else
                {
                    Write();
                }
            }
            initialized = true;
        }

        private static void Write()
        {
            if (Path.IsPathRooted(antlr_options_file_name))
            {
                var jsonString = JsonSerializer.Serialize(defaults);
                File.WriteAllText(antlr_options_file_name, jsonString);
            }
            initialized = true;
        }
    }
}
