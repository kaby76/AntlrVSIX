namespace LanguageServer
{
    using Basics;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    public class ManualAssemblyResolver : IDisposable
    {

        private readonly List<Assembly> _assemblies;

        public ManualAssemblyResolver()
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(OnAssemblyResolve);
            _assemblies = new List<Assembly>();
        }

        public void Add(Assembly assembly)
        {
            _assemblies.Add(assembly);
        }

        public ManualAssemblyResolver(Assembly assembly)
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(OnAssemblyResolve);
            if (assembly == null)
                throw new ArgumentNullException("assembly");
            _assemblies = new List<Assembly>();
            _assemblies.Add(assembly);
        }

        public void Dispose()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= OnAssemblyResolve;
        }

        private Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            foreach (Assembly assembly in _assemblies)
            {
                if (args.Name == assembly.FullName)
                {
                    return assembly;
                }
            }

            return null;
        }
    }

    public class GrammarDescriptionFactory
    {
        private static List<IGrammarDescription> _list_of_languages = new List<IGrammarDescription>();

        private static IGrammarDescription _antlr  = Register("LanguageServer.Antlr.dll");
        private static IGrammarDescription _java   = Register("LanguageServer.Java.dll");
        private static IGrammarDescription _python = Register("LanguageServer.Python.dll");
        private static IGrammarDescription _rust = Register("LanguageServer.Rust.dll");
        private static ManualAssemblyResolver _resolver;

        public static IGrammarDescription Register(string assembly_ffn)
        {
            if (_resolver == null)
            {
                 _resolver = new ManualAssemblyResolver();
            }
            // Check if assembly_ffn is a full path file name. It really may not be.
            if (!System.IO.Path.IsPathRooted(assembly_ffn))
            {
                // Get directory of this dll, and start searching there.
                var t = typeof(GrammarDescriptionFactory);
                var a = t.Assembly;
                var p = System.IO.Path.GetDirectoryName(a.Location);

                var files = System.IO.Directory.GetFiles(p);
                foreach (var f in files)
                {
                    if (System.IO.File.Exists(f) && System.IO.Path.GetExtension(f) == ".dll")
                    {
                        var assembly = Assembly.LoadFile(f);
                        _resolver.Add(assembly);
                    }
                }
                assembly_ffn = p + System.IO.Path.DirectorySeparatorChar + assembly_ffn;
            }
            try
            {
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
            }
            catch (ReflectionTypeLoadException ex)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var exSub in ex.LoaderExceptions)
                {
                    sb.AppendLine(exSub.Message);
                    var exFileNotFound = exSub as System.IO.FileNotFoundException;
                    if (exFileNotFound != null)
                    {
                        if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                        {
                            sb.AppendLine("Fusion Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }
                    sb.AppendLine();
                }
                string errorMessage = sb.ToString();
                //Display or log the error based on your application.
            }
            return null;
        }

        public static IGrammarDescription Create(string ffn)
        {
            foreach (var gd in _list_of_languages)
            {
                if (gd.IsFileType(ffn))
                {
                    if (gd.Name.ToLower() == "antlr" && XOptions.GetProperty("OverrideAntlrPluggins") != "true")
                        return null;
                    if (gd.Name.ToLower() == "java" && XOptions.GetProperty("OverrideJavaPluggins") != "true")
                        return null;
                    if (gd.Name.ToLower() == "python" && XOptions.GetProperty("OverridePythonPluggins") != "true")
                        return null;
                    if (gd.Name.ToLower() == "rust" && XOptions.GetProperty("OverrideRustPluggins") != "true")
                        return null;
                    return gd;
                }
            }
            return null;
        }
    }
}
