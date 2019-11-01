namespace Basics
{
    using System.Collections.Generic;

    public class XOptions
    {
        private static Dictionary<string, string> _properties = new Dictionary<string, string>();

        public static string GetProperty(string name)
        {
            _properties.TryGetValue(name, out string result);
            return result;
        }

        public static void SetProperty(string name, string value)
        {
            _properties[name] = value;
        }
    }
}
