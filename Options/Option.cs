namespace Options
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text.Json;
    using System.Linq;

    public class Option
    {
        private static string antlr_options_file_name = "";
        private static readonly string CorpusLocation = s == null ? "" : s;
        private static Dictionary<string, object> defaults = new Dictionary<string, object>()
        {
            {"IncrementalReformat", true },
            {"RestrictedDirectory", true },
            {"GenerateVisitorListener", false },
            {"OverrideAntlrPluggins", true },
            {"OptInLogging", false },
            {"CorpusLocation", CorpusLocation },
            {"AntlrTerminal", "Purple" },
            {"AntlrNonterminal", "Blue" },
            {"AntlrComment", "Green" },
            {"AntlrKeyword", "Red" },
            {"AntlrLiteral", "LightGreen" },
            {"AntlrMode", "Salmon" },
            {"AntlrChannel", "Coral" }
        };
        private static readonly string home = System.Environment.GetEnvironmentVariable("HOMEPATH");
        private static bool initialized = false;
        private static readonly string s = System.Environment.GetEnvironmentVariable("CORPUS_LOCATION");
        public static bool GetBoolean(string option)
        {
            Initialize();
            bool default_value = false;
            defaults.TryGetValue(option, out object value);
            if (value == null)
            {
                default_value = false;
            }
            else
            {
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.Converters.Add(new ObjectToBoolConverter());
                object obj = JsonSerializer.Deserialize<object>(value.ToString().ToLower(), options);
                default_value = (bool)obj;
            }
            return default_value;
        }

        public static int GetInt32(string option)
        {
            Initialize();
            int default_value = 0;
            defaults.TryGetValue(option, out object value);
            if (value == null)
            {
                default_value = 0;
            }
            else
            {
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.Converters.Add(new ObjectToIntConverter());
                object obj = JsonSerializer.Deserialize<object>(value.ToString().ToLower(), options);
                default_value = (int)obj;
            }
            return default_value;
        }

        public static string GetString(string option)
        {
            Initialize();
            string default_value = "";
            defaults.TryGetValue(option, out object value);
            if (value == null)
            {
                default_value = "";
            }
            else
            {
                default_value = value.ToString();
            }
            return default_value;
        }

        public static void SetBoolean(string option, bool value)
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

        public static void SetString(string option, string value)
        {
            Initialize();
            defaults[option] = value;
            Write();
        }
        private static void Initialize()
        {
            if (initialized)
            {
                return;
            }

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
                    string jsonString = File.ReadAllText(antlr_options_file_name);
                    var file_values = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonString);
                    // Sum defaults and file_values.
                    var new_list = new Dictionary<string, object>(defaults);
                    foreach (var p in file_values)
                    {
                        new_list[p.Key] = p.Value;
                    }
                    bool same = file_values.Count == new_list.Count && !file_values.Except(new_list).Any();
                    defaults = new_list;
                    if (! same)
                    {
                        Write();
                    }
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
                string jsonString = JsonSerializer.Serialize(defaults);
                File.WriteAllText(antlr_options_file_name, jsonString);
            }
            initialized = true;
        }
    }
}
