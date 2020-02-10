namespace LanguageServer
{
    //using Options;
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
        private static IGrammarDescription _antlr = new AntlrGrammarDescription();
        public static List<string> AllLanguages
        {
            get
            {
                List<string> result = new List<string>();
                result.Add(_antlr.Name);
                return result;
            }
        }
        
        public static IGrammarDescription Create(string ffn)
        {
            if (_antlr.IsFileType(ffn))
            {
                return _antlr;
            }
            return null;
        }
    }
}
