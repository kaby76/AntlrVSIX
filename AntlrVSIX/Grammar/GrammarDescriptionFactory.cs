using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using AntlrVSIX.GrammarDescription;

namespace AntlrVSIX.Grammar
{
    class GrammarDescriptionFactory
    {
        private static List<IGrammarDescription> _list_of_languages = new List<IGrammarDescription>();

        private static IGrammarDescription _antlr  = Register("AntlrVSIX.GrammarDescription.Antlr.dll");
        private static IGrammarDescription _java   = Register("AntlrVSIX.GrammarDescription.Java.dll");
        private static IGrammarDescription _python = Register("AntlrVSIX.GrammarDescription.Python.dll");

        public static IGrammarDescription Register(string assembly_ffn)
        {
            // Check if assembly_ffn is a full path file name. It really may not be.
            if (!System.IO.Path.IsPathRooted(assembly_ffn))
            {
                // Get directory of this dll, and start searching there.
                var t = typeof(GrammarDescriptionFactory);
                var a = t.Assembly;
                var p = System.IO.Path.GetDirectoryName(a.Location);
                assembly_ffn = p + System.IO.Path.DirectorySeparatorChar + assembly_ffn;
            }

            var assembly = Assembly.LoadFile(assembly_ffn);
            foreach (Type type in assembly.GetTypes())
            {
                var b = type.GetInterfaces().Any(i => i == typeof(IGrammarDescription));
                if (b)
                {
                    var result = Activator.CreateInstance(type);
                    var gd = result as IGrammarDescription;
                    _list_of_languages.Add(gd);
                    return gd;
                }
            }
            return null;
        }

        public static IGrammarDescription Create(string ffn)
        {
            foreach (var gd in _list_of_languages)
            {
                if (gd.IsFileType(ffn)) return gd;
            }
            return null;
        }

    }
}
